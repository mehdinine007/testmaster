using System;

namespace OrderManagement.Application.Contracts;
public static class RedisConstants
{
    public const string RedisDefaultPrefix = "n:{0} ,c:{1}";

    public const string SaleDetailPrefix = "n:SaleDetail:";
    public const string CommitOrderPrefix = "n:CommitOrder:";
    public const string OrderStepCacheKey = CommitOrderPrefix+"OrderStep_{0}";
    public const string GrpcGetUserById = "n:GrpcGetUsreById:";

    public const string CommitOrderEsaleTypePrefix = "n:CommitOrderEsaleType:";

    public static DateTimeOffset SaleDetailTimeOffset = new DateTimeOffset(DateTime.Now.AddMinutes(20));

    public const string UserRejectionPrefix = "n:UserRejection ,c:{0}";

    public static DateTimeOffset UserRejectionTimeOffset = new DateTimeOffset(DateTime.Now.AddMinutes(20));

    public const string ValidateSmsPrefix = "n:SMS,c:{0}";

    public const string CommitOrderIran = "n:CommitOrderIran";

    public const string UserTransactionKey = "traansaction,n:{0},co:{1}";

    public const string OrderStatusCacheKey = "{0}_status";

    public const string AgencyPrefix = "n:Agency:";
    public const string SaleDetailAgenciesCacheName = "AgencySaleDetail_{0}";
    public const string IkcoBearerToken = "IkcoBearerToken";
    public const string OrderStatusPrefix = "n:OrderStatusPrefix:";
    public const string QuestionnaireSurveyReport = "n:Questionnaire:{0}";
    public const string QuestionnaireSurveyReportWithRelatedEntity = "n:Questionnaire:{0}, rlt_entity:{1}";
}