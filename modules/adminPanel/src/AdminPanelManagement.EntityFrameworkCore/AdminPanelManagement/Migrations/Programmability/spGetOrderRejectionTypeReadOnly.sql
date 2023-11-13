if exists(select 1 from sysObjects where Upper(Name)= 'spGetOrderRejectionTypeReadOnly')
  drop  proc  spGetOrderRejectionTypeReadOnly
GO
create PROCEDURE [dbo].[spGetOrderRejectionTypeReadOnly]
AS
BEGIN
select * from OrderRejectionTypeReadOnly with (nolock)
END