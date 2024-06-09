 UPDATE CompanyProduction SET CompanyId = ( SELECT isnull(CompanyId,0) FROM  dbo.AbpUsers u
WHERE u.UID = CompanyProduction.CreatorId )