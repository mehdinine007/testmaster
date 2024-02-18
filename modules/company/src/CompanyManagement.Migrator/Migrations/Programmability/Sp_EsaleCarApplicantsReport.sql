create PROCEDURE [dbo].[Sp_EsaleCarApplicantsReport]
	@NationalCode varchar(11)
AS
begin
	DROP TABLE IF EXISTS #TmpOrderCopmany
	DROP TABLE IF EXISTS #TmpRes
	select ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode,cod.CompanyId ORDER BY cod.id desc ) rownum 
			,Id = cod.Id
			,cod.NationalCode
			,CarDesc = REPLACE(REPLACE(cod.CarDesc, NCHAR(1610), NCHAR(1740)),NCHAR(1603), NCHAR(1705))
			,OrganizationId = cod.CompanyId
			,OrganizationTitle = org.Title
	into #TmpOrderCopmany
	from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
	inner join [OrderDb].dbo.Organization org  on org.Id = cod.CompanyId
	where cod.NationalCode = @NationalCode 
		 ---------------------------------------------------------------------------------
	select * into #TmpRes from (
	select 
		Id
	   ,NationalCode
	   ,OrganizationTitle
	   ,CarDesc 
	from #TmpOrderCopmany
	where rownum = 1
	union
	select 
		Id = co.Id
	   ,u.NationalCode
	   ,OrganizationTitle = com.Title
	   ,CarDesc = p.Title
    from [OrderDb].dbo.CustomerOrder o
	inner join [OrderDb].dbo.AbpUsers u on u.UID = o.Userid
	inner join [OrderDb].dbo.SaleDetail sd on sd.Id = o.SaleDetailId
	inner join [OrderDb].dbo.ProductAndCategory p on p.Id = sd.ProductId
	inner join [OrderDb].dbo.ProductAndCategory com on com.Code = left(p.Code,4)
	left join #TmpOrderCopmany co on co.rownum = 1 and u.NationalCode = co.NationalCode and co.OrganizationId = com.Id and co.CarDesc = p.Title
	where u.NationalCode = @NationalCode and o.OrderStatus = 40
    ) as tb
	select [id] 
		,[کد ملی] = NationalCode
		,[خودروساز] = OrganizationTitle  
		,[عنوان خودرو] = CarDesc
		,[نوع] = case when id is null then 'سامانه' else 'خودروساز' end
	from #TmpRes
	order by [کد ملی]
END
