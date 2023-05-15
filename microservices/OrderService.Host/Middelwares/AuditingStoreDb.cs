using Azure.Core;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using MsDemo.Shared.ExtensionsInterfaces;
using Nest;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Esale.Web.Host.Middelwares
{
    public class AuditingStoreDb : IAuditingStore, ITransientDependency
    {

        private readonly IRepository<AuditLog, Guid> _auditLogRepository;
        protected IAuditLogInfoToAuditLogConverter Converter { get; }

        private IElkRepository<AuditLog, AuditLog> _elkRepository { get; set; }
        ElasticClient client;
        /// <summary>
        /// Creates  a new <see cref="AuditingStore"/>.
        /// </summary>
        public AuditingStoreDb(IRepository<AuditLog, Guid> auditLogRepository, IAuditLogInfoToAuditLogConverter converter)
        {

            _auditLogRepository = auditLogRepository;
            Converter = converter;
        }

        public virtual async Task SaveAsync(AuditLogInfo auditInfo)
        {

            //await _auditLogRepository.InsertAsync(auditInfo);
            await _auditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
        }

        //public virtual void Save(AuditLogInfo auditInfo)
        //{


        //}

        //protected virtual async Task SaveLogAsync(AuditLogInfo auditInfo)
        //{
        //    using (var uow = UnitOfWorkManager.Begin(IsTransactional:true))
        //    {
        //        await _auditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
        //        await uow.CompleteAsync();
        //    }
        //}


    }
}


