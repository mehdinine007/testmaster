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
 from  CustomerOrder 
inner join SaleDetail 
on CustomerOrder.SaleDetailId=SaleDetail.Id 
inner join ProductAndCategory 
on SaleDetail.ProductId=ProductAndCategory.Id 
inner join ProductAndCategory as company 
on company.Code=LEFT (ProductAndCategory.Code,4)
where     CustomerOrder.UserId=@userId  and CustomerOrder.IsDeleted=0

END 
