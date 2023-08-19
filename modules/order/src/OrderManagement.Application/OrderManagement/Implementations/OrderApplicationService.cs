using System.Text.RegularExpressions;
using System.Text;
using OrderManagement.Domain.Bases;
using OrderManagement.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Microsoft.Extensions.Caching.Distributed;
using OrderManagement.Application.Contracts.Services;
using System.Collections.Generic;
using System;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using OrderManagement.Domain.Shared;
using OrderManagement.Application.OrderManagement.Constants;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrderAppService : ApplicationService, IOrderAppService
{
    private readonly ICommonAppService _commonAppService;
    private readonly IBaseInformationService _baseInformationAppService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IRepository<SaleDetail, int> _saleDetailRepository;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private readonly IRepository<AdvocacyUser, int> _advocacyUsers;
    private readonly IRepository<CustomerOrder, int> _commitOrderRepository;
    private readonly IRepository<Logs, long> _logsRepository;
    private readonly IRepository<OrderStatusTypeReadOnly, int> _orderStatusTypeReadOnlyRepository;
    private readonly IRepository<OrderRejectionTypeReadOnly, int> _orderRejectionTypeReadOnlyRepository;
    private readonly IEsaleGrpcClient _esaleGrpcClient;
    private IConfiguration _configuration { get; set; }
    private readonly IDistributedCache _distributedCache;
    private readonly IRepository<Company, int> _companyRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IRepository<ESaleType, int> _esaleTypeRepository;
    private readonly IMemoryCache _memoryCache;

    public OrderAppService(ICommonAppService commonAppService,
                           IBaseInformationService baseInformationAppService,
                           IHttpContextAccessor contextAccessor,
                           IRepository<SaleDetail, int> saleDetailRepository,
                           IRepository<UserRejectionAdvocacy, int> userRejectionAdcocacyRepository,
                           IRepository<AdvocacyUser, int> advocacyUsers,
                           IRepository<CustomerOrder, int> commitOrderRepository,

                           IRepository<Logs, long> logsRepository,
                           IRepository<OrderStatusTypeReadOnly, int> orderStatusTypeReadOnlyRepository,
                           IRepository<OrderRejectionTypeReadOnly, int> orderRejectionTypeReadOnlyRepository
        ,
                           IEsaleGrpcClient esaleGrpcClient,
                           IConfiguration configuration,
                           IDistributedCache distributedCache,
                           IRepository<Company, int> companyRepository,
                           IUnitOfWorkManager UnitOfWorkManager,
                           IRepository<ESaleType, int> esaleTypeRepository,
                           IMemoryCache memoryCache
        )
    {
        _commonAppService = commonAppService;
        _baseInformationAppService = baseInformationAppService;
        _contextAccessor = contextAccessor;
        _saleDetailRepository = saleDetailRepository;
        _userRejectionAdcocacyRepository = userRejectionAdcocacyRepository;
        _advocacyUsers = advocacyUsers;
        _commitOrderRepository = commitOrderRepository;

        _logsRepository = logsRepository;
        _orderStatusTypeReadOnlyRepository = orderStatusTypeReadOnlyRepository;
        _orderRejectionTypeReadOnlyRepository = orderRejectionTypeReadOnlyRepository;

        _esaleGrpcClient = esaleGrpcClient;
        _distributedCache = distributedCache;
        _configuration = configuration;
        _companyRepository = companyRepository;
        _unitOfWorkManager = UnitOfWorkManager;
        _esaleTypeRepository = esaleTypeRepository;
        _memoryCache = memoryCache;
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

    private void RustySalePlanValidation(CommitOrderDto commitOrder, int esaleTypeId)
    {
        //TODO: make sure esale type name is quite right
        //const string targetEsaleTypeName = "طرح فروش فرسوده";
        //var esaleTypeQuery = await _esaleTypeRepository.GetQueryableAsync();
        //var esaleType = esaleTypeQuery.Select(x => new
        //{
        //    x.SaleTypeName,
        //    x.Id
        //}).FirstOrDefault(x => x.Id == esaleTypeId);
        //if (esaleType == null)
        //    throw new EntityNotFoundException(typeof(ESaleType), esaleTypeId);
        if (esaleTypeId == 3)
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
            commitOrder.Vin = commitOrder.Vin.ToUpper();
            return;
        }
        commitOrder.EngineNo = "";
        commitOrder.ChassiNo = "";
        commitOrder.Vin = "";
        commitOrder.Vehicle = "";
    }


    [Audited]
    [UnitOfWork(isTransactional: false)]
    public async Task CommitOrder(CommitOrderDto commitOrderDto)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        TimeSpan ttl = DateTime.Now.Subtract(DateTime.Now);

        if (!_commonAppService.IsInRole("Customer"))
            throw new UserFriendlyException("دسترسی شما کافی نمی باشد");

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
        var cacheKey = string.Format(RedisConstants.SaleDetailPrefix, commitOrderDto.SaleDetailUId);
        _memoryCache.TryGetValue(cacheKey, out SaleDetailDto);
        if (SaleDetailDto == null)
        {
            var saleDetailQuery = await _saleDetailRepository.GetQueryableAsync();
            var SaleDetailFromDb = saleDetailQuery
                .Select(x => new SaleDetailOrderDto
                {
                    EsaleTypeId = x.ESaleTypeId,
                    Id = x.Id,
                    MinimumAmountOfProxyDeposit = x.MinimumAmountOfProxyDeposit,
                    SaleId = x.SaleId,
                    SalePlanEndDate = x.SalePlanEndDate,
                    SalePlanStartDate = x.SalePlanStartDate,
                    UID = x.UID,
                    ESaleTypeId = x.ESaleTypeId
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
                _memoryCache.Set(string.Format(RedisConstants.SaleDetailPrefix, commitOrderDto.SaleDetailUId.ToString()), SaleDetailDto, DateTime.Now.AddMinutes(4));

                ////await _cacheManager.GetCache("SaleDetail").SetAsync(commitOrderDto.SaleDetailUId.ToString(), SaleDetailDto);
                //await _distributedCache.SetStringAsync(string.Format(RedisConstants.SaleDetailPrefix, commitOrderDto.SaleDetailUId.ToString()),
                //    JsonConvert.SerializeObject(SaleDetailDto), new DistributedCacheEntryOptions()
                //    {
                //        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                //    });
            }
        }
        else
        {
            ttl = SaleDetailDto.SalePlanEndDate.Subtract(DateTime.Now);

        }
        //if(SaleDetailDto.EsaleTypeId == (Int16)EsaleTypeEnum.Youth)
        //{
        //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        //    object UserFromCache = null;
        //    _cacheManager.GetCache("UserProf").TryGetValue(AbpSession.UserId.Value.ToString(), out UserFromCache);
        //    if (UserFromCache != null)
        //    {
        //        var userDto = UserFromCache as UserDto;
        //        if (userDto.Gender = Enums.Gender.Male)
        //        {
        //            throw new UserFriendlyException("با توجه به جنسیت، شما مجاز به شرکت در این طرح نیستید");
        //        }
        //    }
        //    else
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == AbpSession.UserId);
        //        _cacheManager.GetCache("UserProf").Set(AbpSession.UserId.Value.ToString(), ObjectMapper.Map<UserDto>(user));

        //    }
        //}
        ////////////////conntrol repeated order in saledetails// iran&&varedat

        CheckSaleDetailValidation(SaleDetailDto);
        RustySalePlanValidation(commitOrderDto, SaleDetailDto.EsaleTypeId);
        await _commonAppService.IsUserRejected(); //if user reject from advocacy
                                                  //_baseInformationAppService.CheckBlackList(SaleDetailDto.EsaleTypeId); //if user not exsist in blacklist
        await CheckAdvocacy(nationalCode,SaleDetailDto.ESaleTypeId); //if hesab vekalati darad
        _baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhiteListOrder);

        var orderQuery = await _commitOrderRepository.GetQueryableAsync();
        var userId = _commonAppService.GetUserId();
        ///////////////////////////////agar order 1403 barandeh dasht natone sefaresh bezaneh
        var CustomerOrderWinner = orderQuery
             .AsNoTracking()
             .Select(x => new { x.UserId, x.OrderStatus })
             .FirstOrDefault(
                 y => y.UserId == userId &&
              y.OrderStatus == OrderStatusType.Winner
          );
        if (CustomerOrderWinner != null)
            throw new UserFriendlyException("جهت ثبت سفارش جدید لطفا ابتدا از جزئیات سفارش، سفارش قبلی خود که موعد تحویل آن در سال 1403 می باشد را لغو نمایید .");
        ///////////////////////////////check entekhab yek no tarh////////////
        string EsaleTypeId = await _distributedCache.GetStringAsync(userId.ToString());
        if (!string.IsNullOrEmpty(EsaleTypeId))
        {
            if (EsaleTypeId != SaleDetailDto.EsaleTypeId.ToString())
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
                if (SaleDetailDto.ESaleTypeId != activeSuccessfulOrderExists.ESaleTypeId)
                {
                    await _distributedCache.SetStringAsync(userId.ToString(), activeSuccessfulOrderExists.ESaleTypeId.ToString());

                    throw new UserFriendlyException("امکان انتخاب فقط یک نوع طرح فروش وجود دارد");
                }
            }

        }



        ///////////////////////////////////iran/////////////
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            object objectCommitOrderIran = null;
            //_cacheManager.GetCache("CommitOrderIran").
            //TryGetValue(
            //    userId.ToString() + "_" +
            //    SaleDetailDto.SaleId.ToString()
            //    , out objectCommitOrderIran);
            objectCommitOrderIran = await _distributedCache.GetStringAsync(userId.ToString() + "_" + SaleDetailDto.SaleId.ToString());
            if (objectCommitOrderIran != null)
            {
                throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید از درخواست قبلی خود انصراف دهید");
            }
            else
            {
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
                   x.OrderStatus == OrderStatusType.RecentlyAdded
                   );
                if (customerOrderIranFromDb != null)
                {
                    //await _cacheManager.GetCache("CommitOrderIran").
                    //   SetAsync(
                    //       userId.ToString() + "_" +
                    //       SaleDetailDto.SaleId.ToString()
                    //       , customerOrderIranFromDb.Id
                    //       , TimeSpan.FromSeconds(ttl.TotalSeconds));

                    await _distributedCache.SetStringAsync(userId.ToString() + "_" +
                           SaleDetailDto.SaleId.ToString(),
                           customerOrderIranFromDb.Id.ToString(),
                           new DistributedCacheEntryOptions()
                           {
                               AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                           });
                    throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید از درخواست قبلی خود انصراف دهید");


                }
            }
        }
        ///////////////////////////////vardati chek dar bakhshnameh///////////////////////////
        else
        {
            object objectCommitOrderIran = null;
            //_cacheManager.GetCache("CommitOrderimport").
            //TryGetValue(
            //    userId.ToString() + "_" +
            //    commitOrderDto.PriorityId.ToString() + "_" +
            //    SaleDetailDto.SaleId.ToString()
            //    , out objectCommitOrderIran);
          
            objectCommitOrderIran = await _distributedCache.GetStringAsync(userId.ToString() + "_" +
                commitOrderDto.PriorityId.ToString() + "_" +
                SaleDetailDto.SaleId.ToString());

            if (objectCommitOrderIran != null)
            {
                throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید، درخواست قبلی خود را لغو نمایید یا اولویت دیگری را انتخاب نمایید");
            }
            else
            {
              
                CustomerOrder customerOrderIranFromDb =
                orderQuery
                .AsNoTracking()
                .Select(x => new CustomerOrder
                {
                    OrderStatus = x.OrderStatus,
                    SaleId = x.SaleId,
                    PriorityId = x.PriorityId,
                    UserId = x.UserId
                })

                .FirstOrDefault(y =>
                   y.UserId == userId &&
                   y.OrderStatus == OrderStatusType.RecentlyAdded
                   && y.SaleId == SaleDetailDto.SaleId
                   && y.PriorityId == (PriorityEnum)commitOrderDto.PriorityId);
             

                if (customerOrderIranFromDb != null)
                {
                    //await _cacheManager.GetCache("CommitOrderimport").
                    //   SetAsync(
                    //      userId.ToString() + "_" +
                    //        commitOrderDto.PriorityId.ToString() + "_" +
                    //        SaleDetailDto.SaleId.ToString()
                    //       , customerOrderIranFromDb.Id
                    //       , TimeSpan.FromSeconds(ttl.TotalSeconds));
                    await _distributedCache.SetStringAsync(
                          userId.ToString() + "_" +
                            commitOrderDto.PriorityId.ToString() + "_" +
                            SaleDetailDto.SaleId.ToString()
                           , customerOrderIranFromDb.Id.ToString(),
                          new DistributedCacheEntryOptions
                          {
                              AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                          });
                    throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید، درخواست قبلی خود را لغو نمایید یا اولویت دیگری را انتخاب نمایید");
                }

            }
            object objectCustomerOrderFromCache = null;
            //_cacheManager.GetCache("CommitOrderimport")
            //    .TryGetValue(
            //    userId.ToString() + "_" +
            //        SaleDetailDto.Id.ToString(),
            //    out objectCustomerOrderFromCache
            //    );

            objectCustomerOrderFromCache = await _distributedCache.GetStringAsync(
                userId.ToString() + "_" +
                    SaleDetailDto.Id.ToString());

            if (objectCustomerOrderFromCache != null)
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
                && x.OrderStatus == OrderStatusType.RecentlyAdded);

                if (CustomerOrderFromDb != null)
                {
                    // await _cacheManager.GetCache("CommitOrderimport").
                    //SetAsync(
                    //    userId.ToString() + "_" +
                    //    SaleDetailDto.Id.ToString()
                    //    , CustomerOrderFromDb.Id
                    //    , TimeSpan.FromSeconds(ttl.TotalSeconds));
                    await _distributedCache.SetStringAsync(userId.ToString() + "_" +
                       SaleDetailDto.Id.ToString()
                       , CustomerOrderFromDb.Id.ToString(),
                       new DistributedCacheEntryOptions()
                       {
                           AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                       });
                    throw new UserFriendlyException("این خودرو را قبلا انتخاب نموده اید.");

                }
            }



        }
        ////////////////////////






        CustomerOrder customerOrder = new CustomerOrder();
        customerOrder.SaleDetailId = (int)SaleDetailDto.Id;
        customerOrder.UserId = (int)userId;
        customerOrder.PriorityId = (PriorityEnum)commitOrderDto.PriorityId;
        customerOrder.Vin = commitOrderDto.Vin;
        customerOrder.ChassiNo = commitOrderDto.ChassiNo;
        customerOrder.EngineNo = commitOrderDto.EngineNo;
        customerOrder.Vehicle = commitOrderDto.Vehicle;
        customerOrder.OrderStatus = OrderStatusType.RecentlyAdded;
        customerOrder.SaleId = SaleDetailDto.SaleId;
        await _commitOrderRepository.InsertAsync(customerOrder);
        await CurrentUnitOfWork.SaveChangesAsync();

        //unitOfWork.Complete();
        //}
        //await _cacheManager.GetCache("CommitOrder").
        //           SetAsync(
        //       userId.ToString() + "_" +
        //       commitOrderDto.SaleDetailUId.ToString()
        //       , customerOrder.Id,
        //       TimeSpan.FromSeconds(ttl.TotalSeconds));
        await _distributedCache.SetStringAsync(
            userId.ToString() + "_" +
               commitOrderDto.SaleDetailUId.ToString()
               , customerOrder.Id.ToString(),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
            });

        await _distributedCache.SetStringAsync(userId.ToString(), SaleDetailDto.ESaleTypeId.ToString());
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
            await _distributedCache.SetStringAsync(userId.ToString() + "_" +
                  SaleDetailDto.SaleId.ToString()
                , customerOrder.Id.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                });
        }//vardat
        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            //await _cacheManager.GetCache("CommitOrderimport").
            //           SetAsync(
            //              userId.ToString() + "_" +
            //                commitOrderDto.PriorityId.ToString() + "_" +
            //                SaleDetailDto.SaleId.ToString()
            //               , customerOrder.Id,
            //              TimeSpan.FromSeconds(ttl.TotalSeconds));
            await _distributedCache.SetStringAsync(userId.ToString() + "_" +
                            commitOrderDto.PriorityId.ToString() + "_" +
                            SaleDetailDto.SaleId.ToString()
                           , customerOrder.Id.ToString(),
                           new DistributedCacheEntryOptions()
                           {
                               AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                           });

            await _distributedCache.SetStringAsync(userId.ToString() + "_" +
                     SaleDetailDto.Id.ToString()
                     , customerOrder.Id.ToString(),
                     new DistributedCacheEntryOptions
                     {
                         AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                     });

            await _distributedCache.SetStringAsync(userId.ToString()
                   , SaleDetailDto.ESaleTypeId.ToString(),
                   new DistributedCacheEntryOptions
                   {
                       AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl.TotalSeconds))
                   });
            //await _cacheManager.GetCache("CommitOrderimport").
            //     SetAsync(
            //         userId.ToString() + "_" +
            //         SaleDetailDto.Id.ToString()
            //         , customerOrder.Id
            //         , TimeSpan.FromSeconds(ttl.TotalSeconds));
        }





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
    public List<CustomerOrder_OrderDetailDto> GetCustomerOrderList()
    {
        if (!_commonAppService.IsInRole("Customer"))
        {
            throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        }
        var userId = _commonAppService.GetUserId();
        var orderRejections = _orderRejectionTypeReadOnlyRepository.WithDetails().ToList();
        var orderStatusTypes = _orderStatusTypeReadOnlyRepository.WithDetails().ToList();
        //var customerOrder = await _commitOrderRepository.GetAllListAsync(x => x.UserId == userId);
        var customerOrders = _commitOrderRepository.WithDetails()
            .AsNoTracking()
            .Join(_saleDetailRepository.WithDetails(x => x.CarTip.CarType.CarFamily.Company),
            x => x.SaleDetailId,
            y => y.Id,
            (x, y) => new
            {
                y.CarDeliverDate,
                y.SalePlanDescription,
                CompanyName = y.CarTip.CarType.CarFamily.Company.Name,
                CarFamilyTitle = y.CarTip.CarType.CarFamily.Title,
                CarTypeTitle = y.CarTip.CarType.Title,
                CarTipTitle = y.CarTip.Title,
                x.UserId,
                x.OrderStatus,
                x.CreationTime,
                OrderId = x.Id,
                PriorityId = x.PriorityId,
                x.DeliveryDate,
                x.DeliveryDateDescription,
                x.OrderRejectionStatus,
                y.ESaleTypeId,
                y.SalePlanEndDate
              
            }).Where(x => x.UserId == userId)
            .Select(x => new CustomerOrder_OrderDetailDto
            {
                CarDeliverDate = x.CarDeliverDate,
                CarFamilyTitle = x.CarFamilyTitle,
                CarTipTitle = x.CarTipTitle,
                CarTypeTitle = x.CarTipTitle,
                CompanyName = x.CompanyName,
                CreationTime = x.CreationTime,
                OrderId = x.OrderId,
                SaleDescription = x.SalePlanDescription,
                UserId = x.UserId,
                OrderStatusCode = (int)x.OrderStatus,
                PriorityId = x.PriorityId.HasValue ? (int)x.PriorityId : null,
                DeliveryDateDescription = x.DeliveryDateDescription,
                DeliveryDate = x.DeliveryDate,
                OrderRejectionCode = x.OrderRejectionStatus.HasValue ? (int)x.OrderRejectionStatus : null,
                ESaleTypeId = x.ESaleTypeId,
                SalePlanEndDate = x.SalePlanEndDate
            }).ToList();
        var cancleableDate = _configuration.GetValue<string>("CancelableDate");
        customerOrders.ForEach(x =>
        {
            var orderStatusType = orderStatusTypes.FirstOrDefault(y => y.OrderStatusCode == x.OrderStatusCode);
            x.OrderstatusTitle = orderStatusType.OrderStatusTitle;
            if (x.OrderRejectionCode.HasValue)
            {
                var orderRejection = orderRejections.FirstOrDefault(y => y.OrderRejectionCode == x.OrderRejectionCode);
                x.OrderRejectionTitle = orderRejection.OrderRejectionTitle;
                if ((OrderRejectionType)x.OrderRejectionCode == OrderRejectionType.PhoneNumberAndNationalCodeConflict)
                    x.DeliveryDate = null;
            }

            // if (x.OrderStatusCode == 10 && (x.SalePlanEndDate >= DateTime.Now)) // OrderStatusType.RecentlyAdded
            if ((x.OrderStatusCode == 40 && x.SaleId == 3 && !string.IsNullOrEmpty(x.DeliveryDateDescription) && x.DeliveryDateDescription.Contains("1403")) || (x.OrderStatusCode == 10 && x.SaleId == 4)) // OrderStatusType.RecentlyAdded
                x.Cancelable = true;
            else
                x.Cancelable = false;
            x.SalePlanEndDate = null;
            //else if (x.OrderStatusCode == 40 && x.DeliveryDateDescription.Contains(cancleableDate, StringComparison.InvariantCultureIgnoreCase)) // OrderStatusType.Winner
            //    x.Cancelable = true;
            //x.Cancelable = false;
        });
        return customerOrders.OrderByDescending(x => x.OrderId).ToList();
    }
    [Audited]
    [UnitOfWork(isTransactional: false)]
    public async Task<CustomerOrderDto> CancelOrder(int orderId)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        var userId = _commonAppService.GetUserId();
        if (!_commonAppService.IsInRole("Customer"))
        {
            throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        }
        var customerOrder = _commitOrderRepository.WithDetails(x => x.SaleDetail).FirstOrDefault(x => x.Id == orderId);
        if (customerOrder == null)
            throw new UserFriendlyException("شماره سفارش صحیح نمی باشد");

        if (!(customerOrder.OrderStatus == OrderStatusType.RecentlyAdded))
            throw new UserFriendlyException("امکان انصراف وجود ندارد");

        if (customerOrder.UserId != userId)
            throw new UserFriendlyException("شماره سفارش صحیح نمی باشد");


        SaleDetailOrderDto saleDetailOrderDto;
        var saleDetailCahce = await _distributedCache.GetStringAsync(string.Format(RedisConstants.SaleDetailPrefix, customerOrder.SaleDetailId.ToString()));
        if (!string.IsNullOrWhiteSpace(saleDetailCahce))
        {

            //if (_cacheManager.GetCache("SaleDetail").TryGetValue(customerOrder.SaleDetailId.ToString(), out var saleDetailFromCache))
            //{
            saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(JsonConvert.DeserializeObject<SaleDetail>(saleDetailCahce));
            //}
        }
        else
        {
            var saleDetail = _saleDetailRepository.WithDetails().FirstOrDefault(x => x.Id == customerOrder.SaleDetailId)
                ?? throw new UserFriendlyException("جزئیات برنامه فروش یافت نشد");
            saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(saleDetail);
            //await _cacheManager.GetCache("SaleDetail").SetAsync(saleDetailOrderDto.Id.ToString(), saleDetailOrderDto);
            await _distributedCache.SetStringAsync(string.Format(RedisConstants.SaleDetailPrefix, customerOrder.SaleDetailId.ToString()),
                JsonConvert.SerializeObject(saleDetailOrderDto),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(1))
                });

        }
        if (!((customerOrder.OrderStatus == OrderStatusType.Winner && saleDetailOrderDto.SaleId == 3 && !string.IsNullOrEmpty(customerOrder.DeliveryDateDescription) && customerOrder.DeliveryDateDescription.Contains("1403")) || (customerOrder.OrderStatus == OrderStatusType.RecentlyAdded && saleDetailOrderDto.SaleId == 4))) // OrderStatusType.RecentlyAdded
        {
            throw new UserFriendlyException("امکان انصراف وجود ندارد");
        }
      //  CheckSaleDetailValidation(saleDetailOrderDto);
        _baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhilteListEnseraf);

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
            await _distributedCache.RemoveAsync(
                userId.ToString() + "_" +
                  customerOrder.SaleId.ToString());
        }//vardat
        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            //await _cacheManager.GetCache("CommitOrderimport").
            //           RemoveAsync(
            //              userId.ToString() + "_" +
            //                customerOrder.PriorityId.ToString() + "_" +
            //                customerOrder.SaleId.ToString())
            //               ;
            await _distributedCache.RemoveAsync(userId.ToString() + "_" +
                            customerOrder.PriorityId.ToString() + "_" +
                            customerOrder.SaleId.ToString());
            //await _cacheManager.GetCache("CommitOrderimport").
            //     RemoveAsync(
            //         userId.ToString() + "_" +
            //         customerOrder.SaleDetailId.ToString()
            //         );
            await _distributedCache.RemoveAsync(
                     userId.ToString() + "_" +
                     customerOrder.SaleDetailId.ToString());
        }



        customerOrder.OrderStatus = OrderStatusType.Canceled;
        await CurrentUnitOfWork.SaveChangesAsync();
        customerOrder = _commitOrderRepository.WithDetails().
           FirstOrDefault(x => x.UserId == userId
           && x.SaleDetailId == saleDetailOrderDto.Id
           && x.OrderStatus == OrderStatusType.RecentlyAdded);
        if (customerOrder == null)
        {
            await _distributedCache.RemoveAsync(userId.ToString());
        }

        return ObjectMapper.Map<CustomerOrder, CustomerOrderDto>(customerOrder, new CustomerOrderDto());
    }



    [Audited]
    public async Task InsertUserRejectionAdvocacyPlan(string userSmsCode)
    {

        if (!_commonAppService.IsInRole("Customer"))
        {
            throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        }
        //var userId = _abpSession.UserId ?? throw new UserFriendlyException("لطفا لاگین کنید");
        //var user = _userRepository.FirstOrDefault(userId);
        var userNationalCode = _commonAppService.GetNationalCode();
        var userId = _commonAppService.GetUserId();
        var user = await _esaleGrpcClient.GetUserById(userId);
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
        //TODO: check removing the right key
        _baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhiteListOrder, userNationalCode);
        await _distributedCache.RemoveAsync(string.Format(RedisConstants.UserRejectionPrefix, userNationalCode));
        await _distributedCache.RemoveAsync(userNationalCode);

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
}
