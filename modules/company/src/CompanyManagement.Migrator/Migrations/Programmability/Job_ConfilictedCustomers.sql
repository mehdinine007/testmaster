create PROCEDURE [dbo].[Job_ConfilictedCustomers]
AS
BEGIN
	DROP TABLE IF EXISTS [CompanyDb].dbo.ConfilictedCustomers
	
	;WITH Res
		AS
		(
		select   ROW_NUMBER() OVER(PARTITION BY  cod.NationalCode, cod.creatorid ORDER BY cod.id, cs.EndTurnDate desc ) rownum ,

		cod.NationalCode,cod.ContRowId, cod.CarDesc, cp.Id cpid, cod.DeliveryDate, cod.IntroductionDate, cs.EndTurnDate
		,cod.FactorDate, cod.CreatorId
			from [CompanyDb].dbo.ClientsOrderDetailByCompany cod
			left join [CompanyDb].dbo.CompanyPaypaidPrices cp
			on cp.ClientsOrderDetailByCompanyId = cod.Id
			left join [CompanyDb].dbo.CompanySaleCallDates cs
			on cs.ClientsOrderDetailByCompanyId = cod.Id
			inner join [OrderDb].dbo.AbpUsers us
			on us.NationalCode = cod.NationalCode
 


		)
		select  CarDesc, NationalCode, count(cpid) variz, count(case when IntroductionDate is not null then 1 else null end) farakhan
		,count(case when DeliveryDate is not null then 1 else null end) tahvil 
		,count(case when FactorDate is not null then 1 else null end) Factor 
		,count(case when getdate() + 5 > EndTurnDate then 1 else 0 end) duration
		,max(EndTurnDate) EndTurnDate
		,max(DeliveryDate) DeliveryDate
		,max(FactorDate) FactorDate
		,max(CreatorId) CreatorId
		into ConfilictedCustomers
		--,count(case when 
		from Res
		where rownum = 1
		group by  CarDesc,NationalCode
		order by NationalCode,CarDesc
END
