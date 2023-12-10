if exists(select 1 from sysObjects where upper(Name)= 'spClientOrderReport')
	drop proc spClientOrderReport
Go
CREATE procedure [dbo].[spClientOrderReport]
@companyFilter as nvarchar(max) null,
@productFilter as nvarchar(max) null,
@modelTypeFilter as nvarchar(max) null ,
@fromDateFilter as datetime2 null,
@toDateFilter as datetime2 null,
@saleDetailFilter as nvarchar(max) null,
@saleSchemaFilter as nvarchar(max) null,
--@containsOrderCount as bit, -- 1
--@containsAssignedCount as bit,  -- 2
--@containsAnnouncedCount as bit,  -- 3
--@containsPayedCount as bit,  -- 4
--@containsContractCount as bit, -- 5
--@containsInvoiceCount as bit, -- 6
--@containsAdvocacyRejectionCount as bit, -- 7
--@containsOrderRejectionCount as bit, -- 8
--@containsCancelCount as bit, -- 9
--@containsDeliverCount as bit -- 10
@categories as nvarchar(50)
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

IF OBJECT_ID('tempdb..#cat') IS NOT NULL DROP TABLE #cat
select value as cat_type into #cat
from string_split(@categories,',')
where value <> 0

insert into @tempTb (id, [type])
select value , 1
from string_split(@productFilter,',')
where value <> 0;

insert into @tempTb (id, [type])
select value , 2
from string_split(@companyFilter,',')
where value <> 0;

insert into @tempTb (id, [type])
select value , 1
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
pod.Id as ProductId,
cod.ContRowId, 
cpp.Id CompanyId,
cod.CarDesc, 
cod.DeliveryDate, 
cod.IntroductionDate,
cod.FactorDate,
cod.CarCode,
cmp.Title,
cod.Vin,
cod.BodyNumber,
cod.InviteDate,
cpp.Id as companypayedpriceid,
cod.IsCanceled as IsCanceled,
co.OrderStatus,
co.id OrderId,
usr.Id as UserRejectionAdvocacyId
from ProductAndCategory as pod
	inner join SaleDetail as sd 
		on pod.Id = sd.ProductId
	inner join CustomerOrder as co
		on co.SaleDetailId = sd.Id
	inner join ProductAndCategory as cmp
		on cmp.Code = left(pod.Code,4) and cmp.LevelId = 1
	inner join ProductAndCategory_CarTip as pod_c
		on pod_c.ProductId = pod.Id
    inner join AbpUsers as usr 
		on usr.UID = co.userid	
	left join esaledb..ClientsOrderDetailByCompany as cod
		on cod.CarCode = pod_c.CarTipId and cod.NationalCode = usr.NationalCode
	left join esaledb..CompanyPaypaidPrices as cpp
		on cpp.ClientsOrderDetailByCompanyId = cod.Id
where (cod.IsDeleted is null or cod.IsDeleted <> 1)
and pod.IsDeleted <> 1
and (cpp.IsDeleted is null or cpp.IsDeleted <> 1)
and cmp.IsDeleted <> 1
and co.IsDeleted <> 1
and sd.IsDeleted <> 1
and usr.IsDeleted <> 1
and pod_c.IsDeleted <> 1
and (@productFilter is null or exists (select 1 from @tempTb temp where temp.id = pod.Id and temp.[type] = 1))
and (@companyFilter is  null or   exists(select 1 from @tempTb temp where temp.id = cmp.id and temp.[type] = 2))
and (@modelTypeFilter is null or  exists(select 1 from @tempTb temp where temp.id = sd.ESaleTypeId and temp.[type] = 3))
and (@fromdateFilter is null or co.Creationtime>=@fromdateFilter)
and (@todateFilter is null or co.Creationtime<=@todateFilter)
and (@saleDetailFilter is null or exists(select 1 from @tempTb temp where temp.id = sd.Id and temp.[type] = 4))
and (@saleSchemaFilter is null or exists(select 1 from @temptb temp where temp.id = sd.saleid and temp.[type] = 5))
)

select 
IIF(exists(select 1 from #cat where cat_type = 6), count(case when r.FactorDate is not null and r.rownum = 1 then 1 else null end), null) as InvoiceCount,
IIF(exists(select 1 from #cat where cat_type = 1), count(distinct r.orderid), null) as OrderCount,
IIF(exists(select 1 from #cat where cat_type = 2) , count(case when (r.vin is not null and r.rownum = 1 or r.bodynumber is not null )and r.rownum =1 then 1 else null end), null) as AssignCount,
IIF(exists(select 1 from #cat where cat_type = 3) , count(case when r.InviteDate is not null and r.rownum = 1 then 1 else null end), null) as AnnouncementCount,
IIF(exists(select 1 from #cat where cat_type = 4) , count(case when r.companypayedpriceid is not null and r.rownum = 1 then 1 else null end), null) as PaymentCount,
IIF(exists(select 1 from #cat where cat_type = 5), count(case when r.ContRowId is not null and r.rownum = 1 then 1 else null end), null) as ContractCount,
IIF(exists(select 1 from #cat where cat_type = 10) , count(case when r.DeliveryDate is not null and r.rownum =1 then 1 else null end), null) as DeliverCount,
IIF(exists(select 1 from #cat where cat_type = 9) , count(r.IsCanceled), null) as Cancelcount,
IIF(exists(select 1 from #cat where cat_type = 8) , count(case when r.orderstatus = 20 then 1 else null end), null) as OrderRejectioncount,
IIF(exists(select 1 from #cat where cat_type = 7), count(case when r.UserRejectionAdvocacyId is not null then 1 else null end), null) as AdvocacyRejectioncount
from Res as r
--where r.rownum = 1
group by  r.ProductId
--order by r.CarCode
end;
