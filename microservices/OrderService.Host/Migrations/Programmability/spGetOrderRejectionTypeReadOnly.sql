create PROCEDURE [dbo].[spGetOrderRejectionTypeReadOnly]
AS
BEGIN
 select * from OrderRejectionTypeReadOnly with (nolock)
END