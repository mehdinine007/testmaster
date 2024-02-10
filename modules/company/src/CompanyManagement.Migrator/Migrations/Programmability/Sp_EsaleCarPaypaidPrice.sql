create PROCEDURE [dbo].[Sp_EsaleCarPaypaidPrice]
	@Id int
AS
begin
  select id = 1
      ,[TranDate] =FORMAT(TranDate, 'yyyy/MM/dd', 'fa')
	  ,[EndTurnDate]  = convert(bigint,PayedPrice)
  from CompanyPaypaidPrices
  where ClientsOrderDetailByCompanyId = @Id
end
