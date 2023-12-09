if exists(select 1 from sysObjects where Upper(Name) = 'CustomerCarDeliveryTime')
	drop proc CustomerCarDeliveryTime

Go
Create procedure [dbo].[CustomerCarDeliveryTime]
	-- Add the parameters for the stored procedure here
	 @CompanyId as int,
	  @SaleId as int,
	  @PageNo as int = 0,
	  @Type as int = 0
AS
BEGIN
--return null
    declare @orderStatus as int = 40,
	@PageSize as int = 40000;
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    declare @SystemSaleId as int  = 0;
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--if @SaleId = 5
	--begin
	--	set @orderStatus = 10
	--end
	if @SaleId = 1 or @SaleId = 2
	begin
		set @SystemSaleId = 0
	end
	else
	begin
		set @SystemSaleId = @SaleId
	end
	
	
	if @Type = 0
	begin
if @saleid = 6
		begin
			;WITH Res
AS
(

select  --us.NationalCode, sum(f.PriorityLevel)
  co.Id,co.Id as orderid, ct.Id as cartipid,ct.title, us.NationalCode,us.Mobile,us.Name,us.Surname,us.FatherName,us.BirthCertId,us.BirthDate,us.Gender,f.Radif,
ci.id as CityID,ci.Name as City,pv.Id as ProvinceId, pv.Name as Province, us.Tel, us.PostalCode, us.Address, us.IssuingDate , us.Shaba, co.DeliveryDateDescription, co.OrderRejectionStatus, c.Title as sherkat, sd.ESaleTypeId
,co.SaleId ,co.TrakingCode, co.Vin, co.EngineNo, co.ChassiNo, co.Vehicle
,ROW_NUMBER() OVER(PARTITION BY  us.NationalCode ORDER BY f.PriorityLevel desc ) rownum, f.PriorityLevel
----into #sp19

			 from carsupply_test_order..SaleDetail sd(nolock)
		inner join carsupply_test_order.dbo.ProductAndCategory ct(nolock)
		on sd.ProductId = ct.id	
		inner join carsupply_test_order.dbo.ProductAndCategory c(nolock)
		on c.Code = left(ct.Code,4)
		inner join [carsupply_test_order].dbo.customerorder co(nolock)
		on co.SaleDetailId = sd.id
		--[USERDB].carsupply_test_order.dbo
		inner join [carsupply_test_order].[dbo].[PriorityListSaleSchema] ps
		on ps.saleid = sd.SaleId
		inner join carsupply_test_order.[dbo].abpusers  us(nolock)
		on us.uid = co.userid
		inner join [carsupply_test_order].[dbo].PriorityList f(nolock)
		on f.NationalCode = us.NationalCode
		left join carsupply_test_order.aucbase.City(nolock) ci on ci.Id = us.HabitationCityId
left join carsupply_test_order.aucbase.Province pv(nolock) on pv.Id = us.HabitationProvinceId

		where  co.IsDeleted = 0  
			and co.orderstatus = 10-- in(10,40)
		and ps.saleid = @SaleId 
		and ( ps.PriorityLevel = f.PriorityLevel or f.PriorityLevel is null)
			
		and sd.SaleId = @SaleId 
		and c.Id = @CompanyId 
		and (@SaleId = 1 or co.PriorityId is not null)
		and (@SaleId !=1 or co.PriorityId is null)
)
--select count(0) from res
	select
	r.* 
	from Res r 
	where rownum = 1
	order by sherkat, title,radif
	OFFSET (@PageNo * @PageSize) ROWS FETCH NEXT @PageSize  ROWS ONLY;
end
		else
			begin

				;WITH Res
AS
(

select  --us.NationalCode, sum(f.PriorityLevel)
  co.Id,co.Id as orderid, ct.Id as cartipid,ct.title, us.NationalCode,us.Mobile,us.Name,us.Surname,us.FatherName,us.BirthCertId,us.BirthDate,us.Gender,f.Radif,
ci.id as CityID,ci.Name as City,pv.Id as ProvinceId, pv.Name as Province, us.Tel, us.PostalCode, us.Address, us.IssuingDate , us.Shaba, co.DeliveryDateDescription, co.OrderRejectionStatus, c.Title as sherkat, sd.ESaleTypeId
,co.SaleId ,co.TrakingCode, co.Vin, co.EngineNo, co.ChassiNo, co.Vehicle
,ROW_NUMBER() OVER(PARTITION BY  us.NationalCode ORDER BY f.PriorityLevel desc ) rownum, f.PriorityLevel
----into #sp19

			 from carsupply_test_order..SaleDetail sd(nolock)
		inner join carsupply_test_order.dbo.ProductAndCategory ct(nolock)
		on sd.ProductId = ct.id	
		inner join carsupply_test_order.dbo.ProductAndCategory c(nolock)
		on c.Code = left(ct.Code,4)
		inner join carsupply_test_order.dbo.customerorder co(nolock)
		on co.SaleDetailId = sd.id
		--[USERDB].carsupply_test_order.dbo
		inner join [carsupply_test_order].[dbo].[PriorityListSaleSchema] ps
		on ps.saleid = sd.SaleId
		inner join carsupply_test_order.[dbo].abpusers  us(nolock)
		on us.uid = co.userid
		inner join [esaledb].[dbo].PriorityList f(nolock)
		on f.NationalCode = us.NationalCode
		left join esaledb.aucbase.City(nolock) ci on ci.Id = us.HabitationCityId
left join esaledb.aucbase.Province pv(nolock) on pv.Id = us.HabitationProvinceId

		where  co.IsDeleted = 0  
			and co.orderstatus = 10-- in(10,40)
		and ps.saleid = @SaleId 
		and ( ps.PriorityLevel = f.PriorityLevel or f.PriorityLevel is null)
			
		and sd.SaleId = @SaleId 
		and c.Id = @CompanyId 
		and (@SaleId = 1 or co.PriorityId is not null)
		and (@SaleId !=1 or co.PriorityId is null)
	
	)
	--select count(0) from res
	select
	r.* 
	from Res r 
	where rownum = 1
	order by sherkat, title,radif
	OFFSET (@PageNo * @PageSize) ROWS FETCH NEXT @PageSize  ROWS ONLY;

			end




	end
	else
	begin
		;WITH Res
AS
(

select  --us.NationalCode, sum(f.PriorityLevel)
  co.Id,co.Id as orderid, ct.Id as cartipid,ct.title, us.NationalCode,us.Mobile,us.Name,us.Surname,us.FatherName,us.BirthCertId,us.BirthDate,us.Gender,f.Radif,
ci.id as CityID,ci.Name as City,pv.Id as ProvinceId, pv.Name as Province, us.Tel, us.PostalCode, us.Address, us.IssuingDate , us.Shaba, co.DeliveryDateDescription, co.OrderRejectionStatus, c.Title as sherkat, sd.ESaleTypeId
,co.SaleId ,co.TrakingCode, co.Vin, co.EngineNo, co.ChassiNo, co.Vehicle
,ROW_NUMBER() OVER(PARTITION BY  us.NationalCode ORDER BY f.PriorityLevel desc ) rownum, f.PriorityLevel
----into #sp19

			 from SaleDetail sd(nolock)
		inner join [carsupply_test_order].dbo.ProductAndCategory ct(nolock)
		on sd.ProductId = ct.id	
		inner join [carsupply_test_order].dbo.ProductAndCategory c(nolock)
		on c.Code = left(ct.Code,4)
		inner join [carsupply_test_order].dbo.customerorder co(nolock)
		on co.SaleDetailId = sd.id
		--[USERDB].[carsupply_test_order].dbo
		inner join [dbo].[PriorityListSaleSchema] ps
		on ps.saleid = sd.SaleId
		inner join [dbo].abpusers  us(nolock)
		on us.uid = co.userid
		inner join [dbo].PriorityList f(nolock)
		on f.NationalCode = us.NationalCode
		left join carsupply_test_order.aucbase.City(nolock) ci on ci.Id = us.HabitationCityId
left join carsupply_test_order.aucbase.Province pv(nolock) on pv.Id = us.HabitationProvinceId

		where  co.IsDeleted = 0  
			and co.orderstatus = 10-- in(10,40)
		and ps.saleid = @SaleId
		and ( ps.PriorityLevel = f.PriorityLevel or f.PriorityLevel is null)
			
		and sd.SaleId = @SaleId 
		and c.Id = @CompanyId 
		and (@SaleId= 1 or co.PriorityId is not null)
		and (@SaleId !=1 or co.PriorityId is null)
	)
select r.Title,r.ESaleTypeId, count(r.title) from Res r 
	where rownum = 1
	group by r.Title,ESaleTypeId
	order by r.title
	



	end
END
