using Esale.Core.Caching.Redis;
using Esale.Core.Utility.Results;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using Esale.Core.Caching;
using MongoDB.Bson;
using StackExchange.Redis;
using System.Linq.Dynamic.Core;
using Volo.Abp.Features;
using OrderManagement.Domain.Shared;
using Volo.Abp.ObjectMapping;
using MongoDB.Driver;
using OrderManagement.Domain.OrderManagement;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Esale.Share.Authorize;

namespace OrderManagement.Application.OrderManagement;

public class CapacityControlAppService : ApplicationService, ICapacityControlAppService
{
    private readonly ISaleDetailService _saleDetailService;
    private readonly IAgencySaleDetailService _agencySaleDetailService;
    private readonly IEsaleGrpcClient _grpcClient;
    private readonly ICommonAppService _commonAppService;
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<PropertyCategory, ObjectId> _propertyDefinitionRepository;
    private readonly IRepository<ProductProperty, ObjectId> _productPropertyRepository;
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private IConfiguration _configuration { get; set; }
    public CapacityControlAppService(IConfiguration configuration, IEsaleGrpcClient grpcClient, IAgencySaleDetailService agencySaleDetailService, ISaleDetailService saleDetailService, ICommonAppService commonAppService, ICacheManager cacheManager, IRepository<PropertyCategory, ObjectId> propertyDefinitionRepository, IRepository<ProductProperty, ObjectId> productPropertyRepository, IRepository<ProductAndCategory, int> productAndCategoryRepository)
    {
        _configuration = configuration;
        _grpcClient = grpcClient;
        _agencySaleDetailService = agencySaleDetailService;
        _saleDetailService = saleDetailService;
        _commonAppService = commonAppService;
        _cacheManager = cacheManager;
        _propertyDefinitionRepository = propertyDefinitionRepository;
        _productPropertyRepository = productPropertyRepository;
        _productAndCategoryRepository = productAndCategoryRepository;
    }
    public async Task<IResult> SaleDetail()
    {
        var saledetails = _saleDetailService.GetActiveList();
        if (saledetails != null && saledetails.Count > 0)
        {
            foreach (var saledetail in saledetails)
            {
                string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, saledetail.UID.ToString());
                try
                {
                    await _cacheManager.SetStringAsync(_key,
                        CapacityControlConstants.CapacityControlPrefix,
                        saledetail.SaleTypeCapacity.ToString(),
                        new CacheOptions()
                        {
                            Provider = CacheProviderEnum.Redis,
                            RedisHash = false
                        });
                }
                catch (Exception ex)
                {
                    return new ErrorResult(ex.Message, "100");
                }
            }
        }
        return new SuccsessResult();
    }

    public async Task<IResult> Payment()
    {
        var saledetails = _saleDetailService.GetActiveList();
        if (saledetails != null && saledetails.Count > 0)
        {
            foreach (var saledetail in saledetails)
            {
                string _key = string.Format(CapacityControlConstants.PaymentCountPrefix, saledetail.UID.ToString());
                long _value = 0;
                var paymentDtos = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
                {
                    RelationId = saledetail.Id
                });
                if (paymentDtos != null && paymentDtos.Any(x => x.Status == 2))
                {
                    _value = paymentDtos.FirstOrDefault(x => x.Status == 2).Count;
                }
                await _cacheManager.SetStringAsync(_key,
                    CapacityControlConstants.CapacityControlPrefix,
                     _value.ToString(),
                    new CacheOptions()
                    {
                        Provider = CacheProviderEnum.Redis,
                        RedisHash = false
                    });
            }
        }
        return new SuccsessResult();
    }

    public async Task GrpcPaymentTest()
    {
        var propertydto = new PropertyCategoryDto()
        {
            Title = "مشخصات اصلی",
            Display = false,
            Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "امتیاز خودرو",
                    Key = "score", 
                    Type = PropertyTypeEnum.Number,
                    Value = "0",
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "استاندارد 85 گانه",
                    Key = "standard85",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "false"
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مورد علاقه",
                    Key = "isfavorite",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "false"
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "کلاس خودرو",
                    Key = "carclass",
                    Type = PropertyTypeEnum.Coding,
                    CodingType = CodingTypeEnum.CarClass,
                    Value = "0",
                },
            }
        };
        await _propertyDefinitionRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategory>(propertydto));
        propertydto = new PropertyCategoryDto()
        {
            Title = "مشخصات فنی",
            Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سرعت",
                    Key = "speed",
                    Type = PropertyTypeEnum.Text,
                    Value = "120 کیلومتر برساعت"
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "موتور",
                    Key = "engine",
                    Type = PropertyTypeEnum.DropDown,
                    Value = "موتور بی کیفیت",
                    DropDownItems = new List<DropDownItemDto>()
                    {
                        new DropDownItemDto()
                        {
                            Title = "موتور 1",
                            Value = 1
                        },
                        new DropDownItemDto()
                        {
                            Title = "موتور 2",
                            Value = 2
                        }
                    }
                }
            }
        };
        await _propertyDefinitionRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategory>(propertydto));
        propertydto = new PropertyCategoryDto()
        {
            Title = "مشخصات ظاهری",
            Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "رنگ بدنه",
                    Key = "bodycolor",
                    Type = PropertyTypeEnum.Text,
                    Value = "سفید",
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "جنس لاستیک",
                    Key = "cartire",
                    Type = PropertyTypeEnum.Text,
                    Value = "بارز",
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سپر عقب",
                    Key = "rearbumper",
                    Type = PropertyTypeEnum.Text,
                    Value = "رنگ شده",
                },
            }
        };
        await _propertyDefinitionRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategory>(propertydto));
        var propertyQuery = await _propertyDefinitionRepository.GetQueryableAsync();
        var property = propertyQuery.ToList();
        var products = (await _productAndCategoryRepository.GetQueryableAsync()).Where(x => x.Type == ProductAndCategoryType.Product).ToList();
        products.ForEach(async x =>
        {
            var productpropertydto = new ProductPropertyDto()
            {
                ProductId = x.Id,
                //PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(property)
            };
            await _productPropertyRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductProperty>(productpropertydto));
        });





        IMongoCollection<ProductProperty> productFeatureCollection = await _productPropertyRepository.GetCollectionAsync();
        // IAggregateFluent<ProductProperty> productFeatureAggregate = await _productPropertyRepository.GetAggregateAsync();
        var x = productFeatureCollection.Aggregate().Unwind(x => x.PropertyCategories).Unwind(x => x["PropertyCategories.Properties"]).Match(x => x["PropertyCategories.Properties."] == 1).ToList();
        //var bsonObject = x.ToBsonDocument();
        var kvp = BsonSerializer.Deserialize<List<Dictionary<string,object>>>(x.ToJson());
        var aa = kvp.Select(x => x["ProductId"]).ToList();

//        var filter = new BsonDocument( );

//        var update = Builders<ProductProperty>.Update.Set("PropertyCategories.$[].Properties.$[j].Type", 100);

//        var arrayFilters = new List<ArrayFilterDefinition>
//{
//    new BsonDocumentArrayFilterDefinition<ProductProperty>(new BsonDocument("j.Type", 1))
//};
//        var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
//        var xx =  productFeatureCollection.UpdateMany(filter,
//         update, updateOptions);

        ////FilterDefinition<ProductFeature> matchFilter = Builders<ProductFeature>.Filter.ElemMatch<ProductFeature>("ProductFeature", Builders<ProductFeature>.Filter.Eq("FeatureValues.ProductId", 1));
        //var filter = Builders<PropertyDefinition>.Filter.Where(e => e.FeatureValues.Any(x=> x.ProductId == 2));
        //var a = productFeatureAggregate.Match(filter).ToList();
        //var propertyQuery = await _propertyDefinitionRepository.GetQueryableAsync();
        //var property = propertyQuery.ToList();
        //property.ForEach(x =>
        //{
        //    x.Priority = 1;
        //    int i = 0;
        //    int v = 100;
        //    x.Properties.ForEach(p =>
        //    {
        //        i += 1;
        //        v += 1;
        //        p.Priority = i;
        //        p.Value = v.ToString();
        //    });
        //});
        //var productpropertydto = new ProductPropertyDto()
        //{
        //    ProductId = 42,
        //    PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(property)
        //};
        //await _productPropertyRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductProperty>(productpropertydto));
        //property = propertyQuery.ToList().ToList();
        //property.ForEach(x =>
        //{
        //    x.Priority = 1;
        //    int i = 0;
        //    int v = 100;
        //    x.Properties.ForEach(p =>
        //    {
        //        i += 1;
        //        v += 1;
        //        p.Priority = i;
        //        p.Value = v.ToString();
        //    });
        //});
        //productpropertydto = new ProductPropertyDto()
        //{
        //    ProductId = 43,
        //    PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(property)
        //};
        //await _productPropertyRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductProperty>(productpropertydto));
        //property = propertyQuery.ToList().ToList().Take(1).ToList();
        //property.ForEach(x =>
        //{
        //    x.Priority = 1;
        //    int i = 0;
        //    int v = 100;
        //    x.Properties.ForEach(p =>
        //    {
        //        i += 1;
        //        v += 1;
        //        p.Priority = i;
        //        p.Value = v.ToString();
        //    });
        //});
        //productpropertydto = new ProductPropertyDto()
        //{
        //    ProductId = 44,
        //    PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(property)
        //};
        //await _productPropertyRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductProperty>(productpropertydto));

        var productQuery = await _productPropertyRepository.GetQueryableAsync();
        var getproduct = productQuery.ToList();
        //var products = ObjectMapper.Map<List<ProductProperty>, List<ProductPropertyDto>>(getproduct.ToList());
        //var redis = await _redisCacheManager.ScanKeysAsync("n:CapacityControl:*");
        //var redis = _redisCacheManager.RemoveAllAsync("n:CapacityControl:*");
        //var payment = await _grpcClient.RetryForVerify();
        //var _result = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
        //{
        //    RelationId = 60
        //});
        //var handshake = await _grpcClient.HandShake(new PaymentHandShakeDto()
        //{
        //    Amount = 10000,
        //    PspAccountId = 4,
        //    CallBackUrl = "http",
        //    Mobile = "09140352778",
        //    NationalCode = "1092271600",
        //    AdditionalData = "asda"
        //});
        //Validation(165, 1029);
    }

    [SecuredOperation(CapacityControlServicePermissionConstants.Validation)]
    public async Task<bool> ValidationBySaleDetailUId(Guid saleDetailUId)
    {
        await _commonAppService.ValidateOrderStep(OrderStepEnum.SubmitOrder);
        long _capacity = 0;
        string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, saleDetailUId.ToString());
        long.TryParse(await _cacheManager.GetStringAsync(_key, CapacityControlConstants.CapacityControlPrefix, new CacheOptions() { Provider = CacheProviderEnum.Redis, RedisHash = false }), out _capacity);

        _key = string.Format(CapacityControlConstants.PaymentCountPrefix, saleDetailUId.ToString());
        long _request = await _cacheManager.StringIncrementAsync(CapacityControlConstants.CapacityControlPrefix + _key);

        if (_request > _capacity && _capacity > 0)
        {
            throw new UserFriendlyException(OrderConstant.NoCapacityCreateTicket, code: OrderConstant.NoCapacityCreateTicketId);
        }
        await _commonAppService.SetOrderStep(OrderStepEnum.SubmitOrder);
        return true;
    }

    public async Task<IResult> AgencyValidation(int saledetailid, int? agencyId, List<PaymentStatusModel> paymentDtos)
    {
        var saledetail = _saleDetailService.GetById(saledetailid);
        var agencySaledetail = await _agencySaleDetailService.GetBySaleDetailId(saledetail.Id, agencyId ?? 0);
        if (agencySaledetail == null)
        {
            return new ErrorResult("خطا در بازیابی نمایندگی ها", OrderConstant.NoCapacityCreateTicketId);
        }
        long _agancyCapacity = agencySaledetail.DistributionCapacity;
        int _agencyReserveCount = agencySaledetail.ReserveCount;
        long _agancyPaymentCount = 0;
        var paymentDtosForAgency = paymentDtos.Where(x => x.F2 == agencyId).ToList();
        if (paymentDtosForAgency != null && paymentDtosForAgency.Any(x => x.Status == 2))
        {
            _agancyPaymentCount = paymentDtosForAgency.Where(x => x.Status == 2).Sum(x => x.Count);
        }
        if (_agancyPaymentCount >= _agancyCapacity && _agancyCapacity > 0) //control zarfiat kili
        {
            return new ErrorResult(OrderConstant.AgancyNoCapacityCreateTicket, OrderConstant.AgancyNoCapacityCreateTicketId);
        }
        if (agencySaledetail.ReserveCount > 0)
        {

            if (_agancyPaymentCount < _agencyReserveCount)//agar be reserve nareside
            {
                return new SuccsessResult();
            }
            else
            {
                var _allAgenecyForSaleDetail = await _agencySaleDetailService.GetAgeneciesBySaleDetail(saledetail.Id);
                int _sumReseverCount = _allAgenecyForSaleDetail.Sum(x => x.ReserveCount);//sum reserve
                int FreeSpace = saledetail.SaleTypeCapacity - _sumReseverCount;
                var lsSum = from ag in _allAgenecyForSaleDetail
                            join pr in paymentDtos
                            on ag.AgencyId equals pr.F2
                            where ag.ReserveCount < pr.Count
                            && pr.Status == 2
                            select new
                            {
                                cnt = pr.Count - ag.ReserveCount
                            };
                long _sumBuyMoreThanReserve = lsSum.Sum(x => x.cnt);
                if (_sumBuyMoreThanReserve > FreeSpace) //agar zafiat azad(dovom) tamom shode
                {
                    return new ErrorResult(OrderConstant.AgancyNoCapacityCreateTicket, OrderConstant.AgancyNoCapacityCreateTicketId);
                }
            }
        }
        return new SuccsessResult();
    }
    public async Task<IDataResult<List<PaymentStatusModel>>> Validation(int saleDetaild, int? agencyId)
    {
        var saledetail = _saleDetailService.GetById(saleDetaild);
        if (saledetail == null)
        {
            return new ErrorDataResult<List<PaymentStatusModel>>("خطا در بازیابی برنامه های فروش", OrderConstant.NoCapacityCreateTicketId);
        }
        long _saledetailCapacity = saledetail.SaleTypeCapacity;
        long _saleDetailPaymentCount = 0;
        var paymentDtos = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
        {
            RelationId = saleDetaild,
            IsRelationIdGroup = true,
            IsRelationIdBGroup = true,
        });
        if (paymentDtos != null && paymentDtos.Any(x => x.Status == 2))
        {
            _saleDetailPaymentCount = paymentDtos.Where(x => x.Status == 2).Sum(x => x.Count);
        }
        if (_saleDetailPaymentCount >= _saledetailCapacity && _saledetailCapacity > 0) //control zarfiat koli
        {
            return new ErrorDataResult<List<PaymentStatusModel>>(OrderConstant.NoCapacityCreateTicket, OrderConstant.NoCapacityCreateTicketId);
        }
        if (agencyId != null && agencyId != 0)
        {
            var agency = await AgencyValidation(saledetail.Id, agencyId, paymentDtos);
            if (!agency.Success)
                return new ErrorDataResult<List<PaymentStatusModel>>(agency.Message, agency.MessageId);
        }
        return new SuccsessDataResult<List<PaymentStatusModel>>(paymentDtos);
    }
}
