update ProductAndCategory
set ParentId = NULL
where LevelId = 2 and ParentId is not null


update ProductAndCategory
set IsDeleted = 1,LastModificationTime = GETDATE()
where LevelId = 1


update ProductLevel 
set IsDeleted = 1,LastModificationTime = GETDATE()
where Id = 1

update ProductAndCategory 
set LevelId =LevelId-1
where IsDeleted=0
