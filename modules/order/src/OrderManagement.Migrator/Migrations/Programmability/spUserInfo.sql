create PROCEDURE [dbo].[spUserInfo]
	(@nationalCode nvarchar(10))
AS
BEGIN
	DECLARE @Id bigint
	DECLARE @FirstName nvarchar(100)
	DECLARE @LastName nvarchar(100)
	DECLARE @Mobile nvarchar(100)
	DECLARE @UID uniqueidentifier
	select top(1) @UID=UID,@Id=Id,@FirstName=Name,@LastName=Surname,@Mobile=Mobile from AbpUsers where NationalCode=@nationalCode and IsDeleted=0
	select  isnull(@UID,'00000000-0000-0000-0000-000000000000') as UID ,isnull(@Id,0) as Id,isnull(@FirstName,'') as FirstName,isnull(@LastName,'') as LastName,isnull(@Mobile,'') as Mobile
END