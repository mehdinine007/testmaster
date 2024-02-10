create PROCEDURE [dbo].[Sp_EsaleCarCompanyObligationsChart]
   @CompanyId nvarchar(max)
  ,@ProductId nvarchar(max)
  ,@StartDate nvarchar(50)
  ,@EndDate nvarchar(50) 
  ,@ModelType nvarchar(max) 
  ,@SaleType nvarchar(max) 
  ,@OutputType int
as
begin
	declare  @Command nvarchar(max)
			,@Type nvarchar(max)
			,@TypeFormat nvarchar(max)
			,@FormatDispaly nvarchar(max)
	set @Type = case when @OutputType = 1 then 'DAY' when @OutputType = 2 then'MONTH' when @OutputType = 3 then 'YEAR' else '' end
	set @TypeFormat =  case when @OutputType = 1 then 'yyyyMMdd' when @OutputType = 2 then'yyyyMM' when @OutputType = 3 then 'yyyy' else '' end
	set @FormatDispaly =  case when @OutputType = 1 then 'yyyy/MM/dd' when @OutputType = 2 then'yyyy/MM' when @OutputType = 3 then 'yyyy' else '' end

	set @Command = '
		DECLARE @StartDate DATE = '''+@StartDate+''',
				@EndDate DATE = '''+@EndDate+''';
		DROP TABLE IF EXISTS #TmpDate
		select TOP (DATEDIFF('+@Type+', @StartDate, @EndDate) + 1)
			   Date = DATEADD('+@Type+', ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @StartDate)
		into #TmpDate
		from    sys.all_objects a
				CROSS JOIN sys.all_objects b;
        
		DROP TABLE IF EXISTS #TmpOrderCompany
		select IntroductionDate,FactorDate,DeliveryDate
		into #TmpOrderCompany
		from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
		inner join [OrderDb].dbo.AbpUsers u  on u.UID = cod.CreatorId
		where u.CompanyId in ('+@CompanyId+')
		'+case when isnull(@ModelType,'') != '' then ' and cod.ModelType in ('+@ModelType+') ' else '' end  +'
		'+case when isnull(@SaleType,'') != '' then ' and cod.SaleType in ('+@SaleType+') ' else '' end  +'
   
		DROP TABLE IF EXISTS #TmpOrderCompany
		select IntroductionDate,FactorDate,DeliveryDate
		into #TmpCompanyPaypaidPrices
		from [CompanyDb].dbo.CompanyPaypaidPrices pay
		inner join [CompanyDb].dbo.ClientsOrderDetailByCompany cod on cod.id = pay.ClientsOrderDetailByCompanyId
		inner join [OrderDb].dbo.AbpUsers u  on u.UID = cod.CreatorId
		where u.CompanyId in ('+@CompanyId+')
		'+case when isnull(@ModelType,'') != '' then ' and cod.ModelType in ('+@ModelType+') ' else '' end  +'
		'+case when isnull(@SaleType,'') != '' then ' and cod.SaleType in ('+@SaleType+') ' else '' end  +'

		select Date=FORMAT(d.Date,'''+@FormatDispaly+''',''fa'')
		      ,[فراخوان]= (select count(1) from #TmpOrderCompany where FORMAT(IntroductionDate,'''+@TypeFormat+''') = FORMAT(d.Date,'''+@TypeFormat+'''))
		      ,[واریز]= (select count(1) from #TmpCompanyPaypaidPrices where FORMAT(TranDate,'''+@TypeFormat+''') = FORMAT(d.Date,'''+@TypeFormat+''')) 
		      ,[فاکتور]= (select count(1) from #TmpOrderCompany where FORMAT(FactorDate,'''+@TypeFormat+''') = FORMAT(d.Date,'''+@TypeFormat+''')) 
		      ,[تحویل]= (select count(1) from #TmpOrderCompany where FORMAT(DeliveryDate,'''+@TypeFormat+''') = FORMAT(d.Date,'''+@TypeFormat+'''))  
		from #TmpDate d
		group by Date
		order by date
	'
	exec(@Command)
end