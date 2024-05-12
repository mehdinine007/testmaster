create PROCEDURE [dbo].[Sp_OrderPaymentInformation]
	@NationalCode NVARCHAR(10)
	,@TransactionCommitDateFrom NVARCHAR(50)
	,@TransactionCommitDateTo NVARCHAR(50)
	,@OrderStatus INT    
	,@ContractNumber NVARCHAR(50)
	,@TransactionId NVARCHAR(100)
	,@AgencyId NVARCHAR(max) 
	,@CustomerOrderId INT
    ,@SaleDetailId NVARCHAR(max)
    ,@SaleId NVARCHAR(max)
    ,@ProductId NVARCHAR(max)
AS

BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON 
	DECLARE  @Command NVARCHAR(max)
	SET @Command = 
	'SELECT 
		ROW_NUMBER() OVER(ORDER BY customerOrder.ID) AS [ردیف]
		,agency.[name] AS [نمایندگی]
		,FORMAT(customerOrder.TransactionCommitDate,''yyyy/MM/dd'',''fa-IR'') AS [تاریخ]
		,case when customerOrder.OrderStatus = 70 then ''موفق'' else ''ناموفق'' end AS [وضعیت پرداخت]
		,customerOrder.TransactionId AS [شماره تراکنش]
		,customerOrder.ID AS [شماره سفارش]
		,SaleDetail.SalePlanDescription AS [برنامه فروش]
		,SaleSchema.Title AS [بخشنامه]
		,productAndCategory.Title AS [نام خودرو]
		,'''' AS [بانک پرداخت]
		,AbpUsers.NationalCode AS [کد ملی]
		,customerOrder.PaymentPrice AS [مبلغ]
		,customerOrder.ContractNumber AS [شماره قرارداد]
		,CustomerOrder.SignTicketId
	FROM 
		OrderDb.dbo.CustomerOrder AS customerOrder
		INNER JOIN OrderDb.dbo.SaleDetail AS SaleDetail ON customerOrder.SaleDetailId = SaleDetail.Id
		INNER JOIN OrderDb.dbo.AbpUsers AS AbpUsers ON customerOrder.Userid = AbpUsers.UID
		INNER JOIN OrderDb.dbo.SaleSchema AS saleSchema ON SaleDetail.SaleId = saleSchema.Id
		INNER JOIN OrderDb.dbo.ProductAndCategory AS productAndCategory ON SaleDetail.ProductId = productAndCategory.Id
		INNER JOIN OrderDb.dbo.Agency AS agency ON customerOrder.AgencyId = agency.id
		INNER JOIN OrderDb.dbo.OrderStatusTypeReadOnly AS orderStatusTypeReadOnly ON customerOrder.OrderStatus = orderStatusTypeReadOnly.Code
	WHERE
		customerOrder.OrderStatus IN (10,70,80)
		AND customerOrder.IsDeleted = 0 AND SaleDetail.IsDeleted = 0 AND AbpUsers.IsDeleted  = 0
		AND saleSchema.IsDeleted  = 0 AND productAndCategory.IsDeleted  = 0  and agency.IsDeleted = 0 
		'+CASE WHEN ISNULL(@NationalCode,'') != '' THEN +' AND AbpUsers.NationalCode = '''+@NationalCode+'''' ELSE'' END +' 
		'+CASE WHEN ISNULL(@TransactionCommitDateFrom,'') != '' THEN +' AND customerOrder.TransactionCommitDate >= '''+@TransactionCommitDateFrom+''' ' ELSE '' END + '
		'+CASE WHEN ISNULL(@TransactionCommitDateTo,'') != '' THEN +' AND customerOrder.TransactionCommitDate <= '''+@TransactionCommitDateTo+''' ' ELSE '' END + '
		'+CASE WHEN ISNULL(@OrderStatus,'') != '' THEN +' AND customerOrder.OrderStatus = '+ CAST(@OrderStatus AS NVARCHAR(50)) ELSE'' END +' 
		'+CASE WHEN ISNULL(@ContractNumber,'') != '' THEN +' AND customerOrder.ContractNumber = '''+@ContractNumber+'''' ELSE'' END +' 
		'+CASE WHEN ISNULL(@AgencyId,'') != '' THEN +' AND customerOrder.AgencyId in ('+@AgencyId+')' ELSE'' END +' 
		'+CASE WHEN ISNULL(@CustomerOrderId,0) != 0 THEN +' AND customerOrder.Id = '+CONVERT(NVARCHAR(20),@CustomerOrderId) ELSE'' END +' 
		'+CASE WHEN ISNULL(@SaleDetailId,'') != '' THEN +' AND customerOrder.SaleDetailId in ('+@SaleDetailId+')' ELSE'' END +' 
		'+CASE WHEN ISNULL(@SaleId,'') != '' THEN +' AND SaleDetail.SaleId in ('+@SaleId+')' ELSE'' END +' 
		'+CASE WHEN ISNULL(@ProductId,'') != '' THEN +' AND productAndCategory.Id in ('+@ProductId+')' ELSE'' END +' 
		'+CASE WHEN ISNULL(@TransactionId,'') != '' THEN +' AND customerOrder.TransactionId = '''+@TransactionId+'''' ELSE'' END +' '

	EXEC(@Command)
		
END 
