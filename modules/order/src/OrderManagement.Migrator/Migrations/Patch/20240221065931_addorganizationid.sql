if not exists(select 1 from syscolumns c where c.name = 'OrganizationId' and id in (select id from sysObjects where Upper(Name)= 'ProductAndCategory' )) 
  exec(N'ALTER TABLE [ProductAndCategory] ADD [OrganizationId] int');
if exists(select 1 from syscolumns c where c.name = 'OrganizationId' and id in (select id from sysObjects where Upper(Name)= 'ProductAndCategory' )) begin
  exec(N'update ProductAndCategory 
		 set OrganizationId = (select p.id from ProductAndCategory p where p.Code = left(ProductAndCategory.Code,4))
		 where OrganizationId is null') 
end
exec(N'alter table ProductAndCategory alter column [OrganizationId] int not null');




