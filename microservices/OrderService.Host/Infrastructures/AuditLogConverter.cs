using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace OrderService.Host
{
    public class AuditLogConverter : IAuditLogInfoToAuditLogConverter, ITransientDependency
    {
        protected IGuidGenerator GuidGenerator { get; }
        protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected AbpExceptionHandlingOptions ExceptionHandlingOptions { get; }

        public AuditLogConverter(IGuidGenerator guidGenerator, IExceptionToErrorInfoConverter exceptionToErrorInfoConverter, IJsonSerializer jsonSerializer, IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
        {
            GuidGenerator = guidGenerator;
            ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
            JsonSerializer = jsonSerializer;
            ExceptionHandlingOptions = exceptionHandlingOptions.Value;
        }

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

            //var remoteServiceErrorInfos = auditLogInfo.Exceptions?.Select(exception => ExceptionToErrorInfoConverter.Convert(exception, options =>
            //{
            //    options.SendExceptionsDetailsToClients = true;
            //    options.SendStackTraceToClients = true;
            //}))
            //                              ?? new List<RemoteServiceErrorInfo>();
            var  remoteServiceErrorInfos = new List<RemoteServiceErrorInfo>();
            string message = "";
            if (auditLogInfo.Exceptions != null)
            {
                var exception = auditLogInfo.Exceptions.FirstOrDefault();
                
                if(exception != null)
                {
                    message = exception.Message ?? "";
                    if (exception.InnerException != null)
                    {
                        message += "||" + exception.InnerException.Message;
                    }
                    if(exception.StackTrace != null)
                    {
                        message += "||" + exception.StackTrace;
                    }
                }
            }
          

             var exceptions = message != ""
                ? JsonSerializer.Serialize(message, indented: true)
                : null;

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
    }
}
