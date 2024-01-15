create PROCEDURE [dbo].[Sp_EsaleCarApplicantsReport]
	@NationalCode varchar(11),
	@PageIndex int = 0,
	@PageSize int = 20,
	@Type int 

AS
BEGIN
	;WITH Res
		AS
		(
		select   ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode ORDER BY cod.id desc ) rownum 
			,cod.NationalCode
			,cod.ModelType 
			,cod.SaleType 
			,IntroductionDate= FORMAT(cod.IntroductionDate, 'yyyy/MM/dd', 'fa')   
			,TurnDetail = FORMAT(cs.StartTurnDate, 'yyyy/MM/dd', 'fa') + ' - ' + FORMAT(cs.EndTurnDate, 'yyyy/MM/dd', 'fa') 
			,PayedDetail= convert(varchar(250),cp.PayedPrice) + ' - ' + FORMAT(cp.TranDate, 'yyyy/MM/dd', 'fa') 
			,ContRowIdDate = FORMAT(cod.ContRowIdDate, 'yyyy/MM/dd', 'fa') 
			,cod.ContRowId 
			,cod.Vin 
			,InviteDate = FORMAT(cod.InviteDate, 'yyyy/MM/dd', 'fa')
			,DeliveryDate = FORMAT(cod.DeliveryDate, 'yyyy/MM/dd', 'fa')
		    ,cod.FinalPrice
			,cod.CarDesc 
			,cod.CarCode
			,FactorDate = FORMAT(cod.FactorDate, 'yyyy/MM/dd', 'fa')
		 from Esale_Company.dbo.ClientsOrderDetailByCompany cod
		 left join Esale_Company.dbo.CompanyPaypaidPrices cp
		 on cp.ClientsOrderDetailByCompanyId = cod.Id
		 left join Esale_Company.dbo.CompanySaleCallDates cs
		 on cs.ClientsOrderDetailByCompanyId = cod.Id
		 inner join EsaleDb.dbo.AbpUsers us
		 on us.NationalCode = cod.NationalCode
		   and ((isnull(@nationalCode,'') = '' and @Type = 1) or cod.NationalCode = @nationalCode)

		)
		select *
		into #TmpRes
		from Res
		where rownum = 1
		order by NationalCode
		OFFSET (@pageSize * @pageIndex) ROWS FETCH NEXT @pageSize ROWS ONLY
		if @Type = 1 begin
		  select [id] = NationalCode
		        ,[کد ملی] = NationalCode
				,[تاریخ فراخوان] = IntroductionDate  
				,[تاریخ قرارداد] = ContRowIdDate 
				,[تاریخ فاکتور] = FactorDate 
				,[تاریخ تحویل] = DeliveryDate
		  from #TmpRes
		  order by [کد ملی]
		end else if @Type = 2 begin
		  select [کد ملی] = NationalCode
				,[کد نوع فروش] = ModelType 
				,[کد نوع طرح] = SaleType  
				,[تاریخ فراخوان] = IntroductionDate  
				,[جزئیات مهلت پرداخت] = TurnDetail
				,[جزئیات پرداخت] = PayedDetail
				,[تاریخ قرارداد] = ContRowIdDate
				,[شماره قرارداد] = ContRowId
				,[VIN خودرو] = Vin
				,[شماره شاسی خودرو] = ''
				,[تاریخ ارسال دعوتنامه] =InviteDate
				,[تاریخ تحویل خودرو] = DeliveryDate
				,[قیمت نهایی خودرو] = FinalPrice
				,[عنوان خودرو] = CarDesc
				,[کد خودرو] = CarCode
				,[تاریخ فاکتور] = FactorDate
		  from #TmpRes
		end
END
