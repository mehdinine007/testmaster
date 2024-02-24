if exists(select 1 from sysObjects where Upper(Name)= 'spGetUserRejectionAdvocacy')
  drop  proc  spGetUserRejectionAdvocacy
GO
create PROCEDURE [dbo].[spGetUserRejectionAdvocacy]
	(@nationalCode nvarchar(10))
AS
BEGIN
 select  id,Format(CreationTime,'yyyy/MM/dd-HH:mm:ss','fa')  as CreationTime from UserRejectionAdvocacy where NationalCode=@nationalCode and IsDeleted=0
 UNION 
 select id,Format(CreationTime,'yyyy/MM/dd-HH:mm:ss','fa') as CreationTime from UserRejectionAdvocacy where NationalCode=@nationalCode and IsDeleted=0

END