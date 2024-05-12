create PROCEDURE [dbo].[Sp_CompanyProductPlanChart]
   @OrganizationId int,
   @ProductionDateFrom nvarchar(50), 
   @ProductionDateTo nvarchar(50)  
as
begin
  select CarDesc,Count(1) as [Count] 
  from CompanyProduction
  where IsDeleted = 0 
    and CompanyId = @OrganizationId
and (isnull(@ProductionDateFrom,'')= '' or ProductionDate >= convert(datetime,@ProductionDateFrom))
and (isnull(@ProductionDateTo,'')= '' or ProductionDate <= convert(datetime,@ProductionDateTo))
  group by CarDesc
  order by CarDesc
end
