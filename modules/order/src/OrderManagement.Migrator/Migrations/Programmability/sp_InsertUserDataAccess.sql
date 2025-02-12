create PROCEDURE [dbo].[sp_InsertUserDataAccess]
	 @CompanyId  int,
	  @SaleId  int
AS
BEGIN
DROP TABLE IF EXISTS #Temp
select cu.Userid,sa.ProductId , u.NationalCode 
into #Temp
from CustomerOrder cu 
     inner join AbpUsers u
	 on cu.Userid=u.UID
     inner join SaleDetail   sa
     on cu.SaleDetailId=sa.Id
     inner join ProductAndCategory product
     on sa.ProductId=product.Id
     inner join ProductAndCategory  co
     on co.Code=LEFT (product.Code,4) 
	 where cu.OrderStatus=40 and cu.SaleId=@SaleId  and co.id=@CompanyId
     and co.IsDeleted=  0 and u.IsDeleted = 0 and Product.IsDeleted = 0 and cu.IsDeleted = 0

INSERT INTO [dbo].[UserDataAccess]
           ([UserId]
           ,[Nationalcode]
           ,[RoleTypeId]
           ,[CreationTime]
           ,[IsDeleted])
     SELECT
            [Userid]
           ,[NationalCode]
           ,1
           ,GETDATE()
           ,0
     FROM #Temp  t
	 where not exists(select 1 from UserDataAccess u where t.Nationalcode = u.Nationalcode and u.RoleTypeId = 1)

    INSERT INTO [dbo].[UserDataAccess]
           ([UserId]
           ,[Nationalcode]
           ,[RoleTypeId]
           ,[Data]
           ,[CreationTime]
           ,[IsDeleted])
     SELECT
           [Userid]
           ,[NationalCode]
           ,3
           ,'[{ "ProductId:"'+ CONVERT(NVARCHAR, [ProductId])  +'}]'
           ,GETDATE()
           ,0
    FROM #Temp t
	 where not exists(select 1 from UserDataAccess u where t.Nationalcode = u.Nationalcode and u.RoleTypeId = 3)

END 
