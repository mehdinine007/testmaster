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
from SaleDetail as sd
	inner join ProductAndCategory as ct
		on ct.Id = sd.ProductId
	inner join ProductAndCategory as c
		on c.Code = left(ct.code,4)
	inner join CustomerOrder as co
		on co.SaleDetailId = sd.Id
	left join aucbase.Province as pv
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