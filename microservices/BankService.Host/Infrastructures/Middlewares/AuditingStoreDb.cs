using Azure.Core;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using MsDemo.Shared.ExtensionsInterfaces;
using Nest;
using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BankService.Host.Infrastructures.Middlewares
{
    public class AuditingStoreDb : IAuditingStore, ITransientDependency
    {

        private readonly IRepository<AuditLog, Guid> _auditLogRepository;
        protected IAuditLogInfoToAuditLogConverter Converter { get; }
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private IElkRepository<AuditLog, AuditLog> _elkRepository { get; set; }
        ElasticClient client;
        /// <summary>
        /// Creates  a new <see cref="AuditingStore"/>.
        /// </summary>
        public AuditingStoreDb(IRepository<AuditLog, Guid> auditLogRepository, AuditLogConverter converter, IUnitOfWorkManager unitOfWorkManager)
        {

            _auditLogRepository = auditLogRepository;
            Converter = converter;
            _unitOfWorkManager = unitOfWorkManager;

        }
       
        public virtual async Task SaveAsync(AuditLogInfo auditInfo)
        {
            try
            {

                await _auditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo), autoSave: true) ;

                //  await _unitOfWorkManager.Current.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
         


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


