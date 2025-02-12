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
			,@FormatDispaly nvarchar(max)
			,@DateCondition nvarchar(max)
	set @Type = case when @OutputType = 1 then 'DAY' when @OutputType = 2 then'MONTH' when @OutputType = 3 then 'YEAR' else '' end
	set @FormatDispaly =  case when @OutputType = 1 then 'yyyy/MM/dd' when @OutputType = 2 then'yyyy/MM' when @OutputType = 3 then 'yyyy' else '' end
	set @DateCondition = ' #Year = [Year] '
	if @OutputType = 2  
	  set @DateCondition += ' and #Month = [Month] '
    else if @OutputType = 1   
	  set @DateCondition += ' and #Day = [Day] '
	set @Command = '
		DECLARE @StartDate DATE = '''+@StartDate+''',
				@EndDate DATE = '''+@EndDate+'''
		DROP TABLE IF EXISTS #TmpDate
		select [Date]=FORMAT(Date,'''+@FormatDispaly+''',''fa'') 
		      ,[Year]=FORMAT(Date,''yyyy'',''fa'')
		      ,[Month]=FORMAT(Date,''MM'',''fa'')
		      ,[Day]=FORMAT(Date,''dd'',''fa'')
		into #TmpDate
		from (
		  select TOP (DATEDIFF('+@Type+', @StartDate, @EndDate) + 1)
		  	     Date = EOMONTH(DATEADD('+@Type+', ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @StartDate))
		  from    sys.all_objects a
		  CROSS JOIN sys.all_objects b
        ) as tb
	;with TmpOrderCompany
		AS
		(
		select cod.Id,IntroductionYear,IntroductionMonth,IntroductionDay
		      ,FactorYear,FactorMonth,FactorDay
			  ,DeliveryYear,DeliveryMonth,DeliveryDay
			  ,ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode,CompanyId ORDER BY cod.id desc ) rownum 
		from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
		where cod.CompanyId in ('+@CompanyId+')
		'+case when isnull(@ProductId,'') != '' then ' and cod.CarDesc in (select value from openjson('''+@ProductId+''') ) ' else '' end  +'
		'+case when isnull(@ModelType,'') != '' then ' and cod.ModelType in ('+@ModelType+') ' else '' end  +'
		'+case when isnull(@SaleType,'') != '' then ' and cod.SaleType in ('+@SaleType+') ' else '' end  +'
        )

		select Date
		      ,[فراخوان]= (select count(1) from TmpOrderCompany where rownum = 1 and  '+REPLACE(@DateCondition,'#','Introduction')+')
		      ,[واریز]= (select count(1) 
			             from [CompanyDb].dbo.CompanyPaypaidPrices pay
						 inner join TmpOrderCompany co on co.Id = pay.ClientsOrderDetailByCompanyId 
			             where rownum = 1 and '+REPLACE(@DateCondition,'#','pay.Tran')+') 
		      ,[فاکتور]= (select count(1) from TmpOrderCompany where rownum = 1 and '+REPLACE(@DateCondition,'#','Factor')+') 
		      ,[تحویل]= (select count(1) from TmpOrderCompany where rownum = 1 and '+REPLACE(@DateCondition,'#','Delivery')+')  
		from #TmpDate d
		order by date
	'
	exec(@Command)
end