create procedure [dbo].[GetRecentCustomerAndOrder]
	@saleId as int,
	@companyId as int,
	@nationalCode as nvarchar(10)
as BEGIN
select  
co.Id as OrderId,
ct.Id as ProductId,
ct.Title as ProductName,
us.NationalCode,
us.Mobile,
us.Name,
us.Surname,
us.FatherName,
us.BirthCertId,
us.BirthDate,
us.Gender,
pv.Id as ProvienceId,
pv.Name as ProvienceName,
us.Tel,
us.PostalCode,
us.Address,
us.IssuingDate,
us.Shaba,
co.DeliveryDateDescription,
co.OrderRejectionStatus,
c.Title as CompanyName,
sd.ESaleTypeId,
c.Id as Id,
co.TrackingCode,
sd.CompanySaleId
from OrderDb..SaleDetail as sd
	inner join OrderDb..ProductAndCategory as ct
		on ct.Id = sd.ProductId
	inner join OrderDb..ProductAndCategory as c
		on c.Code = left(ct.code,4)
	inner join OrderDb..CustomerOrder as co
		on co.SaleDetailId = sd.Id
	inner join OrderDb..AbpUsers as us
		on us.UID = co.UserId
	left join OrderDb.aucbase.City as ci
		on ci.Id = us.HabitationCityId
	left join OrderDb.aucbase.Province as pv
		on pv.Id = ci.ProvinceId
where
sd.IsDeleted = 0 
and sd.SaleId = @SaleId 
and
ct.IsDeleted = 0 and
c.IsDeleted = 0 and 
c.Id = @companyId and
co.IsDeleted = 0 and 
co.OrderStatus = 10 and
us.IsDeleted = 0 and 
us.NationalCode = @nationalCode
end;
