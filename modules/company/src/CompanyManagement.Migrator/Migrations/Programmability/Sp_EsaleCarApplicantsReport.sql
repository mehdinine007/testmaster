create PROCEDURE [dbo].[Sp_EsaleCarApplicantsReport]
	@NationalCode varchar(11),
	@Type int 

AS
begin
	DROP TABLE IF EXISTS #TmpOrderCopmany
	DROP TABLE IF EXISTS #TmpRes
	select ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode ORDER BY cod.id desc ) rownum 
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
			,OrganizationId = u.CompanyId
			,OrganizationTitle = org.Title
		 into #TmpOrderCopmany
		 from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
		 inner join [OrderDb].dbo.AbpUsers u  on u.UID = cod.CreatorId
		 inner join [OrderDb].dbo.Organization org  on org.Id = u.CompanyId
		 where cod.NationalCode = @NationalCode
		 ---------------------------------------------------------------------------------
		 select * into #TmpRes from (
		 select 
		     Id
			,NationalCode
			,ModelType 
			,SaleType 
			,IntroductionDate   
			,ContRowIdDate 
			,ContRowId 
			,Vin 
			,InviteDate
			,DeliveryDate
		    ,FinalPrice
			,CarDesc 
			,CarCode
			,FactorDate
			,OrganizationId  
			,OrganizationTitle
		 from #TmpOrderCopmany
		 where rownum = 1
		 union
		 select 
		     Id = co.Id
			,u.NationalCode
			,ModelType 
			,SaleType 
			,IntroductionDate   
			,ContRowIdDate 
			,ContRowId 
			,co.Vin 
			,InviteDate
			,co.DeliveryDate
		    ,FinalPrice
			,CarDesc = p.Title
			,CarCode = p.Code
			,FactorDate
			,OrganizationId = com.Id
			,OrganizationTitle = com.Title
         from OrderDb.dbo.CustomerOrder o
		 inner join OrderDb.dbo.AbpUsers u on u.UID = o.Userid
		 inner join OrderDb.dbo.SaleDetail sd on sd.Id = o.SaleDetailId
		 inner join OrderDb.dbo.ProductAndCategory p on p.Id = sd.ProductId
		 inner join OrderDb.dbo.ProductAndCategory com on com.Code = left(p.Code,4)
		 left join #TmpOrderCopmany co on u.NationalCode = co.NationalCode and co.OrganizationId = com.Id and co.CarDesc = p.Title
		 where rownum = 1
		) as tb
		if @Type = 1 begin
		  select [id] = NationalCode
		        ,[کد ملی] = NationalCode
				,[خودروساز] = OrganizationTitle  
				,[عنوان خودرو] = CarDesc
				,[کد خودرو] = CarCode
		  from #TmpRes
		  order by [کد ملی]
		end else if @Type = 2 begin
		  select id 
		        ,[کد ملی] = NationalCode
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
		  from #TmpRes
		end
END
