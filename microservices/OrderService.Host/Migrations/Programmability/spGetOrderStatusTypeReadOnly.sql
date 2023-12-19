if exists(select 1 from sysObjects where Upper(Name)= 'spGetOrderStatusTypeReadOnly')
  drop  proc  spGetOrderStatusTypeReadOnly
GO
create PROCEDURE [dbo].[spGetOrderStatusTypeReadOnly]
AS
BEGIN

select * from OrderStatusTypeReadOnly with (nolock)


END