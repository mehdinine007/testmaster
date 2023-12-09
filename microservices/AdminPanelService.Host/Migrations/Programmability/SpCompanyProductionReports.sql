if exists(select 1 from sysObjects where upper(Name)= 'SpCompanyProductionReports')
	drop proc SpCompanyProductionReports
Go
create PROCEDURE [dbo].[SpCompanyProductionReports]
	@CompanyId as int,
	@ProductFilter as nvarchar(max) null,
	@SaleTypeId as int,
	@Fromdate as datetime2(7),
	@Todate as datetime2(7)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @products table
	(
		pid int not null
	);
	
	insert into @products (pid)
	select value from string_split(@ProductFilter,',');
		   
	select  com.Title as companyTitle
			,pc.Title as ProductTitle
			,[value] = sum (cp.ProductionCount)
			
	from CompanyProduction cp
	inner join [carsupply_test_order].dbo.ProductAndCategory pc(nolock) on cp.CarCode = pc.Id
	inner join [carsupply_test_order].dbo.ProductAndCategory com(nolock) on com.Code= left(pc.Code,4)
	
	where pc.IsDeleted = 0
	and (@CompanyId is null or com.Id = @CompanyId) and cp.IsDeleted = 0
	and (@ProductFilter is null or exists(select 1 from @products where pid = pc.Id))
	and (@Fromdate is null or @Todate is null or cp.ProductionDate between @Fromdate and @Todate)
	
	
	group by
	 com.Title
	 ,pc.Title

END