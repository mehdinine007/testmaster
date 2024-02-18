create PROCEDURE [dbo].[Sp_EsaleCarApplicantCountChart]
   @CompanyId nvarchar(max)
  ,@ProductId nvarchar(max)
  ,@StartDate nvarchar(50)
  ,@EndDate nvarchar(50) 
  ,@DeliveryDate nvarchar(max) 
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
			   Date = EOMONTH(DATEADD('+@Type+', ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @StartDate))

		into #TmpDate
		from    sys.all_objects a
				CROSS JOIN sys.all_objects b;
   
		DROP TABLE IF EXISTS #TmpGetOrder
		select o.id,o.CreationTime
		into #TmpGetOrder
		from CustomerOrder o
		inner join SaleDetail sd on sd.Id = o.SaleDetailId
		inner join ProductAndCategory p on p.Id = sd.ProductId
		inner join ProductAndCategory com on com.Code = left(p.Code,4)
		where OrderStatus = 40 
		   and com.Id in ('+@CompanyId+')
		   and p.Id in ('+@ProductId+')
 		   and convert(date,o.CreationTime) between @StartDate and @EndDate
		   '+case when isnull(@DeliveryDate,'') != '' then ' and DeliveryDateDescription = '''+@DeliveryDate+''''  else '' end +'
	
		select Date=FORMAT(d.Date,'''+@FormatDispaly+''',''fa''),sum(case when co.Id is not null then 1 else 0 end) 
		from #TmpDate d
		left join #TmpGetOrder co on FORMAT(co.CreationTime,'''+@TypeFormat+''',''fa'') = FORMAT(d.Date,'''+@TypeFormat+''',''fa'')
		group by FORMAT(d.Date,'''+@FormatDispaly+''',''fa'')
		order by Date
	'
	exec(@Command)
end