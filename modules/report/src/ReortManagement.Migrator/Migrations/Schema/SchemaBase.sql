if not exists (select [schema_id] from [sys].[schemas] where [name] = 'Rpt')
begin
  exec(N'create SCHEMA [Rpt]');
end
