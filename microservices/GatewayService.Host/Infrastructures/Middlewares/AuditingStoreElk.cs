﻿using Azure.Core;
using Elasticsearch.Net;
using GatewayService.Host;
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

namespace OrderService.Host.Infrastructures.Middlewares
{
    public class AuditingStoreElk : IAuditingStore, ITransientDependency
    {
        public class Body
        {
            public string _index { get; set; }
            public string _id { get; set; }
            public int _version { get; set; }
            public string result { get; set; }
            public Shards _shards { get; set; }
            public int _seq_no { get; set; }
            public int _primary_term { get; set; }
        }

        public class Shards
        {
            public int total { get; set; }
            public int successful { get; set; }
            public int failed { get; set; }
        }
        private readonly IRepository<AuditLog, Guid> _auditLogRepository;
        protected AuditLogConverter Converter { get; }
        private IConfiguration _configuration { get; set; }

        private IElkRepository<AuditLog, AuditLog> _elkRepository { get; set; }
        ElasticClient client;
        /// <summary>
        /// Creates  a new <see cref="AuditingStore"/>.
        /// </summary>
        public AuditingStoreElk(IRepository<AuditLog, Guid> auditLogRepository, IElkRepository<AuditLog, AuditLog> ElkRepository, ElasticClient client, IConfiguration Configuration, AuditLogConverter converter)
        {
            _auditLogRepository = auditLogRepository;
            _elkRepository = ElkRepository;
            this.client = client;
            _configuration = Configuration;
            Converter = converter;
        }

        public virtual async Task SaveAsync(AuditLogInfo auditInfo)
        {
            try
            {
                if (_configuration.GetSection("IsElkEnabled").Value == "1")
                {
                    string jsonData = JsonConvert.SerializeObject(await Converter.ConvertAsync(auditInfo));
                    var response = client.LowLevel.Index<StringResponse>(_configuration.GetSection("ElkIndexName").Value, jsonData);
                    Body body = JsonConvert.DeserializeObject<Body>(response.Body);
                    if (body._shards.successful != 1)
                    {
                        await _auditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
                    }
                }
                else
                {
                    await _auditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));

                }

                //client.IndexDocument(x);

            }
            catch (Exception ex)
            {
                await _auditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
            }


            // 
        }

        //public virtual void Save(AuditInfo auditInfo)
        //{
        //    try
        //    {
        //        if (_configuration.GetSection("IsElkEnabled").Value == "1")
        //        {
        //            string jsonData = JsonConvert.SerializeObject(AuditLog.CreateFromAuditInfo(auditInfo));
        //            var response = client.LowLevel.Index<StringResponse>(_configuration.GetSection("ElkIndexName").Value, jsonData);
        //            Body body = JsonConvert.DeserializeObject<Body>(response.Body);
        //            if (body._shards.successful != 1)
        //            {
        //                _auditLogRepository.Insert(AuditLog.CreateFromAuditInfo(auditInfo));
        //            }
        //        }
        //        else
        //        {
        //            _auditLogRepository.Insert(AuditLog.CreateFromAuditInfo(auditInfo));

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _auditLogRepository.Insert(AuditLog.CreateFromAuditInfo(auditInfo));

        //    }

        //}
    }
}


