﻿using Elasticsearch.Net;
using MsDemo.Shared.ExtensionsImplementions;
using MsDemo.Shared.ExtensionsInterfaces;
using Nest;
using PaymentService.Host.Infrastructures.Middlewares;
using Volo.Abp.Auditing;

namespace PaymentService.Host.Infrastructures.Extensions
{
    public static class ElkExtension
    {
        public static IServiceCollection ElkNest(this IServiceCollection services, IConfiguration configuration, string IndexName)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration["ElasticSearch:Url"]));
            var settings = new ConnectionSettings(pool).
                DisableDirectStreaming().
                // DefaultIndex($"{IndexName}-{DateTime.UtcNow:yyyy-MM}")
                DefaultIndex(IndexName)
                .ThrowExceptions(alwaysThrow: true)
                .PrettyJson();
            //AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton(client);
            services.AddScoped(typeof(IElkRepository<,>), typeof(ElkRepository<,>));
            services.AddScoped<IAuditingStore, AuditingStoreElk>();
            CreateIndex(client, IndexName);
            return services;
        }
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            if (!client.Indices.Exists(indexName).Exists)
            {
                var createIndexResponse = client.Indices.Create(indexName);
            }
        }
    }
}
