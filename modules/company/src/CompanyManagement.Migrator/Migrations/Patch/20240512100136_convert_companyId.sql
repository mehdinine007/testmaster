 UPDATE CompanyProduction SET CompanyId = ( SELECT isnull(CompanyId,0) FROM  carsupply_test_order.dbo.AbpUsers c
WHERE c.UID = CompanyProduction.CreatorId )