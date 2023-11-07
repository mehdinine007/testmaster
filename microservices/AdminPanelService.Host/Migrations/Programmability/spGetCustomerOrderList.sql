if exists(select 1 from sysObjects where Upper(Name)= 'spGetCustomerOrderList')
  drop  proc  spGetCustomerOrderList
GO
create PROCEDURE [dbo].[spGetCustomerOrderList]
	(@userId uniqueidentifier)
AS
BEGIN
select CustomerOrder.SaleDetailId
,CustomerOrder.Id,
CustomerOrder.OrderRejectionStatus,
CustomerOrder.OrderStatus,
CustomerOrder.DeliveryDateDescription,
CustomerOrder.PriorityId,Format(CustomerOrder.CreationTime,'yyyy/MM/dd-HH:mm:ss','fa') as CreationTime
,SaleDetail.ESaleTypeId,
ProductAndCategory.Title as CarName,
ProductAndCategory.Code,
Company.Title  as CompanyName,
 CASE WHEN CustomerOrder.LastModificationTime IS NULL THEN NULL ELSE Format(CustomerOrder.LastModificationTime,'yyyy/MM/dd-HH:mm:ss','fa')  END AS LastModificationTime
 from  [Esale_Order].dbo.CustomerOrder 
inner join [Esale_Order].dbo.SaleDetail 
on CustomerOrder.SaleDetailId=SaleDetail.Id 
inner join [Esale_Order].dbo.ProductAndCategory 
on SaleDetail.ProductId=ProductAndCategory.Id 
inner join [Esale_Order].dbo.ProductAndCategory as company 
on company.Code=LEFT ([Esale_Order].dbo.ProductAndCategory.Code,4)
where     [Esale_Order].dbo.CustomerOrder.UserId=@userId  and [Esale_Order].dbo.CustomerOrder.IsDeleted=0

END
