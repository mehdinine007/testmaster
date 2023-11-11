if exists(select 1 from sysObjects where Upper(Name)= 'spClientOrderReport')
  drop  proc  spClientOrderReport
GO

USE [carsupply-test-company]
GO
/****** Object:  StoredProcedure [dbo].[ClientOrderReport]    Script Date: 10/21/2023 10:14:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spClientOrderReport]
@companyFilter as nvarchar(max) null,
@productFilter as nvarchar(max) null,
@modelTypeFilter as nvarchar(max) null ,
@fromDateFilter as datetime2 null,
@toDateFilter as datetime2 null,
@saleDetailFilter as nvarchar(max) null,
@saleSchemaFilter as nvarchar(max) null,
@containsOrderCount as bit,
@containsAssignedCount as bit,
@containsAnnouncedCount as bit,
@containsPayedCount as bit,
@containsContractCount as bit,
@containsInvoiceCount as bit,
@containsAdvocacyRejectionCount as bit,
@containsOrderRejectionCount as bit,
@containsCancelCount as bit,
@containsDeliverCount as bit
as
begin

--declare @resultTb table
--(
--	CarCode nvarchar(250) not null ,
--	OrderCount bigint null,
--	AssignCount bigint null,
--	AnnouncementCount bigint null,
--	PaymentCount bigint null,
--	ContractCount bigint null,
--	InvoiceCount bigint null,
--	AdvocacyRejectioncount bigint null,
--	OrderRejectioncount bigint null,
--	Cancelcount bigint null,
--	DeliverCount bigint null
--);
declare @tempTb table
(
	id int not null,
	[type] int not null
);

insert into @tempTb (id, [type])
select value , 1
from string_split(@productFilter,',')
where value <> 0;

insert into @tempTb (id, [type])
select value , 2
from string_split(@companyFilter,',')
where value <> 0;

insert into @tempTb (id, [type])
select value , 3
from string_split(@modelTypeFilter,',')
where value <> 0;

insert into @tempTb (id, [type])
select value , 4
from string_split(@saleDetailFilter,',')
where value <> 0;

insert into @tempTb (id, [type])
select value , 5
from string_split(@saleSchemaFilter,',')
where value <> 0;

WITH Res
AS
(
select ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode ORDER BY cod.id desc ) rownum ,
cod.NationalCode,
cod.ContRowId, 
cp.Id CompanyId,
cod.CarDesc, 
cod.DeliveryDate, 
cod.IntroductionDate,
cod.FactorDate,
cod.CarCode,
cmp.Title,
cod.Vin,
cod.BodyNumber,
cod.InviteDate,
cp.Id as companypayedpriceid,
0 as IsCanceled,
co.OrderStatus,
cod.orderid,
ura.Id as UserRejectionAdvocacyId
from ClientsOrderDetailByCompany as cod
	inner join carsupply_test_order..ProductAndCategory_CarTip pcm 
		on pcm.cartipid=cod.CarCode
	inner join carsupply_test_order..ProductAndCategory pac 
		on pac.Id= pcm.productid
	left join CompanyPaypaidPrices as cp
		on cp.ClientsOrderDetailByCompanyId = cod.Id
	inner join carsupply_test_order..ProductAndCategory as cmp
		on left(pac.Code,4) = cmp.Code
	inner join carsupply_demo_user..AbpUsers as usr 
		on usr.NationalCode = cod.NationalCode
	inner join carsupply_test_order..CustomerOrder co
		on co.UserId = usr.UID
	inner join carsupply_test_order..SaleDetail as sd
		on sd.Id = co.SaleId
	left join carsupply_test_order..UserRejectionAdvocacy as ura
		on ura.NationalCode = COD.NationalCode -- check it
where cod.IsDeleted <> 1
and pac.IsDeleted <> 1
and cp.IsDeleted <> 1
and cmp.IsDeleted <> 1
and co.IsDeleted <> 1
and sd.IsDeleted <> 1
and ura.IsDeleted <> 1
and (@companyFilter is  null or   exists(select 1 from @tempTb temp where temp.id = cmp.id and temp.[type] = 2))
and (@productFilter is null or exists (select 1 from @tempTb temp where temp.id = pac.Id and temp.[type] = 1))
and (@modelTypeFilter is null or  exists(select 1 from @tempTb temp where temp.id = sd.ESaleTypeId and temp.[type] = 3))
and (@fromDateFilter is null or  @fromDateFilter <= pac.CreationTime)
and (@toDateFilter is null or  @toDateFilter >= pac.CreationTime)
and (@saleDetailFilter is null or exists(select 1 from @tempTb temp where temp.id = sd.Id and temp.[type] = 4))
and (@saleSchemaFilter is null or exists(select 1 from @temptb temp where temp.id = sd.saleid and temp.[type] = 5))
)

select
r.CarCode,
IIF(@containsInvoiceCount = 0 , null , count(case when r.FactorDate is not null then 1 else null end)) as InvoiceCount,
IIF(@containsOrderCount = 0 , null , count(r.orderid)) as OrderCount,
IIF(@containsAssignedCount = 0 , null , count(case when r.vin is not null then 1 when r.bodynumber is not null then 1 else null end)) as AssignCount,
IIF(@containsAnnouncedCount = 0 , null , count(case when r.InviteDate is not null then 1 else null end)) as AnnouncementCount,
IIF(@containsPayedCount = 0 , NULL , count(case when r.companypayedpriceid is not null then 1 else null end)) as PaymentCount,
IIF(@containsContractCount = 0 ,null , count(case when r.ContRowId is not null then 1 else null end)) as ContractCount,
IIF(@containsDeliverCount = 0 , null , count(case when r.ContRowId is not null then 1 else null end)) as DeliverCount,
IIF(@containsCancelCount = 0 ,null , count(r.IsCanceled)) as Cancelcount,
IIF(@containsOrderRejectionCount = 0 ,null , count(case when r.orderstatus = 20 then 1 else null end)) as OrderRejectioncount,
IIF(@containsAdvocacyRejectionCount = 0 , null , count(case when r.UserRejectionAdvocacyId is not null then 1 else null end)) as AdvocacyRejectioncount
from Res as r
where r.rownum = 1
group by  r.CarCode
order by r.CarCode
end;