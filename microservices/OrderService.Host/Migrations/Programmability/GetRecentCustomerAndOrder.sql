if exists(select 1 from sysObjects where Upper(Name) = 'GetRecentCustomerAndOrder')
	drop proc GetRecentCustomerAndOrder

GO
/****** Object:  StoredProcedure [dbo].[GetRecentCustomerAndOrder]    Script Date: 12/24/2023 1:50:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetRecentCustomerAndOrder]
	@saleId as int,
	@companyId as int,
	@userId uniqueidentifier ,
	@provienceId as int 
as BEGIN
select  
co.Id as OrderId,
ct.Id as ProductId,
ct.Title as ProductName,
pv.Id as ProvienceId,
pv.Name as ProvienceName,
co.DeliveryDateDescription,
co.OrderRejectionStatus,
c.Title as CompanyName,
sd.ESaleTypeId,
c.Id as Id,
co.TrackingCode,
sd.CompanySaleId
from carsupply_test_order..SaleDetail as sd
	inner join carsupply_test_order..ProductAndCategory as ct
		on ct.Id = sd.ProductId
	inner join carsupply_test_order..ProductAndCategory as c
		on c.Code = left(ct.code,4)
	inner join carsupply_test_order..CustomerOrder as co
		on co.SaleDetailId = sd.Id
	left join carsupply_test_order.aucbase.Province as pv
		on pv.Id = @provienceId
where
sd.IsDeleted = 0 
and sd.SaleId = @SaleId 
and
ct.IsDeleted = 0 and
c.IsDeleted = 0 and 
c.Id = @companyId and
co.IsDeleted = 0 and 
co.OrderStatus = 10 and
co.Userid = @userId
end;