create PROCEDURE [dbo].[Sp_EsaleCarPayDuration]
	@Id int
AS
begin
  select id = 1
      ,[StartTurnDate] =FORMAT(StartTurnDate, 'yyyy/MM/dd', 'fa')
	  ,[EndTurnDate]  = FORMAT(EndTurnDate, 'yyyy/MM/dd', 'fa')
  from CompanySaleCallDates
  where ClientsOrderDetailByCompanyId = @Id
end
