create PROCEDURE [dbo].[spGetOrderStatusTypeReadOnly]
AS
BEGIN
	select * from OrderStatusTypeReadOnly with (nolock)
END