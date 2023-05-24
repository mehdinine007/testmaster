using Esale.Core.Utility.Results;
using Google.Protobuf;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement
{
    public class CapacityControlAppService : ApplicationService, ICapacityControlAppService
    {
        private readonly IRepository<SaleDetail, int> _saleDetailRepository;
        private readonly IDistributedCache _distributedCache;
        private IConfiguration _configuration { get; set; }

        public CapacityControlAppService(IRepository<SaleDetail, int> saleDetailRepository, IConfiguration configuration, IDistributedCache distributedCache)
        {
            _saleDetailRepository = saleDetailRepository;
            _configuration = configuration;
            _distributedCache = distributedCache;
        }
        public async Task<IResult> SaleDetails()
        {
            var _getData = new List<Dictionary<string, object>>();
            if (_getData != null)
            {
                var data = _getData
                    .Cast<IDictionary<string, object>>()
                    .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).ToList();
                foreach (var row in data)
                {
                    string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, row["UId"].ToString());
                    decimal _count = decimal.Parse(row["Count"].ToString());
                    try
                    {
                        await _distributedCache.SetStringAsync(string.Format(CapacityControlConstants.CapacityControlPrefix,_key), decimal.ToInt64(_count).ToString());
                    }
                    catch (Exception ex)
                    {
                        return new ErrorResult(ex.Message,"100");
                    }
                }
            }
            return new SuccsessResult();
        }

        public async Task<IResult> Payment()
        {
            var _getSaleData = new List<Dictionary<string, object>>();
            if (_getSaleData != null)
            {
                var saleData = _getSaleData
                    .Cast<IDictionary<string, object>>()
                    .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).ToList();
                foreach (var row in saleData)
                {
                    string _key = string.Format(CapacityControlConstants.PaymentCountPrefix, row["UId"].ToString());
                    var _getPaymentData = new List<Dictionary<string, object>>();
                    if (_getPaymentData != null)
                    {
                        var paymentData = _getPaymentData
                            .Cast<IDictionary<string, object>>()
                            .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).FirstOrDefault();
                        int _count = 0;
                        if (paymentData != null && paymentData.Count > 0)
                            _count = int.Parse(paymentData["Count"].ToString());
                        int _value = _count;
                        await _distributedCache.SetStringAsync(string.Format(CapacityControlConstants.PaymentCountPrefix, _key), _value.ToString());
                    }
                }
            }
            return new SuccsessResult();
        }

    }
}
