#region NS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Json;
#endregion

namespace ReportService.Host
{
    public class AuditLogConverter : IAuditLogInfoToAuditLogConverter, ITransientDependency
    {

        #region Initialize
        protected IGuidGenerator GuidGenerator { get; }
        protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected AbpExceptionHandlingOptions ExceptionHandlingOptions { get; }
        #endregion

        #region CTOR
        public AuditLogConverter(IGuidGenerator guidGenerator, IExceptionToErrorInfoConverter exceptionToErrorInfoConverter, IJsonSerializer jsonSerializer, IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
        {
            GuidGenerator = guidGenerator;
            ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
            JsonSerializer = jsonSerializer;
            ExceptionHandlingOptions = exceptionHandlingOptions.Value;
        }
        #endregion

        #region ConvertAsync
        public virtual Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo)
        {
            var auditLogId = GuidGenerator.Create();
            var extraProperties = new ExtraPropertyDictionary();
            if (auditLogInfo.ExtraProperties != null)
            {
                foreach (var pair in auditLogInfo.ExtraProperties)
                {
                    extraProperties.Add(pair.Key, pair.Value);
                }
            }
            var entityChanges = auditLogInfo
                                    .EntityChanges?
                                    .Select(entityChangeInfo => new EntityChange(GuidGenerator, auditLogId, entityChangeInfo, tenantId: auditLogInfo.TenantId))
                                    .ToList()
                                ?? new List<EntityChange>();

            var actions = auditLogInfo
                              .Actions?
                              .Select(auditLogActionInfo => new AuditLogAction(GuidGenerator.Create(), auditLogId, auditLogActionInfo, tenantId: auditLogInfo.TenantId))
                              .ToList()
                          ?? new List<AuditLogAction>();
            var exceptions = "";
            if (auditLogInfo.Exceptions != null)
            {
                var exception = auditLogInfo.Exceptions.FirstOrDefault();
                if (exception != null)
                {
                    exceptions = exception.GetType().FullName + ": " + (exception.Message ?? "");
                    if (exception.InnerException != null)
                    {
                        exceptions += Environment.NewLine + "Inner Exception: " +
                        exception.InnerException.GetType().FullName + ": " +
                        (exception.InnerException.Message ?? "");
                    }
                    if (exception.StackTrace != null)
                    {
                        exceptions += Environment.NewLine + "StackTrace: " + exception.StackTrace;
                    }
                }
            }
            var comments = auditLogInfo
                .Comments?
                .JoinAsString(Environment.NewLine);
            var auditLog = new AuditLog(
                auditLogId,
                auditLogInfo.ApplicationName,
                auditLogInfo.TenantId,
                auditLogInfo.TenantName,
                auditLogInfo.UserId,
                auditLogInfo.UserName,
                auditLogInfo.ExecutionTime,
                auditLogInfo.ExecutionDuration,
                auditLogInfo.ClientIpAddress,
                auditLogInfo.ClientName,
                auditLogInfo.ClientId,
                auditLogInfo.CorrelationId,
                auditLogInfo.BrowserInfo,
                auditLogInfo.HttpMethod,
                auditLogInfo.Url,
                auditLogInfo.HttpStatusCode,
                auditLogInfo.ImpersonatorUserId,
                auditLogInfo.ImpersonatorUserName,
                auditLogInfo.ImpersonatorTenantId,
                auditLogInfo.ImpersonatorTenantName,
                extraProperties,
                entityChanges,
                actions,
                exceptions,
                comments
            );
            return Task.FromResult(auditLog);
        }
        #endregion

    }
}
