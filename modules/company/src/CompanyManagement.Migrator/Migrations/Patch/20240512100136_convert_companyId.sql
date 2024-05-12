 UPDATE CompanyProduction SET CompanyId = ( SELECT isnull(CompanyId,0) FROM  carsupply_test_order.dbo.AbpUsers u
WHERE u.UID = CompanyProduction.CreatorId )