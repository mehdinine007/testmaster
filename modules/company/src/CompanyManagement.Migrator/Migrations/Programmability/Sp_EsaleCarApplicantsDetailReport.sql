create PROCEDURE [dbo].[Sp_EsaleCarApplicantsDetailReport]
	@Id bigint
AS
begin
	DROP TABLE IF EXISTS #TmpOrderCopmany
	select ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode,cod.CompanyId ORDER BY cod.id desc ) rownum 
			,Id = cod.Id
			,cod.NationalCode
			,cod.ModelType 
			,cod.SaleType 
			,IntroductionDate= FORMAT(cod.IntroductionDate, 'yyyy/MM/dd', 'fa')   
			,ContRowIdDate = FORMAT(cod.ContRowIdDate, 'yyyy/MM/dd', 'fa') 
			,cod.ContRowId 
			,cod.Vin 
			,InviteDate = FORMAT(cod.InviteDate, 'yyyy/MM/dd', 'fa')
			,DeliveryDate = FORMAT(cod.DeliveryDate, 'yyyy/MM/dd', 'fa')
		    ,cod.FinalPrice
			,cod.CarDesc 
			,cod.CarCode
			,FactorDate = FORMAT(cod.FactorDate, 'yyyy/MM/dd', 'fa')
			,OrganizationId = cod.CompanyId
			,OrganizationTitle = org.Title
   into #TmpOrderCopmany
   from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
   inner join [OrderDb].dbo.Organization org  on org.Id = cod.CompanyId
   where cod.Id = @Id
   ---------------------------------------------------------------------------------
   select [کد ملی] = NationalCode
		,[کد نوع فروش] = ModelType 
		,[کد نوع طرح] = SaleType  
		,[تاریخ فراخوان] = IntroductionDate  
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
   from #TmpOrderCopmany
END
