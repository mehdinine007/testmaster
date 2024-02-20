create PROCEDURE [dbo].[Sp_EsaleCompanyCarDescList]
   @CompanyId nvarchar(max)
as
begin
	declare  @Command nvarchar(max)
	set @Command = '
	;with TmpOrderCompany
		AS
		(
		select CarDesc
			  ,ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode,CompanyId ORDER BY cod.id desc ) rownum 
		from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
		where cod.CompanyId in ('+@CompanyId+')
        )
		select Id = CarDesc,Title = CarDesc
		from TmpOrderCompany d
		where rownum = 1
		group by CarDesc
		order by Title
	'
	exec(@Command)
end