if not exists (select [schema_id] from [sys].[schemas] where [name] = 'aucbase')
begin
  exec(N'create SCHEMA [aucbase]');
end
