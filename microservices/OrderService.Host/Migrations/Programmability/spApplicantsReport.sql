create proc [dbo].[spApplicantsReport]
@companyFilter as int null, 
@productFilter as nvarchar(max) null,
@modelTypeFilter as int null ,
@fromdateFilter as datetime2 null,
@todateFilter as datetime2 null,
@saleDetailFilter as int null,
@saleSchemaFilter as int null,
@nationalcode nvarchar(10) null,
@categoriesFilter nvarchar(max) null
as
begin
	

		----------------
		declare @products table (pid int not null);
		insert into @products (pid)
		select value
		from string_split(@productFilter,',')
		where value <> 0

			----------------
		declare @categories table (cid int not null);
		insert into @categories (cid)
		select value
		from string_split(@categoriesFilter,',')
		where value <> 0

		----------------
		select distinct  ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode ORDER BY cod.id desc ) rownum ,
		cod.NationalCode
		,cod.SaleType
		,cod.ModelType
		,cod.IntroductionDate
		---
		,cod.InviteDate
		,cod.ContRowIdDate
		,cod.ContRowId
		,cod.vin
		,cod.BodyNumber
		---
		,cod.CooperateBenefit
		,cod.CancelBenefit
		,cod.DelayBenefit
		,cod.DeliveryDate
		,cod.FinalPrice
		,cod.CarDesc
		,cod.CarCode
		,cod.FactorDate
	

		from [CompanyDb]..ClientsOrderDetailByCompany cod 
		inner join ProductAndCategory_CarTip pcm on pcm.cartipid=cod.CarCode
		inner join ProductAndCategory pc on pc.Id= pcm.productid
		inner join ProductAndCategory com on com.Code = left(pc.Code,4)
		inner join AbpUsers as usr on usr.NationalCode = cod.NationalCode
		inner join customerorder co on usr.UID = co.UserId
		left join SaleDetail as sd  on sd.id = co.saledetailid
		inner join [CompanyDb]..CompanyPaypaidPrices cp on cod.Id = cp.ClientsOrderDetailByCompanyId
		--left join Esaledb_new.[dbo].[UserRejectionAdvocacy] ur on  ur.NationalCode = cod.NationalCode



			where	cod.IsDeleted=0
				and pcm.isdeleted=0
				and pc.IsDeleted=0
				and co.IsDeleted=0
				and sd.IsDeleted=0
				and cp.IsDeleted=0
				--and ur.IsDeleted=0

				and (@companyFilter is null or com.Id=@companyFilter)
			
				and  (exists(select 1 from @products ) AND ( pc.Id in (select pid from @products) AND PC.LevelId=4) )

			
				
				and (@modelTypeFilter is null or cod.ModelType=@modelTypeFilter)
				and (@fromdateFilter is null or co.Creationtime>=@fromdateFilter)
				and (@todateFilter is null or co.Creationtime<=@todateFilter)

				and (@saleDetailFilter is null or co.saledetailid=@saleDetailFilter)
				and (@saleSchemaFilter is null or sd.saleid=@saleSchemaFilter)
				and (@nationalcode is null or cod.NationalCode=@nationalcode)



				and ( (exists(select 1 from @categories where cid=1 ) and  usr.Id is not null ) or not exists(select 1 from @categories where cid=1 ))
				and ( (exists(select 1 from @categories where cid=2 ) and cod.OrderId is not null)or not exists(select 1 from @categories where cid=2 ))
				
				and  ((exists(select 1 from @categories where cid=3 ) and  co.OrderStatus = 40)or not exists(select 1 from @categories where cid=3 ))
				and  ((exists(select 1 from @categories where cid=4 ) and cod.IntroductionDate is not null)or not exists(select 1 from @categories where cid=4 ))

				and  ((exists(select 1 from @categories where cid=5 ) and  cp.PayedPrice <> 0) or not exists(select 1 from @categories where cid=5 ))
				and  ((exists(select 1 from @categories where cid = 6 ) and cod.ContRowId <> 0)or not exists(select 1 from @categories where cid=6 ))
				and  ((exists(select 1 from @categories where cid = 7) and cod.FactorDate is not null)or not exists(select 1 from @categories where cid=7))
				and  ((exists(select 1 from @categories where cid = 8) and co.OrderStatus = 20 )or not exists(select 1 from @categories where cid=8 ))
				--and ((exists(select 1 from @categories where cid = 9) and ur.Id is not null )or not exists(select 1 from @categories where cid=9 ))
				and ((exists(select 1 from @categories where cid = 10) and cod.IsCanceled=1)or not exists(select 1 from @categories where cid=10 ))
				and ((exists(select 1 from @categories where cid =11) and cod.DeliveryDate is not null)or not exists(select 1 from @categories where cid=11 ))
				


end