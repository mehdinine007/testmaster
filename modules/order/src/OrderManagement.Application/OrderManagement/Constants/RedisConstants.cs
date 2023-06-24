using System;

namespace OrderManagement.Application.OrderManagement.Constants;

public static class RedisConstants
{
    public const string RedisDefaultPrefix = "n:{0} ,c:{1}";

    public const string SaleDetailPrefix = "n:SaleDetail ,c:{0}";

    public static DateTimeOffset SaleDetailTimeOffset = new DateTimeOffset(DateTime.Now.AddMinutes(20));

    public const string UserRejectionPrefix = "n:UserRejection ,c:{0}";

    public static DateTimeOffset UserRejectionTimeOffset = new DateTimeOffset(DateTime.Now.AddMinutes(20));

    public const string ValidateSmsPrefix = "n:SMS,c:{0}";

    public const string CommitOrderIran = "n:CommitOrderIran";

    public const string UserTransactionKey = "traansaction,n:{0},co:{1}";

    public const string OrderStatusCacheKey = "{0}_status";
}