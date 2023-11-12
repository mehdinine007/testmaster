if exists(select 1 from sysObjects where Upper(Name)= 'GetRecentCustomerAndOrder')
  drop  proc  GetRecentCustomerAndOrder
GO
/****** Object:  StoredProcedure [dbo].[GetRecentCustomerAndOrder]    Script Date: 11/12/2023 10:36:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetRecentCustomerAndOrder]
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
c.Id as Id
from carsupply_test_order..SaleDetail as sd
	inner join carsupply_test_order..ProductAndCategory as ct
		on ct.Id = sd.ProductId
	inner join carsupply_test_order..ProductAndCategory as c
		on c.Code = left(ct.code,4)
	inner join carsupply_test_order..CustomerOrder as co
		on co.SaleDetailId = sd.Id
	inner join carsupply_test_order..AbpUsers as us
		on us.UID = co.UserId
	left join carsupply_test_order.aucbase.City as ci
		on ci.Id = us.HabitationCityId
	left join carsupply_test_order.aucbase.Province as pv
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