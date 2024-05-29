exec(N'declare @priority int = 0
update bank set @priority = priority =@priority + 1 where priority = 0 and IsDeleted = 0')