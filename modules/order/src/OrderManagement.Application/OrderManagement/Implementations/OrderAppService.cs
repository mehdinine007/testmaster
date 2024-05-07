using System.Text.RegularExpressions;
using OrderManagement.Domain.Bases;
using OrderManagement.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using OrderManagement.Application.Contracts.Services;
using System.Collections.Generic;
using System;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Results;
using IFG.Core.DataAccess;
using Volo.Abp.ObjectMapping;
using IFG.Core.Caching;
using OrderManagement.Application.Contracts.OrderManagement.Models;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using Esale.Share.Authorize;
using IFG.Core.Utility.Security;
using Core.Utility.Tools;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using Volo.Abp.Data;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using IFG.Core.Utility.Tools;
using Permission.Order;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrderAppService : ApplicationService, IOrderAppService
{
    private readonly ICommonAppService _commonAppService;
    private readonly IBaseInformationService _baseInformationAppService;
    private readonly IRepository<SaleDetail, int> _saleDetailRepository;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private readonly IRepository<AdvocacyUser, int> _advocacyUsers;
    private readonly IRepository<CustomerOrder, int> _commitOrderRepository;
    private readonly IRepository<OrderStatusTypeReadOnly, int> _orderStatusTypeReadOnlyRepository;
    private readonly IRepository<OrderRejectionTypeReadOnly, int> _orderRejectionTypeReadOnlyRepository;
    private readonly IEsaleGrpcClient _esaleGrpcClient;
    private IConfiguration _configuration { get; set; }
    private readonly IIpgServiceProvider _ipgServiceProvider;
    private readonly ICapacityControlAppService _capacityControlAppService;
    private readonly IRandomGenerator _randomGenerator;
    private readonly IRepository<CarTip_Gallery_Mapping> _carTipGalleryMappingRepository;
    private readonly IAuditingManager _auditingManager;
    private readonly IObjectMapper _objectMapper;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ICacheManager _cacheManager;
    private readonly IAttachmentService _attachmentService;
    private readonly IProductAndCategoryService _productAndCategoryService;
    private readonly IRepository<Priority, int> _priorityRepository;
    private readonly IOrganizationService _organizationService;
    private readonly IRepository<CarMakerBlackList, long> _blackListRepository;
    private readonly IRepository<CustomerPriority> _customerPriorityRepository;
    private readonly IUserDataAccessService _userDataAccessService;
    private readonly ICompanyGrpcClient _companyGrpcClient;
    private readonly IRepository<City, int> _cityRepository;
    public OrderAppService(ICommonAppService commonAppService,
                           IBaseInformationService baseInformationAppService,
                           IRepository<SaleDetail, int> saleDetailRepository,
                           IRepository<UserRejectionAdvocacy, int> userRejectionAdcocacyRepository,
                           IRepository<AdvocacyUser, int> advocacyUsers,
                           IRepository<CustomerOrder, int> commitOrderRepository,
                           IIpgServiceProvider ipgServiceProvider,
                           IRepository<OrderStatusTypeReadOnly, int> orderStatusTypeReadOnlyRepository,
                           IRepository<OrderRejectionTypeReadOnly, int> orderRejectionTypeReadOnlyRepository,
                           IEsaleGrpcClient esaleGrpcClient,
                           IConfiguration configuration,
                           ICapacityControlAppService capacityControlAppService,
                           IRandomGenerator randomGenerator,
                           IRepository<Gallery, int> galleryRepository,
                           IRepository<CarTip_Gallery_Mapping, int> carTipGalleryRepsoitory,
                           IRepository<CarTip_Gallery_Mapping> carTipGalleryMappingRepository,
                           IAuditingManager auditingManager,
                           IObjectMapper objectMapper,
                           IUnitOfWorkManager unitOfWorkManager,
                           ICacheManager cacheManager,
                           IAttachmentService attachmentService,
                           IProductAndCategoryService productAndCategoryService,
                           IRepository<Priority, int> priorityRepository,
                           IOrganizationService organizationService,
                           IRepository<CarMakerBlackList, long> blackListRepository,
                           IRepository<CustomerPriority> customerPriorityRepository,
                           ICompanyGrpcClient companyGrpcClient,
                           IUserDataAccessService userDataAccessService,
                           IRepository<City, int> cityRepository
                           )
    {
        _commonAppService = commonAppService;
        _baseInformationAppService = baseInformationAppService;
        _saleDetailRepository = saleDetailRepository;
        _userRejectionAdcocacyRepository = userRejectionAdcocacyRepository;
        _advocacyUsers = advocacyUsers;
        _commitOrderRepository = commitOrderRepository;
        _ipgServiceProvider = ipgServiceProvider;
        _orderStatusTypeReadOnlyRepository = orderStatusTypeReadOnlyRepository;
        _orderRejectionTypeReadOnlyRepository = orderRejectionTypeReadOnlyRepository;
        _esaleGrpcClient = esaleGrpcClient;
        _configuration = configuration;
        _capacityControlAppService = capacityControlAppService;
        _randomGenerator = randomGenerator;
        _carTipGalleryMappingRepository = carTipGalleryMappingRepository;
        _auditingManager = auditingManager;
        _objectMapper = objectMapper;
        _unitOfWorkManager = unitOfWorkManager;
        _cacheManager = cacheManager;
        _attachmentService = attachmentService;
        _productAndCategoryService = productAndCategoryService;
        _priorityRepository = priorityRepository;
        _organizationService = organizationService;
        _blackListRepository = blackListRepository;
        _customerPriorityRepository = customerPriorityRepository;
        _userDataAccessService = userDataAccessService;
        _companyGrpcClient = companyGrpcClient;
        _cityRepository = cityRepository;
    }



    private async Task<AdvocacyUserFromBankDto> CheckAdvocacy(string NationalCode, int esaleTypeId)
    {
        var advocacyUser = await _esaleGrpcClient.GetUserAdvocacyByNationalCode(NationalCode);

        if (advocacyUser == null)
            throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
        return new AdvocacyUserFromBankDto
        {
            ShebaNumber = advocacyUser.ShebaNumber,
            BankId = advocacyUser.BankId,
            AccountNumber = advocacyUser.AccountNumber
        };
    }
    public async Task<bool> Test()
    {

        var orderrep = await _advocacyUsers.GetQueryableAsync();
        AdvocacyUser users = new AdvocacyUser();
        Random rnd = new Random();
        int month = rnd.Next(1, 1000000);  // creates a number between 1 and 12
        string nc = month.ToString();
        users.nationalcode = month.ToString();
        users.price = 100;
        users.bankName = "vbn";
        users.BanksId = 3;
        users.dateTime = DateTime.Now;
        users.shabaNumber = "123";
        await _advocacyUsers.InsertAsync(users);
        await CurrentUnitOfWork.SaveChangesAsync();

        users = orderrep.FirstOrDefault(x => x.nationalcode == nc);
        await _advocacyUsers.DeleteAsync(users);
        await CurrentUnitOfWork.SaveChangesAsync();
        return true;



    }

    private async Task RustySalePlanValidation(CommitOrderDto commitOrder, ESaleTypeEnums esaleTypeId, string nationalCode)
    {
        if (esaleTypeId == ESaleTypeEnums.WornOutSale)
        {
            const string pattern = ".[A-Z a-z 0-9]";
            if (string.IsNullOrWhiteSpace(commitOrder.Vin) || !Regex.IsMatch(commitOrder.Vin, pattern, RegexOptions.Compiled))
                throw new UserFriendlyException("فرمت شماره VIN صحیح نیست");
            if (string.IsNullOrWhiteSpace(commitOrder.EngineNo) || commitOrder.EngineNo.Length > 20)
                throw new UserFriendlyException("فرمت شماره موتور صحیح نیست");
            if (string.IsNullOrWhiteSpace(commitOrder.ChassiNo) || commitOrder.ChassiNo.Length > 20)
                throw new UserFriendlyException("فرمت شماره شاسی صحیح نیست");
            if (string.IsNullOrWhiteSpace(commitOrder.Vehicle))
                throw new UserFriendlyException("نام خودرو به درستی وارد نشده است");
            var oldCarAccess = await _userDataAccessService.CheckOldCar(nationalCode, commitOrder.EngineNo, commitOrder.Vin, commitOrder.ChassiNo);
            if (!oldCarAccess.Success)
                throw new UserFriendlyException(oldCarAccess.Message, oldCarAccess.MessageId);
            commitOrder.Vin = commitOrder.Vin.ToUpper();
            return;
        }
        commitOrder.EngineNo = "";
        commitOrder.ChassiNo = "";
        commitOrder.Vin = "";
        commitOrder.Vehicle = "";
    }

    [UnitOfWork(false)]
    public async Task<GetOrderByIdResponseDto> GetOrderById(int orderId)
    {
        var query = (await _commitOrderRepository.WithDetailsAsync(x => x.SaleDetail.Product)).AsNoTracking();
        var queryResult = query.Select(x => new
        {
            OrderId = x.Id,
            x.OrderStatus,
            x.SaleDetail.Product.OrganizationId,
            ProductCode = x.SaleDetail.Product.Code,
            ProductId = x.SaleDetail.Product.Id
        })
        .FirstOrDefault(x => x.OrderId == orderId);
        if (queryResult is null)
            return null;

        return new(queryResult.ProductId,
            queryResult.ProductCode,
            queryResult.OrganizationId,
            (int)queryResult.OrderStatus);
    }


    [Audited]
    [UnitOfWork(isTransactional: false)]
    [SecuredOperation(OrderAppServicePermissionConstants.CommitOrder)]
    public async Task<CommitOrderResultDto> CommitOrder(CommitOrderDto commitOrderDto)
    {



        await _commonAppService.ValidateOrderStep(OrderStepEnum.SaveOrder);
        var allowedStatusTypes = new List<int>() { (int)OrderStatusType.RecentlyAdded, (int)OrderStatusType.PaymentSucceeded };

        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        TimeSpan ttl = DateTime.Now.Subtract(DateTime.Now);

        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            string PriorityFromConfig = _configuration.GetSection("Priority").Value;
            if (string.IsNullOrEmpty(PriorityFromConfig))
            {
                throw new UserFriendlyException("خطا در بازیابی اولویت ها");

            }
            if (PriorityFromConfig.IndexOf(((int?)commitOrderDto.PriorityId).ToString()) == -1)
            {
                throw new UserFriendlyException("اولویت صحیح انتخاب نشده است.");
            }
        }
        else
        {

            if (commitOrderDto.PriorityId != null)
            {
                throw new UserFriendlyException("انتخاب ها صحیح نمی باشند.");
            }

        }
        var nationalCode = _commonAppService.GetNationalCode();
        SaleDetailOrderDto SaleDetailDto = null;
        SaleDetailDto = await _cacheManager.GetAsync<SaleDetailOrderDto>(
            commitOrderDto.SaleDetailUId.ToString(),
            RedisConstants.SaleDetailPrefix,
            new CacheOptions()
            {
                Provider = CacheProviderEnum.Hybrid
            });
        if (SaleDetailDto == null)
        {
            var saleDetailQuery = await _saleDetailRepository.GetQueryableAsync();
            var SaleDetailFromDb = saleDetailQuery
                .Select(x => new SaleDetailOrderDto
                {
                    //EsaleTypeId = x.ESaleTypeId,
                    Id = x.Id,
                    MinimumAmountOfProxyDeposit = x.MinimumAmountOfProxyDeposit,
                    SaleId = x.SaleId,
                    SalePlanEndDate = x.SalePlanEndDate,
                    SalePlanStartDate = x.SalePlanStartDate,
                    UID = x.UID,
                    ESaleTypeId = (ESaleTypeEnums)x.ESaleTypeId,
                    CarFee = x.CarFee,
                    ProductId = x.ProductId,
                    SaleProcess = x.SaleProcess
                })
                .FirstOrDefault(x => x.UID == commitOrderDto.SaleDetailUId);

            if (SaleDetailFromDb == null)
            {
                throw new UserFriendlyException("تاریخ برنامه فروش به پایان رسیده است.");
            }
            else
            {
                SaleDetailDto = SaleDetailFromDb;
                ttl = SaleDetailDto.SalePlanEndDate.Subtract(DateTime.Now);
                await _cacheManager.SetAsync(
                    commitOrderDto.SaleDetailUId.ToString(),
                    RedisConstants.SaleDetailPrefix,
                    SaleDetailDto, 240,
                    new CacheOptions()
                    {
                        Provider = CacheProviderEnum.Hybrid
                    });
            }
        }
        else
        {
            ttl = SaleDetailDto.SalePlanEndDate.Subtract(DateTime.Now);

        }

        #region Check ProductAccess
        bool hasProductAccess = _configuration.GetValue<bool?>("UserDataAccessConfig:HasProduct") ?? false;
        bool hasProductAccessExists = _configuration.GetValue<bool?>("UserDataAccessConfig:HasProductExists") ?? false;
        if (hasProductAccess || hasProductAccessExists)
        {
            var productAccess = await _userDataAccessService.CheckProductAccess(nationalCode, SaleDetailDto.ProductId, hasProductAccessExists);
            if (!productAccess.Success)
                throw new UserFriendlyException(productAccess.Message, productAccess.MessageId);
        }
        #endregion


        if (SaleDetailDto.SaleProcess == SaleProcessType.CashSale && commitOrderDto.AgencyId is null)
        {
            throw new UserFriendlyException(OrderConstant.AgencyNotFound, OrderConstant.AgencyId);
        }
        if (SaleDetailDto.SaleProcess == SaleProcessType.CashSale && commitOrderDto.PspAccountId is null)
        {
            throw new UserFriendlyException(OrderConstant.PspAccountNotFound, OrderConstant.PspAccountId);
        }
        UserDto customer = new UserDto();
        if (SaleDetailDto.ESaleTypeId == ESaleTypeEnums.YouthSale || SaleDetailDto.SaleProcess == SaleProcessType.CashSale)
        {
            customer = await _esaleGrpcClient.GetUserId(_commonAppService.GetUserId().ToString());
        }
        if (SaleDetailDto.ESaleTypeId == ESaleTypeEnums.YouthSale && customer.GenderCode != (int)GenderType.Female)
        {
            throw new UserFriendlyException("طرح فروش مربوط به شما نمی باشد");
        }
        if (SaleDetailDto.SaleProcess == SaleProcessType.CashSale)
        {
            if (!customer.NationalCode.Equals(nationalCode))
            {
                throw new UserFriendlyException("شما نمیتوانید سفارش شخص دیگری را پرداخت کنید");
            }
        }

        ////////////////conntrol repeated order in saledetails// iran&&varedat

        CheckSaleDetailValidation(SaleDetailDto);
        await RustySalePlanValidation(commitOrderDto, SaleDetailDto.ESaleTypeId, nationalCode);

        if (SaleDetailDto.SaleProcess == SaleProcessType.RegularSale)
            if (!await NationalCodeExistsInPriority(nationalCode))
                throw new UserFriendlyException("کد ملی متقاضی در لیست الویت بندی وجود نداشت");


        await _commonAppService.IsUserRejected(); //if user reject from advocacy


        var orderQuery = await _commitOrderRepository.GetQueryableAsync();
        var userId = _commonAppService.GetUserId();

        _baseInformationAppService.CheckBlackList(SaleDetailDto.ESaleTypeId); //if user reject from advocacy
        var CustomerOrderWinner = orderQuery
            .AsNoTracking()
            .Select(x => new { x.UserId, x.OrderStatus, x.SaleDetailId, x.DeliveryDateDescription, x.Id })
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(
                y => y.UserId == userId &&
             (y.OrderStatus == OrderStatusType.Winner)
         );
        if (CustomerOrderWinner != null)
        {
            throw new UserFriendlyException("جهت ثبت سفارش جدید لطفا ابتدا از جزئیات سفارش، سفارش قبلی خود که احراز شده اید یا در حال بررسی می باشد را لغو نمایید");

        }

        string EsaleTypeId = await _cacheManager.GetStringAsync("_EsaleType", RedisConstants.CommitOrderPrefix + userId.ToString()
           , new CacheOptions()
           {
               Provider = CacheProviderEnum.Redis
           });
        if (!string.IsNullOrEmpty(EsaleTypeId))
        {
            if (EsaleTypeId != SaleDetailDto.ESaleTypeId.ToString())
            {
                throw new UserFriendlyException("امکان انتخاب فقط یک نوع طرح فروش وجود دارد");
            }
        }
        else
        {

            var activeSuccessfulOrderExists = orderQuery
                .AsNoTracking()
                .Select(x => new { x.UserId, x.OrderStatus, x.SaleDetail.ESaleTypeId })
                .FirstOrDefault(
                    y => y.UserId == userId &&
                 y.OrderStatus == OrderStatusType.RecentlyAdded
             );

            if (activeSuccessfulOrderExists != null)
            {
                if (SaleDetailDto.ESaleTypeId != (ESaleTypeEnums)activeSuccessfulOrderExists.ESaleTypeId)
                {
                    await _cacheManager.SetWithPrefixKeyAsync("_EsaleType",
                           RedisConstants.CommitOrderPrefix + userId.ToString(),
                           SaleDetailDto.ESaleTypeId.ToString(),
                           ttl.TotalSeconds);
                    throw new UserFriendlyException("امکان انتخاب فقط یک نوع طرح فروش وجود دارد");
                }
            }

        }
        Console.WriteLine("aftercachecchek");



        ///////////////////////////////////iran/////////////
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            object objectCommitOrderIran = null;
            //_cacheManager.GetCache("CommitOrderIran").
            //TryGetValue(
            //    userId.ToString() + "_" +
            //    SaleDetailDto.SaleId.ToString()
            //    , out objectCommitOrderIran);
            objectCommitOrderIran = await _cacheManager.GetStringAsync(userId.ToString() + "_" + SaleDetailDto.SaleId.ToString(), "",
                new CacheOptions()
                {
                    Provider = CacheProviderEnum.Redis
                });
            if (objectCommitOrderIran != null)
            {
                throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید از درخواست قبلی خود انصراف دهید");
            }
            else
            {
                var allowedOrderStatuses = new List<int>() { (int)OrderStatusType.RecentlyAdded, (int)OrderStatusType.PaymentNotVerified };
                CustomerOrder customerOrderIranFromDb =
                _commitOrderRepository
                .ToListAsync()
                .Result
                .Select(x => new CustomerOrder
                {
                    UserId = x.UserId,
                    SaleId = x.SaleId,
                    OrderStatus = x.OrderStatus
                })
                .FirstOrDefault(x =>
                   x.UserId == userId &&
                   x.SaleId == SaleDetailDto.SaleId &&
                   //x.OrderStatus == OrderStatusType.RecentlyAdded
                   allowedOrderStatuses.Any(y => (int)x.OrderStatus == y)
                   );
                if (customerOrderIranFromDb != null)
                {
                    //await _cacheManager.GetCache("CommitOrderIran").
                    //   SetAsync(
                    //       userId.ToString() + "_" +
                    //       SaleDetailDto.SaleId.ToString()
                    //       , customerOrderIranFromDb.Id
                    //       , TimeSpan.FromSeconds(ttl.TotalSeconds));

                    await _cacheManager.SetStringAsync(userId.ToString() + "_" +
                           SaleDetailDto.SaleId.ToString(),
                           "",
                           customerOrderIranFromDb.Id.ToString(),
                           new CacheOptions()
                           {
                               Provider = CacheProviderEnum.Redis
                           }, ttl.TotalSeconds);
                    throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید از درخواست قبلی خود انصراف دهید");


                }
            }
        }
        ///////////////////////////////vardati chek dar bakhshnameh///////////////////////////
        else
        {
            object objectCommitOrderIran = null;


            objectCommitOrderIran = await _cacheManager.GetStringAsync(userId.ToString() + "_" +
                commitOrderDto.PriorityId.ToString() + "_" +
                SaleDetailDto.SaleId.ToString(),
                RedisConstants.CommitOrderPrefix,
                new CacheOptions()
                {
                    Provider = CacheProviderEnum.Redis
                });

            if (objectCommitOrderIran != null && !commitOrderDto.OrderId.HasValue)
            {
                throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید، درخواست قبلی خود را لغو نمایید یا اولویت دیگری را انتخاب نمایید");
            }
            else
            {
                var customerOrderIranFromDb =
                orderQuery
                .AsNoTracking()
                .Select(x => new CustomerOrderDto
                {
                    OrderStatus = (int)x.OrderStatus,
                    SaleId = x.SaleId,
                    PriorityId = x.PriorityId,
                    UserId = x.UserId,
                    Id = x.Id
                })
                .FirstOrDefault(y =>
                   y.UserId == userId
                   //y.OrderStatus == OrderStatusType.RecentlyAdded
                   && y.SaleId == SaleDetailDto.SaleId
                   && y.PriorityId == (PriorityEnum)commitOrderDto.PriorityId
                   && allowedStatusTypes.Any(d => y.OrderStatus == d));


                if (customerOrderIranFromDb != null && (!commitOrderDto.OrderId.HasValue || customerOrderIranFromDb.Id != commitOrderDto.OrderId.Value))
                {

                    await _cacheManager.SetWithPrefixKeyAsync("_" + commitOrderDto.PriorityId.ToString() + "_" + SaleDetailDto.SaleId.ToString(),
                           RedisConstants.CommitOrderPrefix + userId.ToString(),
                           customerOrderIranFromDb.Id.ToString(),
                           ttl.TotalSeconds);
                    throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید، درخواست قبلی خود را لغو نمایید یا اولویت دیگری را انتخاب نمایید");
                }

            }
            object objectCustomerOrderFromCache = null;


            objectCustomerOrderFromCache = await _cacheManager.GetStringAsync(
                    userId.ToString() + "_" +
                    SaleDetailDto.Id.ToString(),
                    RedisConstants.CommitOrderPrefix,
                    new CacheOptions()
                    {
                        Provider = CacheProviderEnum.Redis
                    });

            if (objectCustomerOrderFromCache != null


                    )
            {
                throw new UserFriendlyException("این خودرو را قبلا انتخاب نموده اید");
            }
            if (objectCustomerOrderFromCache == null)
            {
                var CustomerOrderFromDb = orderQuery
                     .AsNoTracking()
                    .Select(x => new CustomerOrder
                    {
                        SaleDetailId = x.SaleDetailId,
                        SaleId = x.SaleId,
                        UserId = x.UserId,
                        OrderStatus = x.OrderStatus
                    })
                    .FirstOrDefault(x =>
                x.UserId == userId
                && x.SaleDetailId == (int)SaleDetailDto.Id
                && allowedStatusTypes.Any(y => y == (int)x.OrderStatus));


                if (CustomerOrderFromDb != null


                        )
                {

                    await _cacheManager.SetWithPrefixKeyAsync("_" + SaleDetailDto.Id.ToString(),
                              RedisConstants.CommitOrderPrefix + userId.ToString(),
                              CustomerOrderFromDb.Id.ToString(),
                              ttl.TotalSeconds);
                    throw new UserFriendlyException("این خودرو را قبلا انتخاب نموده اید.");

                }
            }



        }




        CustomerOrder customerOrder = new CustomerOrder();

        if (SaleDetailDto.SaleProcess == SaleProcessType.CashSale)
        {
            // try
            // {
            IResult agencyCapacityControl = null;

            agencyCapacityControl = await _capacityControlAppService.Validation(SaleDetailDto.Id, commitOrderDto.AgencyId);
            if (!agencyCapacityControl.Success)
                throw new UserFriendlyException(agencyCapacityControl.Message);
        }

        {
            customerOrder.SaleDetailId = SaleDetailDto.Id;
            customerOrder.UserId = userId;
            customerOrder.PriorityId = (PriorityEnum)commitOrderDto.PriorityId;
            customerOrder.Vin = commitOrderDto.Vin;
            customerOrder.ChassiNo = commitOrderDto.ChassiNo;
            customerOrder.EngineNo = commitOrderDto.EngineNo;
            customerOrder.Vehicle = commitOrderDto.Vehicle;
            customerOrder.OrderStatus = OrderStatusType.RecentlyAdded;
            customerOrder.SaleId = SaleDetailDto.SaleId;
            customerOrder.AgencyId = commitOrderDto.AgencyId;
            customerOrder.PaymentSecret = _randomGenerator.GetUniqueInt();
            customerOrder.OrderDeliveryStatus = OrderDeliveryStatusType.OrderRegistered;
            customerOrder.TrackingCode = SaleDetailDto.SaleProcess == SaleProcessType.SaleWithTrackingCode ? Core.Utility.Tools.RandomGenerator.GetUniqueInt(_configuration.GetValue<int>("RandomeCodeLength")).ToString() : null;
            await _commitOrderRepository.InsertAsync(customerOrder);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        ApiResult<IpgApiResult> handShakeResponse = new ApiResult<IpgApiResult>();
        if (SaleDetailDto.SaleProcess == SaleProcessType.CashSale)
        {
            handShakeResponse = await _ipgServiceProvider.HandShakeWithPsp(new PspHandShakeRequest()
            {
                CallBackUrl = _configuration.GetValue<string>("CallBackUrl"),
                Amount = (long)SaleDetailDto.MinimumAmountOfProxyDeposit,
                Mobile = customer.MobileNumber,
                AdditionalData = customerOrder.PaymentSecret.HasValue ? customerOrder.PaymentSecret.Value.ToString() : string.Empty,
                NationalCode = nationalCode,
                PspAccountId = commitOrderDto.PspAccountId.Value,
                FilterParam1 = customerOrder.SaleDetailId,
                FilterParam2 = customerOrder.AgencyId.HasValue ? customerOrder.AgencyId.Value : default,
                FilterParam3 = customerOrder.Id
            });
            if (!handShakeResponse.Success)
            {
                customerOrder.OrderStatus = OrderStatusType.PaymentNotVerified;
                await _commitOrderRepository.UpdateAsync(customerOrder);
                await CurrentUnitOfWork.SaveChangesAsync();
                throw new UserFriendlyException(handShakeResponse.Message);
            }
            customerOrder.PaymentId = handShakeResponse.Result.PaymentId;
            await _commitOrderRepository.UpdateAsync(customerOrder, autoSave: true);
            await CurrentUnitOfWork.SaveChangesAsync();

        }

        //await _distributedCache.SetStringAsync(
        // userId.ToString() + "_" +
        //    commitOrderDto.SaleDetailUId.ToString()
        //    , customerOrder.Id.ToString(),
        // new DistributedCacheEntryOptions()
        // {
        //     AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
        // });

        //await _distributedCache.SetStringAsync(userId.ToString(), SaleDetailDto.ESaleTypeId.ToString()); //braye inke ye nafar chan noe forosh ro entekhab nakone
        //if iran
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            //await _cacheManager.GetCache("CommitOrderIran").
            //  SetAsync(
            //      userId.ToString() + "_" +
            //      SaleDetailDto.SaleId.ToString()
            //    , customerOrder.Id
            //    , TimeSpan.FromSeconds(ttl.TotalSeconds)
            //  );
            await _cacheManager.SetStringAsync(userId.ToString() + "_" +
                  SaleDetailDto.SaleId.ToString(),
                  "",
                  customerOrder.Id.ToString(),
                  new CacheOptions()
                  {
                      Provider = CacheProviderEnum.Redis
                  }, ttl.TotalSeconds);
        }//vardat

        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            await _cacheManager.SetWithPrefixKeyAsync("_" + commitOrderDto.PriorityId.ToString() + "_" + SaleDetailDto.SaleId.ToString(),
                            RedisConstants.CommitOrderPrefix + userId.ToString(),
                            customerOrder.Id.ToString(),
                            ttl.TotalSeconds);
            await _cacheManager.SetWithPrefixKeyAsync("_" + SaleDetailDto.Id.ToString(),
                            RedisConstants.CommitOrderPrefix + userId.ToString(),
                            customerOrder.Id.ToString(),
                            ttl.TotalSeconds);
            //await _distributedCache.SetStringAsync(RedisConstants.CommitOrderPrefix + userId.ToString() + "_" +
            //                commitOrderDto.PriorityId.ToString() + "_" +
            //                SaleDetailDto.SaleId.ToString()
            //               , customerOrder.Id.ToString(),
            //               new DistributedCacheEntryOptions()
            //               {
            //                   AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
            //               });

            //await _distributedCache.SetStringAsync(RedisConstants.CommitOrderPrefix + userId.ToString() + "_" +
            //         SaleDetailDto.Id.ToString()
            //         , customerOrder.Id.ToString(),
            //         new DistributedCacheEntryOptions
            //         {
            //             AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
            //         });
            await _cacheManager.SetWithPrefixKeyAsync("_EsaleType",
                           RedisConstants.CommitOrderPrefix + userId.ToString(),
                           SaleDetailDto.ESaleTypeId.ToString(),
                           ttl.TotalSeconds);



        }

        //var pspCacheKey = string.Format(RedisConstants.UserTransactionKey, nationalCode, customerOrder.Id);
        //var userTransactionToken = await _distributedCache.GetStringAsync(cacheKey);
        //if (userTransactionToken != null)
        //{
        //    await _distributedCache.RemoveAsync(cacheKey);
        //}
        //await _distributedCache.SetStringAsync(cacheKey, handShakeResponse.Token);
        //var  ObjectMapper.Map<HandShakeResponseDto, HandShakeResultDto>(handShakeResponse);
        await _commonAppService.SetOrderStep(OrderStepEnum.SaveOrder);
        var encryptionIsRequired = SaleDetailDto.SaleProcess == SaleProcessType.DirectSale;
        var encryptionKey = string.Empty;
        var organizationUrl = string.Empty;
        if (encryptionIsRequired)
        {
            var productId = SaleDetailDto.ProductId;
            var product = await _productAndCategoryService.GetById(productId, false);
            var cmp = await _productAndCategoryService.GetProductAndCategoryByCode(product.Code.Substring(0, 4));
            var organization = await _organizationService.GetById(cmp.Id);
            encryptionKey = organization.EncryptKey;
            var enc = string.Format(OrderConstant.OrganizationEncryptedExpression, nationalCode, customerOrder.SaleId);
            organizationUrl = string.Format(OrderConstant.OrganizationUrlFormat,
                organization.Url.RemoveLastCharacterIfExists('/'),
                enc.Aes256Encrypt(organization.EncryptKey));
        }
        return new CommitOrderResultDto()
        {
            OrganizationUrl = organizationUrl,
            PaymentGranted = SaleDetailDto.SaleProcess == SaleProcessType.CashSale,
            UId = commitOrderDto.SaleDetailUId,
            TrackingCode = customerOrder.TrackingCode,
            PaymentMethodConigurations = SaleDetailDto.SaleProcess == SaleProcessType.CashSale ? new()
            {
                Message = handShakeResponse?.Result?.Message,
                StatusCode = handShakeResponse?.Result?.StatusCode,
                Token = handShakeResponse?.Result?.Token,
                HtmlContent = handShakeResponse?.Result?.HtmlContent
            } : new()
        };
    }
    public bool CanOrderEdit(OrderStatusType orderStatusType, string deliveryDate, int saleId)
    {
        if (_configuration.GetSection("OrderEditEndTime").Value == null || DateTime.Parse(_configuration.GetSection("OrderEditEndTime").Value) < DateTime.Now)
        {
            return false;
        }
        if ((orderStatusType == OrderStatusType.Winner && !string.IsNullOrEmpty(deliveryDate)) || (orderStatusType == OrderStatusType.RecentlyAdded)) // OrderStatusType.RecentlyAdded
        {
            return true;
        }
        return false;
    }

    private void CheckSaleDetailValidation(SaleDetailOrderDto sale)
    {
        if (sale == null)
        {
            throw new UserFriendlyException("تاریخ برنامه فروش به پایان رسیده است.");
        }
        else
        {
            if (DateTime.Now > sale.SalePlanStartDate
              &&
              DateTime.Now <= sale.SalePlanEndDate
              )
            {
                return;
            }
            else
            {
                throw new UserFriendlyException("تاریخ برنامه فروش به پایان رسیده است.");
            }
        }
    }

    [UnitOfWork(false, IsolationLevel.ReadUncommitted)]
    [SecuredOperation(OrderAppServicePermissionConstants.GetCustomerOrderList)]
    public async Task<CustomerOrder_OrderDetailTreeDto> GetCustomerOrderList(List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {

        var userId = _commonAppService.GetUserId();
        var orderRejections = _orderRejectionTypeReadOnlyRepository.WithDetails().ToList();
        var orderStatusTypes = _orderStatusTypeReadOnlyRepository.WithDetails().ToList();
        var customerOrders = _commitOrderRepository.WithDetails()
            .AsNoTracking()
            .Join(await _saleDetailRepository.WithDetailsAsync(x => x.Product.Organization),
            x => x.SaleDetailId,
            y => y.Id,
            (x, y) => new
            {
                y.CarDeliverDate,
                y.SalePlanDescription,
                x.UserId,
                x.OrderStatus,
                x.CreationTime,
                OrderId = x.Id,
                PriorityId = x.PriorityId,
                x.DeliveryDate,
                x.DeliveryDateDescription,
                x.OrderRejectionStatus,
                y.ESaleTypeId,
                y.ProductId,
                y.Product,
                y.SalePlanEndDate,
                y.Id,
                y.SaleId,
                x.TrackingCode,
                x.TransactionCommitDate,
                x.PaymentPrice,
                x.TransactionId,
                x.SignTicketId,
                x.SignStatus

            }).Where(x => x.UserId == userId)
            .Select(x => new CustomerOrder_OrderDetailDto
            {
                CarDeliverDate = x.CarDeliverDate,
                CreationTime = x.CreationTime,
                OrderId = x.OrderId,
                SaleDescription = x.SalePlanDescription,
                UserId = x.UserId,
                OrderStatusCode = (int)x.OrderStatus,
                PriorityId = x.PriorityId.HasValue ? (int)x.PriorityId : null,
                DeliveryDateDescription = x.DeliveryDateDescription,
                DeliveryDate = x.DeliveryDate,
                OrderRejectionCode = x.OrderRejectionStatus.HasValue ? (int)x.OrderRejectionStatus : null,
                ESaleTypeId = (ESaleTypeEnums)x.ESaleTypeId,
                ProductId = x.ProductId,
                Product = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryViewModel>(x.Product),
                SalePlanEndDate = x.SalePlanEndDate,
                Id = x.Id,
                SaleId = x.SaleId,
              
                SignTicketId = x.SignTicketId,
                SignStatusId = x.SignStatus,
                SignStatusTitle = x.SignStatus != null ? EnumHelper.GetDisplayName(x.SignStatus) : null,
                TransactionCommitDate = x.TransactionCommitDate,
                PaymentPrice = x.PaymentPrice,
                TransactionId = x.TransactionId,
                TrackingCode = x.TrackingCode,
                CompanyName = x.Product.Organization.Title
            }).ToList();
        var cancleableDate = _configuration.GetValue<string>("CancelableDate");
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, customerOrders.Select(x => x.ProductId).ToList(), attachmentType, attachmentlocation);
        CustomerOrder_OrderDetailTreeDto resultObject = new();
        var enableExactPriorityCalculation = _configuration.GetValue<bool>("EnableExactPriorityCalculation");
        if (enableExactPriorityCalculation)
        {
            var anyCompletedOrder = customerOrders.FirstOrDefault(x => x.OrderStatusCode == (int)OrderStatusType.Winner
                && x.PriorityId.HasValue &&
                x.PriorityId.Value == 1);
            var customerPriority = (await _customerPriorityRepository.GetQueryableAsync())
                .Select(x => new
                {
                    x.Uid,
                    x.ChosenPriorityByCustomer,
                    x.ApproximatePriority
                })
                .AsNoTracking()
                .FirstOrDefault(x => x.Uid == userId);
            resultObject.PrimaryPriority = customerPriority?.ChosenPriorityByCustomer;
            if (anyCompletedOrder == null)
            {
                resultObject.ApproximatePriority = customerPriority?.ApproximatePriority;
            }
        }

        customerOrders.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.ProductId).ToList();
            x.Product.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);

            var orderStatusType = orderStatusTypes.FirstOrDefault(y => y.Code == x.OrderStatusCode);
            x.OrderstatusTitle = orderStatusType.Title;
            if (x.OrderRejectionCode.HasValue)
            {
                var orderRejection = orderRejections.FirstOrDefault(y => y.Code == x.OrderRejectionCode);
                x.OrderRejectionTitle = orderRejection.Title;
                if ((OrderRejectionType)x.OrderRejectionCode == OrderRejectionType.PhoneNumberAndNationalCodeConflict)
                    x.DeliveryDate = null;
            }
            if (CanOrderEdit((OrderStatusType)x.OrderStatusCode, x.DeliveryDateDescription, x.SaleId)) // OrderStatusType.RecentlyAdded
            {
                try
                {
                    // _baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhilteListEnseraf);
                    _baseInformationAppService.CheckBlackList(x.ESaleTypeId);
                    x.Cancelable = true;

                }
                catch (Exception ex)
                {
                    x.Cancelable = false;
                }
            }
            else
                x.Cancelable = false;

            //else if (x.OrderStatusCode == 40 && x.DeliveryDateDescription.Contains(cancleableDate, StringComparison.InvariantCultureIgnoreCase)) // OrderStatusType.Winner
            //    x.Cancelable = true;
        });
        resultObject.OrderList = customerOrders.OrderByDescending(x => x.OrderId).ToList();
        return resultObject;


    }
    [Audited]
    [UnitOfWork(isTransactional: false)]
    [SecuredOperation(OrderAppServicePermissionConstants.CancelOrder)]
    public async Task<CustomerOrderDto> CancelOrder(int orderId)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        var userId = _commonAppService.GetUserId();
        //if (!_commonAppService.IsInRole("Customer"))
        //{
        //    throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        //}
        //_baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhiteListOrder);
        var customerOrder = _commitOrderRepository.WithDetails().FirstOrDefault(x => x.Id == orderId);
        if (customerOrder == null)
            throw new UserFriendlyException("شماره سفارش صحیح نمی باشد");

        //if (!(customerOrder.OrderStatus == OrderStatusType.RecentlyAdded))
        //    throw new UserFriendlyException("امکان انصراف وجود ندارد");

        if (customerOrder.UserId != userId)
            throw new UserFriendlyException("شماره سفارش صحیح نمی باشد");

        SaleDetailOrderDto saleDetailOrderDto;
        saleDetailOrderDto = await _cacheManager.GetAsync<SaleDetailOrderDto>(
            customerOrder.SaleDetailId.ToString(),
            RedisConstants.SaleDetailPrefix,
            new CacheOptions()
            {
                Provider = CacheProviderEnum.Hybrid
            });
        if (saleDetailOrderDto == null)
        {
            var saleDetail = _saleDetailRepository.WithDetails().FirstOrDefault(x => x.Id == customerOrder.SaleDetailId)
                ?? throw new UserFriendlyException("جزئیات برنامه فروش یافت نشد");
            saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(saleDetail);
            await _cacheManager.SetAsync(customerOrder.SaleDetailId.ToString(),
                RedisConstants.SaleDetailPrefix,
                saleDetailOrderDto, 240,
                new CacheOptions()
                {
                    Provider = CacheProviderEnum.Hybrid
                });
        }
        if (!CanOrderEdit(customerOrder.OrderStatus, customerOrder.DeliveryDateDescription, customerOrder.SaleId)) // OrderStatusType.RecentlyAdded
        {
            throw new UserFriendlyException("امکان انصراف وجود ندارد");
        }
        _baseInformationAppService.CheckBlackList(saleDetailOrderDto.ESaleTypeId); //if user reject from advocacy


        //var saleDetailCahce = await _distributedCache.GetStringAsync(string.Format(RedisConstants.SaleDetailPrefix, customerOrder.SaleDetailId.ToString()));
        //if (!string.IsNullOrWhiteSpace(saleDetailCahce))
        //{

        //    //if (_cacheManager.GetCache("SaleDetail").TryGetValue(customerOrder.SaleDetailId.ToString(), out var saleDetailFromCache))
        //    //{
        //    saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(JsonConvert.DeserializeObject<SaleDetail>(saleDetailCahce));
        //    //}
        //}
        //else
        //{
        //    var saleDetail = _saleDetailRepository.WithDetails().FirstOrDefault(x => x.Id == customerOrder.SaleDetailId)
        //        ?? throw new UserFriendlyException("جزئیات برنامه فروش یافت نشد");
        //    saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(saleDetail);
        //    //await _cacheManager.GetCache("SaleDetail").SetAsync(saleDetailOrderDto.Id.ToString(), saleDetailOrderDto);
        //    await _distributedCache.SetStringAsync(string.Format(RedisConstants.SaleDetailPrefix, customerOrder.SaleDetailId.ToString()),
        //        JsonConvert.SerializeObject(saleDetailOrderDto),
        //        new DistributedCacheEntryOptions
        //        {
        //            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(saleDetail.SalePlanEndDate.Subtract(DateTime.Now).TotalSeconds))
        //        });

        //}
        // CheckSaleDetailValidation(saleDetailOrderDto);
        //var currentTime = DateTime.Now;
        //if (currentTime > saleDetailOrderDto.SalePlanEndDate)
        //    throw new UserFriendlyException("امکان انصراف برای سفارشاتی که برنامه فروش مرتبط ،منقضی شده باشد ممکن نیست");
        //if iran
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            //await _cacheManager.GetCache("CommitOrderIran").
            //  RemoveAsync(
            //      userId.ToString() + "_" +
            //      customerOrder.SaleId.ToString()
            //  );
            await _cacheManager.RemoveAsync(
                userId.ToString() + "_" + customerOrder.SaleId.ToString(),
                "",
                new CacheOptions()
                {
                    Provider = CacheProviderEnum.Redis
                });
        }//vardat
        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            //await _distributedCache.RemoveAsync(userId.ToString() + "_" +
            //                customerOrder.PriorityId.ToString() + "_" +
            //                customerOrder.SaleId.ToString());
            //await _distributedCache.RemoveAsync(
            //         userId.ToString() + "_" +
            //         customerOrder.SaleDetailId.ToString());
            //await _redisCacheManager.RemoveAllAsync(RedisConstants.CommitOrderPrefix + userId.ToString() + "_");
            await _cacheManager.RemoveWithPrefixKeyAsync(RedisConstants.CommitOrderPrefix + userId.ToString());
        }



        customerOrder.OrderStatus = OrderStatusType.Canceled;
        await CurrentUnitOfWork.SaveChangesAsync();
        customerOrder = _commitOrderRepository.WithDetails().
           FirstOrDefault(x => x.UserId == userId
           && x.OrderStatus == OrderStatusType.RecentlyAdded);
        if (customerOrder == null)
        {
            await _cacheManager.RemoveAsync(userId.ToString(),
                RedisConstants.CommitOrderEsaleTypePrefix,
                new CacheOptions()
                {
                    Provider = CacheProviderEnum.Redis
                });
        }
        return ObjectMapper.Map<CustomerOrder, CustomerOrderDto>(customerOrder, new CustomerOrderDto());
    }



    [Audited]
    [SecuredOperation(OrderAppServicePermissionConstants.InsertUserRejectionAdvocacyPlan)]
    public async Task InsertUserRejectionAdvocacyPlan(string userSmsCode)
    {

        //if (!_commonAppService.IsInRole("Customer"))
        //{
        //    throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        //}
        //var userId = _abpSession.UserId ?? throw new UserFriendlyException("لطفا لاگین کنید");
        //var user = _userRepository.FirstOrDefault(userId);
        var userNationalCode = _commonAppService.GetNationalCode();
        var userId = _commonAppService.GetUserId();
        var user = await _esaleGrpcClient.GetUserId(userId.ToString());
        var (userMobile, userShaba, userAccountNumber) = (user.MobileNumber, user.Shaba, user.AccountNumber);

        await _commonAppService.ValidateSMS(userMobile, userNationalCode, userSmsCode, SMSType.UserRejectionAdvocacy);
        //var saleDetail = await _saleDetailRepository.WithDetails()
        //    .Select(x => new { x.SalePlanStartDate, x.SalePlanEndDate, x.SaleId })
        //    .FirstOrDefaultAsync(x => x.SalePlanStartDate <= DateTime.Now && x.SalePlanEndDate >= DateTime.Now);
        //if (saleDetail == null)
        //    throw new UserFriendlyException("هیچ برنامه فروش فعالی وجود ندارد");
        var saleId = 2;
        //var order = await _commitOrderRepository
        //    .WithDetails()
        //        .Select(x => new
        //        {
        //            x.OrderStatus,
        //            x.UserId
        //        })
        //    .FirstOrDefaultAsync(x => x.UserId == userId && x.OrderStatus == OrderStatusType.RecentlyAdded);
        //if (order != null)
        //{
        //    throw new UserFriendlyException("سفارش در حال بررسی دارید. جهت درخواست حذف از درخواست قبلی خود انصراف دهید");
        //}

        var userRejectionAdvocacyDisable = _configuration.GetValue<bool?>("UserRejectionAdvocacyIsDisable") ?? false;
        if (userRejectionAdvocacyDisable)
            throw new UserFriendlyException("تا اطلاع ثانوی انصراف از طرح های فروش ممکن نیست");
        //await _cacheManager.GetCache("UserRejection").RemoveAsync(userNationalCode);
        await _cacheManager.RemoveAsync(string.Format(RedisConstants.UserRejectionPrefix, userNationalCode),
            "",
            new CacheOptions()
            {
                Provider = CacheProviderEnum.Redis
            });
        await _cacheManager.RemoveAsync(userNationalCode,
            "",
            new CacheOptions()
            {
                Provider = CacheProviderEnum.Redis
            });
        var userRejected = _userRejectionAdcocacyRepository.WithDetails()
            .Select(x => x.NationalCode)
            .FirstOrDefault(x => userNationalCode == x);
        if (userRejected != null)
        {
            throw new UserFriendlyException("شما قبلا انصراف داده اید");
        }
        await _userRejectionAdcocacyRepository.InsertAsync(new UserRejectionAdvocacy()
        {
            Archived = false,
            NationalCode = userNationalCode,
            SaleId = saleId,
            accountNumber = userAccountNumber,
            ShabaNumber = userShaba,
            datetime = DateTime.Now

        });


    }
    [SecuredOperation(OrderAppServicePermissionConstants.UserRejectionStatus)]
    public async Task<bool> UserRejectionStatus()
    {
        string NationalCode = _commonAppService.GetNationalCode();


        string status = await _userRejectionAdcocacyRepository
       .WithDetails()
       .Select(x => x.NationalCode)
       .FirstOrDefaultAsync(x => x == NationalCode);

        return !string.IsNullOrEmpty(status);
    }

    public async Task<List<CustomerOrderReportDto>> GetCompaniesCustomerOrders()
    {
        throw new NotImplementedException();
        //var userId = _abpSession.GetUserId();
        //var user = await _userRepository.GetAsync(userId);
        //if (!_commonAppService.IsInRole("Company"))
        //    throw new UserFriendlyException("دسترسی کاربر جاری برای دیدن سفارشات کافی نیست");
        //if (!user.CompanyId.HasValue)
        //    throw new UserFriendlyException("کاربرجاری شناسه فعال ندارد.");

        //var compayId = user.CompanyId.Value;
        //var commitOrderQuery = (await _commitOrderRepository.GetQueryableAsync())
        //    .Include(x => x.SaleDetail.CarTip.CarType.CarFamily.Company)
        //    .Include(x => x.)
        //var result = await _commitOrderRepository.GetAllIncluding(x => x.SaleDetail.CarTip.CarType.CarFamily.Company,
        //    x => x.User, x => x.User.BirthCity,
        //    x => x.User.HabitationCity,
        //    x => x.User.IssuingCity,
        //    x => x.User.BirthProvince,
        //    x => x.User.HabitationProvince,
        //    x => x.User.IssuingProvince)
        //    .Select(x => new
        //    {
        //        //x.DeliveryDate,
        //        CarName = x.SaleDetail.CarTip.Title,
        //        x.User,
        //        x.OrderStatus,
        //        x.OrderRejectionStatus,
        //        CompanyId = x.SaleDetail.CarTip.CarType.CarFamily.Company.Id,


        //    })
        //    .Where(x => x.OrderRejectionStatus != OrderRejectionType.PhoneNumberAndNationalCodeConflict && x.OrderStatus == OrderStatusType.Winner && x.CompanyId == compayId)
        //    .Select(x => new CustomerOrderReportDto()
        //    {
        //        CarTipName = x.CarName,
        //        CustomerInformation = ObjectMapper.Map<User, UsersCustomerOrdersDto>(x.User, new UsersCustomerOrdersDto()),
        //        //DeliveryDate = x.DeliveryDate
        //    }).ToListAsync();
        //return result;
    }

    public async Task<List<CustomerOrderPriorityUserDto>> GetCustomerInfoPriorityUser()
    {
        throw new NotImplementedException();
        //var userId = CurrentUser.Id;
        //if (!_commonAppService.IsInRole("Company"))
        //    throw new UserFriendlyException("دسترسی کاربر جاری برای دیدن سفارشات کافی نیست");
        //var user = await _esaleGrpcClient.GetUserById(_commonAppService.GetUserId());
        //if (!user.CompanyId.HasValue)
        //    throw new UserFriendlyException("کاربرجاری شناسه فعال ندارد.");
        //var compayId = user.CompanyId.Value;
        //var result = await _commitOrderRepository.WithDetails(x => x.SaleDetail.CarTip.CarType.CarFamily.Company)
        //    .Select(x => new
        //    {
        //        x.OrderStatus,
        //        x.OrderRejectionStatus,
        //        CompanyId = x.SaleDetail.CarTip.CarType.CarFamily.Company.Id,
        //        PriorityUser = x.PriorityUser
        //    })
        //    .Where(x => x.OrderRejectionStatus != OrderRejectionType.PhoneNumberAndNationalCodeConflict && x.OrderStatus == OrderStatusType.Winner && x.CompanyId == compayId)
        //    .Select(x => new CustomerOrderPriorityUserDto()
        //    {
        //        PriorityUser = x.PriorityUser,
        //        CustomerInformation = new UserInfoPriorityDto()
        //        {
        //            BirthCityName = "",
        //            Mobile = 
        //        },

        //    }).ToListAsync();
        //return result;
    }

    public async Task<IPaymentResult> CheckoutPayment(IPgCallBackRequest callBackRequest)
    {
        using (var auditingScope = _auditingManager.BeginScope())
        {

            var (status, paymentId, paymentSecret) =
            (callBackRequest.StatusCode, callBackRequest.PaymentId, callBackRequest.AdditionalData);
            int orderId = default;
            List<OrderLog> comments = new List<OrderLog>();
            var _iPgCallBackLogData = JsonConvert.DeserializeObject<IPgCallBackLogData>(JsonConvert.SerializeObject(callBackRequest));
            try
            {
                comments.Add(new OrderLog
                {
                    Description = "Start CheckPayment",
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(_iPgCallBackLogData))
                });
                var order = (await _commitOrderRepository.GetQueryableAsync())
                  .AsNoTracking()
                  .FirstOrDefault(x => x.PaymentId == paymentId);
                if (order is null)
                {
                    comments.Add(new OrderLog
                    {
                        Description = $"سفارش  وجود ندارد"
                    });
                    return new PaymentResult()
                    {
                        Message = "سفارش  وجود ندارد",
                        Status = 1,
                        PaymentId = 0,
                        OrderId = 0
                    };
                }
                _iPgCallBackLogData.OrderId = order.Id;
                comments.Add(new OrderLog
                {
                    Description = "GetOrder",
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(order))
                });
                if (order.OrderStatus != OrderStatusType.RecentlyAdded)
                {
                    comments.Add(new OrderLog
                    {
                        Description = "Check OrderStatus"
                    });
                    return new PaymentResult()
                    {
                        Message = "سفارش معتبر نمیباشد",
                        Status = 1,
                        PaymentId = 0,
                        OrderId = order.Id
                    };
                }
                if (order is null || (!int.TryParse(paymentSecret, out var numericPaymentSecret) || (order.PaymentSecret.HasValue && order.PaymentSecret.Value != numericPaymentSecret)))
                {

                    comments.Add(new OrderLog
                    {
                        Description = "Check Payment Secret"
                    });
                    return new PaymentResult()
                    {
                        Message = "درخواست معتبر نیست",
                        Status = 1,
                        PaymentId = 0,
                        OrderId = order.Id
                    };
                }
                if (order != null)
                    orderId = order.Id;

                if (status != 0)
                {
                    comments.Add(new OrderLog
                    {
                        Description = "Payment Canceled"
                    });
                    await NotVerifyActions(order.UserId, order.Id);
                    return new PaymentResult()
                    {
                        Message = "عملیات پرداخت ناموفق بود",
                        Status = 1,
                        PaymentId = 0,
                        OrderId = order.Id
                    };
                }


                comments.Add(new OrderLog
                {
                    Description = "Check Capacity Control"
                });
                var capacityControl = await _capacityControlAppService.Validation(order.SaleDetailId, order.AgencyId);
                if (!capacityControl.Success)
                {
                    await _ipgServiceProvider.ReverseTransaction(paymentId);
                    comments.Add(new OrderLog
                    {
                        Description = capacityControl.Message
                    });
                    await NotVerifyActions(order.UserId, order.Id);
                    return new PaymentResult()
                    {
                        Message = capacityControl.Message,
                        Status = 1,
                        PaymentId = 0,
                        OrderId = order.Id
                    };
                }
                comments.Add(new OrderLog
                {
                    Description = "callgrpc GetPaymentInformation"
                });

                var paymentInformation = await _esaleGrpcClient.GetPaymentInformation(paymentId);
                comments.Add(new OrderLog
                {
                    Description = "VerifyTransaction"
                });
                var verificationResponse = await _ipgServiceProvider.VerifyTransaction(paymentId);

                await UpdateStatus(new()
                {
                    Id = order.Id,
                    OrderStatus = (int)OrderStatusType.PaymentSucceeded,
                    SignStatus = SignStatusEnum.PreparingContract,
                    TransactionCommitDate = paymentInformation.TransactionDate,
                    TransactionId = paymentInformation.TransactionCode,
                    PaymentPrice = paymentInformation.Amount
                });
                comments.Add(new OrderLog
                {
                    Description = "Payment Succeeded"
                });
                return new PaymentResult()
                {
                    PaymentId = paymentId,
                    Message = verificationResponse.Result.Message,
                    OrderId = order.Id,
                    Status = status
                };

            }

            catch (Exception e)
            {
                comments.Add(new OrderLog
                {
                    Description = $"عملیات با خطا مواجه شد"
                });
                _auditingManager.Current.Log.Exceptions.Add(e);
                return new PaymentResult()
                {
                    Message = "عملیات با خطا مواجه شد",
                    PaymentId = paymentId,
                    Status = 2,
                    OrderId = orderId
                };
            }

            finally
            {
                _auditingManager.Current.Log.SetProperty("IPgCallBackLog", comments);
                _auditingManager.Current.Log.Comments.Add(JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    { "TransactionCode",_iPgCallBackLogData.TransactionCode},
                    { "PaymentId",_iPgCallBackLogData.PaymentId},
                    { "StatusCode",_iPgCallBackLogData.StatusCode},
                    { "OrderId",_iPgCallBackLogData.OrderId}
                }));
                await auditingScope.SaveAsync();
            }
        }

    }
    private async Task NotVerifyActions(Guid UserId, int OrderId)
    {
        //await _redisCacheManager.RemoveAllAsync(RedisConstants.CommitOrderPrefix + UserId.ToString() + "_*");
        await _cacheManager.RemoveWithPrefixKeyAsync(RedisConstants.CommitOrderPrefix + UserId.ToString());
        await UpdateStatus(new()
        {
            Id = OrderId,
            OrderStatus = (int)OrderStatusType.PaymentNotVerified
        });
        await _commonAppService.SetOrderStep(OrderStepEnum.PreviewOrder, UserId);
    }
    public async Task UpdateStatus(CustomerOrderDto customerOrderDto)
    {
        var order = _objectMapper.Map<CustomerOrderDto, CustomerOrder>(customerOrderDto);

        if (customerOrderDto.OrderStatus == (int)OrderStatusType.PaymentSucceeded)
        {
            await _commitOrderRepository.AttachAsync(order, o => o.OrderStatus, o => o.TransactionCommitDate, o => o.PaymentPrice, o => o.TransactionId, o => o.SignStatus);
        }
        else
        {
            await _commitOrderRepository.AttachAsync(order, o => o.OrderStatus);
        }
        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async Task UpdateSignStatus(CustomerOrderDto customerOrderDto)
    {
        var order = _objectMapper.Map<CustomerOrderDto, CustomerOrder>(customerOrderDto);
        await _commitOrderRepository.AttachAsync(order, o => o.SignStatus);
        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async Task RetryPaymentForVerify()
    {
        var payments = await _esaleGrpcClient.RetryForVerify();
        if (payments == null || payments.Count == 0)
            return;
        payments = payments
            .Where(x => x.PaymentStatus != 1 && x.FilterParam3 != null && x.FilterParam3 != 0)
            .ToList();
        foreach (var payment in payments)
        {
            int orderId = payment.FilterParam3 ?? 0;
            var orderstatus = payment.PaymentStatus == 2 ? (int)OrderStatusType.PaymentSucceeded : (int)OrderStatusType.PaymentNotVerified;
            await UpdateStatus(new CustomerOrderDto()
            {
                Id = orderId,
                OrderStatus = orderstatus,
                SignStatus = orderstatus == (int)OrderStatusType.PaymentSucceeded ? SignStatusEnum.PreparingContract : null
            });
            if (payment.PaymentStatus == 3)
            {
                try
                {
                    var order = _commitOrderRepository.WithDetails()
                   .AsNoTracking()
                   .FirstOrDefault(x => x.Id == orderId);
                    if (order != null)
                        //await _redisCacheManager.RemoveAllAsync(RedisConstants.CommitOrderPrefix + order.UserId.ToString() + "_*");
                        await _cacheManager.RemoveWithPrefixKeyAsync(RedisConstants.CommitOrderPrefix + order.UserId.ToString());
                }
                catch (Exception EX)
                {
                    throw EX;
                }
            }
        }
    }

    public async Task RetryOrderForVerify()
    {
        var deadLine = _configuration.GetValue<int>("RetryOrderForVerifyMinute");
        var orders = _commitOrderRepository
            .WithDetails()
            .AsNoTracking()
            .Where(x => x.OrderStatus == OrderStatusType.RecentlyAdded && x.PaymentId != null)
            .ToList()
            .Where(x => (DateTime.Now - x.CreationTime).TotalMinutes > deadLine)
            .ToList();

        if (orders == null || orders.Count == 0)
            return;
        foreach (var order in orders)
        {
            var payment = await _esaleGrpcClient.GetPaymentInformation(order.PaymentId ?? 0);
            if (payment != null && payment.PaymentStatusId > 1)
            {
                var orderstatus = payment.PaymentStatusId == 2 ? (int)OrderStatusType.PaymentSucceeded : (int)OrderStatusType.PaymentNotVerified;
                await UpdateStatus(new CustomerOrderDto()
                {
                    Id = order.Id,
                    OrderStatus = orderstatus,
                    SignStatus = orderstatus == (int)OrderStatusType.PaymentSucceeded ? SignStatusEnum.PreparingContract : null
                });
                if (payment.PaymentStatusId == 3)
                    await _cacheManager.RemoveWithPrefixKeyAsync(RedisConstants.CommitOrderPrefix + order.UserId.ToString());
            }
        }
    }

    [SecuredOperation(OrderAppServicePermissionConstants.GetDetail)]
    public async Task<CustomerOrder_OrderDetailDto> GetDetail(SaleDetail_Order_InquiryDto inquiryDto)
    {
        CustomerOrder_OrderDetailDto inquiryResult = new();

        var exception = new UserFriendlyException("درخواست معتبر نیست");
        if (!inquiryDto.SaleDetailUid.HasValue && !inquiryDto.OrderId.HasValue)
            throw exception;

        switch (inquiryDto)
        {
            case SaleDetail_Order_InquiryDto dto when dto.OrderId.HasValue:
                inquiryResult = await GetOrderDetailById(dto.OrderId.Value, inquiryDto.AttachmentType, inquiryDto.AttachmentLocation);
                break;
            case SaleDetail_Order_InquiryDto dto when dto.SaleDetailUid.HasValue:
                await _commonAppService.ValidateOrderStep(OrderStepEnum.PreviewOrder);
                inquiryResult = await GetSaleDetailByUid(dto.SaleDetailUid.Value, inquiryDto.AttachmentType, inquiryDto.AttachmentLocation);
                await _commonAppService.SetOrderStep(OrderStepEnum.PreviewOrder);
                break;
            default:
                throw exception;
        }
        return inquiryResult;
    }

    [SecuredOperation(OrderAppServicePermissionConstants.GetSaleDetailByUid)]
    public async Task<CustomerOrder_OrderDetailDto> GetSaleDetailByUid(Guid saleDetailUid, List<AttachmentEntityTypeEnum> attachmentEntityType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        //if (!_commonAppService.IsInRole("Customer"))
        //{
        //    throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        //}
        var userId = _commonAppService.GetUserId();
        var saleDetailQuery = (await _saleDetailRepository.GetQueryableAsync()).Include(x => x.Product);
        var saleDetail = saleDetailQuery.AsNoTracking()
            .Select(y => new CustomerOrder_OrderDetailDto
            {
                CarDeliverDate = y.CarDeliverDate,
                ESaleTypeId = (ESaleTypeEnums)y.ESaleTypeId,
                SaleDetailUid = y.UID,
                MinimumAmountOfProxyDeposit = y.MinimumAmountOfProxyDeposit,
                ManufactureDate = y.ManufactureDate,
                DeliveryDate = y.CarDeliverDate,
                SalePlanEndDate = y.SalePlanEndDate,
                ProductId = y.ProductId,
                Product = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryViewModel>(y.Product)
            })
            .FirstOrDefault(x => x.SaleDetailUid == saleDetailUid);

        if (saleDetail.SalePlanEndDate <= DateTime.Now)
            throw new UserFriendlyException("تاریخ برنامه فروش به پایان و سفارش قابل مشاده نیست");


        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, new List<int> { saleDetail.ProductId }.ToList(), attachmentEntityType, attachmentlocation);
        var attachment = attachments.Where(y => y.EntityId == saleDetail.ProductId).ToList();
        saleDetail.Product.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        var user = await _esaleGrpcClient.GetUserId(_commonAppService.GetUserId().ToString());
        saleDetail.SurName = user.SurName;
        saleDetail.Name = user.Name;
        saleDetail.NationalCode = user.NationalCode;
        return saleDetail;
    }

    [SecuredOperation(OrderAppServicePermissionConstants.GetOrderDetailById)]
    public async Task<OrderDetailDto> GetReportOrderDetail(int id)
    {
        var userId = _commonAppService.GetUserId();
        var orderStatusTypes = _orderStatusTypeReadOnlyRepository.WithDetails().ToList();
        var customerOrderQuery = await _commitOrderRepository.GetQueryableAsync();
        PaymentInformationResponseDto paymentInformation = new();
        var customerOrder = customerOrderQuery
            .AsNoTracking()
            .Join(_saleDetailRepository.WithDetails(x => x.Product),
            x => x.SaleDetailId,
            y => y.Id,
            (x, y) => new OrderDetailDto()
            {
                UserId = x.UserId,
                CreationTime = x.CreationTime,
                OrderId = x.Id,
                ProductTitle = y.Product.Title,
                PaymentPrice = x.PaymentPrice,
                TransactionId = x.TransactionId,
                TransactionCommitDate = x.TransactionCommitDate,
                ContractNumber=x.ContractNumber
                //PspTitle = ?? 
            }).FirstOrDefault(x => x.UserId == userId && x.OrderId == id);
        var user = await _esaleGrpcClient.GetUserId(customerOrder.UserId.ToString());
        var cityIds = new List<int>();
        var cities = (await _cityRepository.GetQueryableAsync())
            .AsNoTracking()
            .Where(x => x.Id == (user.IssuingCityId ?? 0) || x.Id == (user.BirthCityId ?? 0))
            .ToList();
        customerOrder.SurName = user.SurName;
        customerOrder.Name = user.Name;
        customerOrder.NationalCode = user.NationalCode;
        customerOrder.Mobile = user.MobileNumber;
        customerOrder.Address = user.Address;
        customerOrder.IssuingCityTitle = user.IssuingCityId.HasValue ? cities?.FirstOrDefault(x => x.Id == user.IssuingCityId.Value)?.Name : string.Empty;
        customerOrder.BirthCertId = user.BirthCertId;
        customerOrder.BirthDate = user.BirthDate;
        customerOrder.BirthCityTitle = user.BirthCityId.HasValue ? cities?.FirstOrDefault(x => x.Id == user.BirthCityId.Value)?.Name : string.Empty;
        customerOrder.PostalCode = user.PostalCode;


        return customerOrder;
    }

    [SecuredOperation(OrderAppServicePermissionConstants.GetOrderDetailById)]
    public async Task<CustomerOrder_OrderDetailDto> GetOrderDetailById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        //if (!_commonAppService.IsInRole("Customer"))
        //{
        //    throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        //}
        var userId = _commonAppService.GetUserId();
        var orderStatusTypes = _orderStatusTypeReadOnlyRepository.WithDetails().ToList();
        var customerOrderQuery = await _commitOrderRepository.GetQueryableAsync();
        PaymentInformationResponseDto paymentInformation = new();
        var customerOrder = customerOrderQuery
            .AsNoTracking()
            .Join(_saleDetailRepository.WithDetails(x => x.Product),
            x => x.SaleDetailId,
            y => y.Id,
            (x, y) => new CustomerOrder_OrderDetailDto()
            {
                CarDeliverDate = y.CarDeliverDate,
                CreationTime = x.CreationTime,
                OrderId = x.Id,
                SaleDescription = y.SalePlanDescription,
                UserId = x.UserId,
                OrderStatusCode = (int)x.OrderStatus,
                PriorityId = x.PriorityId.HasValue ? (int)x.PriorityId : null,
                DeliveryDateDescription = x.DeliveryDateDescription,
                DeliveryDate = x.DeliveryDate,
                OrderRejectionCode = x.OrderRejectionStatus.HasValue ? (int)x.OrderRejectionStatus : null,
                ESaleTypeId = (ESaleTypeEnums)y.ESaleTypeId,
                ManufactureDate = y.ManufactureDate,
                SaleDetailUid = y.UID,
                SalePlanEndDate = y.SalePlanEndDate,
                ProductId = y.ProductId,
                Product = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryViewModel>(y.Product),
                PaymentId = x.PaymentId,
                TransactionCommitDate = x.TransactionCommitDate,
                PaymentPrice = x.PaymentPrice,
                TransactionId = x.TransactionId,
            }).FirstOrDefault(x => x.UserId == userId && x.OrderId == id);

        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, new List<int> { customerOrder.ProductId }.ToList(), attachmentType, attachmentlocation);
        var attachment = attachments.Where(y => y.EntityId == customerOrder.ProductId).ToList();
        customerOrder.Product.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);

        if (customerOrder.SalePlanEndDate <= DateTime.Now)
            throw new UserFriendlyException("تاریخ برنامه فروش به پایان و سفارش قابل مشاده نیست");


        var user = await _esaleGrpcClient.GetUserId(customerOrder.UserId.ToString());
        customerOrder.SurName = user.SurName;
        customerOrder.Name = user.Name;
        customerOrder.NationalCode = user.NationalCode;
        return customerOrder;
    }

    public async Task<bool> NationalCodeExistsInPriority(string nationalCode)
    {
        var priority = (await _priorityRepository.GetQueryableAsync())
            .AsNoTracking()
            .Select(x => x.NationalCode)
            .FirstOrDefault(x => x == nationalCode);
        return !string.IsNullOrEmpty(priority);
    }

    public async Task<List<ClientOrderDetailDto>> GetOrderDetailFromOrganizationList()
    {
        var clientOrderDetailDto = await _companyGrpcClient.GetOrderDetailList();
        return clientOrderDetailDto;

    }
}
