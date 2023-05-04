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
using Abp.Runtime.Caching;
using Volo.Abp.Application.Services;
using System.Linq;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkAttribute = Volo.Abp.Uow.UnitOfWorkAttribute;
using System.Data;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrderApplicationService : ApplicationService, IOrderAppService
{
    private readonly ICommonAppService _commonAppService;
    private readonly IBaseInformationService _baseInformationAppService;
    private readonly IHttpContextAccessor _contextAccessor;
    //private readonly IRandomGenerator _iRandomGenerator;
    private readonly ICacheManager _cacheManager;
    //private readonly IRepository<User, long> _userRepository;
    private readonly IRepository<SaleDetail, int> _saleDetailRepository;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private readonly IRepository<CustomerOrder, int> _commitOrderRepository;
    private readonly IRepository<Logs, long> _logsRepository;
    private readonly IBankAppService _bankAppService;
    //private readonly IAbpSession _abpSession;
    private readonly IRepository<OrderStatusTypeReadOnly, int> _orderStatusTypeReadOnlyRepository;
    private readonly IRepository<OrderRejectionTypeReadOnly, int> _orderRejectionTypeReadOnlyRepository;
    private IConfiguration _configuration { get; set; }

    public OrderApplicationService(ICommonAppService CommonAppService,
                                   //IRandomGenerator RandomGenerator,
                                   IHttpContextAccessor ContextAccessor,
                                   ICacheManager CacheManager,
                                   //IRepository<User, long> UserRepository,
                                   IRepository<Logs, long> LogsRepository,
                                   //IUnitOfWorkManager UnitOfWorkManager,
                                   IRepository<UserRejectionAdvocacy, int> UserRejectionAdcocacyRepository,
                                   Microsoft.Extensions.Configuration.IConfiguration Configuration,
                                   IRepository<CustomerOrder, int> CommitOrderRepository,
                                   IRepository<SaleDetail, int> SaleDetailRepository,
                                   IBaseInformationService BaseInformationAppService,
                                   IBankAppService BankAppService,
                                   IRepository<OrderStatusTypeReadOnly, int> orderStatusTypeReadOnlyRepository,
                                   IRepository<OrderRejectionTypeReadOnly, int> orderRejectionTypeReadOnlyRepository
            )
    {
        _configuration = Configuration;
        _commonAppService = CommonAppService;
        _contextAccessor = ContextAccessor;
        //_iRandomGenerator = RandomGenerator;
        _cacheManager = CacheManager;
        //_userRepository = UserRepository;
        _logsRepository = LogsRepository;
        _commitOrderRepository = CommitOrderRepository;
        //_unitOfWorkManager = UnitOfWorkManager;
        //  _configuration = Configuration;
        _saleDetailRepository = SaleDetailRepository;
        _userRejectionAdcocacyRepository = UserRejectionAdcocacyRepository;
        //_abpSession = abpSession;
        _baseInformationAppService = BaseInformationAppService;
        _bankAppService = BankAppService;
        _orderStatusTypeReadOnlyRepository = orderStatusTypeReadOnlyRepository;
        _orderRejectionTypeReadOnlyRepository = orderRejectionTypeReadOnlyRepository;
        // _ctx = dbContextProvider;
    }

    [Audited]
    //[UnitOfWork(isTransactional: false)]
    public async Task CommitOrder(CommitOrderDto commitOrderDto)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

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
        object SaleDetailFromCache = null;
        SaleDetailOrderDto SaleDetailDto = null;
        _cacheManager.GetCache("SaleDetail").TryGetValue(commitOrderDto.SaleDetailUId.ToString(), out SaleDetailFromCache);
        if (SaleDetailFromCache != null)
        {
            SaleDetailDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(SaleDetailFromCache as SaleDetail);
        }
        else
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
                    UID = x.UID
                })
                .FirstOrDefault(x => x.UID == commitOrderDto.SaleDetailUId);
            if (SaleDetailFromDb == null)
            {
                throw new UserFriendlyException("تاریخ برنامه فروش به پایان رسیده است.");
            }
            else
            {
                SaleDetailDto = SaleDetailFromDb;
                await _cacheManager.GetCache("SaleDetail").SetAsync(commitOrderDto.SaleDetailUId.ToString(), SaleDetailDto);
            }
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
        await _commonAppService.IsUserRejected(); //if user reject from advocacy
                                                  //_baseInformationAppService.CheckBlackList(SaleDetailDto.EsaleTypeId); //if user not exsist in blacklist
        _bankAppService.CheckAdvocacy(_commonAppService.GetNationalCode()); //if hesab vekalati darad
        _baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhiteListOrder);
        var orderQuery = await _commitOrderRepository.GetQueryableAsync();
        var userId = _commonAppService.GetUserId();
        var activeSuccessfulOrderExists = orderQuery
            .AsNoTracking()
            .Select(x => new { x.UserId, x.OrderStatus })
            .FirstOrDefault(
                y => y.UserId == userId &&
             y.OrderStatus == OrderStatusType.Winner
         );
        if (activeSuccessfulOrderExists != null)
            throw new UserFriendlyException("جهت ثبت سفارش جدید لطفا ابتدا از جزئیات سفارش، سفارش قبلی خود که موعد تحویل آن در سال 1403 می باشد را لغو نمایید .");

        // await _baseInformationAppService.CheckAdvocacyPrice(SaleDetailDto.MinimumAmountOfProxyDeposit);
        ///////////////////////////////////iran/////////////
        var ttl = SaleDetailDto.SalePlanEndDate.Subtract(DateTime.Now);
        ///
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            object objectCommitOrderIran = null;
            _cacheManager.GetCache("CommitOrderIran").
            TryGetValue(
                userId.ToString() + "_" +
                SaleDetailDto.SaleId.ToString()
                , out objectCommitOrderIran);
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
                    await _cacheManager.GetCache("CommitOrderIran").
                       SetAsync(
                           userId.ToString() + "_" +
                           SaleDetailDto.SaleId.ToString()
                           , customerOrderIranFromDb.Id
                           , TimeSpan.FromSeconds(ttl.TotalSeconds));
                    throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید از درخواست قبلی خود انصراف دهید");


                }
            }
        }
        ///////////////////////////////vardati chek dar bakhshnameh///////////////////////////
        else
        {
            object objectCommitOrderIran = null;
            _cacheManager.GetCache("CommitOrderimport").
            TryGetValue(
                userId.ToString() + "_" +
                commitOrderDto.PriorityId.ToString() + "_" +
                SaleDetailDto.SaleId.ToString()
                , out objectCommitOrderIran);
            if (objectCommitOrderIran != null)
            {
                throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید، درخواست قبلی خود را لغو نمایید یا اولویت دیگری را انتخاب نمایید");
            }
            else
            {
                CustomerOrder customerOrderIranFromDb =
                _commitOrderRepository.ToListAsync().Result
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
                    await _cacheManager.GetCache("CommitOrderimport").
                       SetAsync(
                          userId.ToString() + "_" +
                            commitOrderDto.PriorityId.ToString() + "_" +
                            SaleDetailDto.SaleId.ToString()
                           , customerOrderIranFromDb.Id
                           , TimeSpan.FromSeconds(ttl.TotalSeconds));
                    throw new UserFriendlyException("درخواست شما برای خودروی دیگری در حال بررسی می باشد. جهت سفارش جدید، درخواست قبلی خود را لغو نمایید یا اولویت دیگری را انتخاب نمایید");
                }

            }
            object objectCustomerOrderFromCache = null;
            _cacheManager.GetCache("CommitOrderimport")
                .TryGetValue(
                userId.ToString() + "_" +
                    SaleDetailDto.Id.ToString(),
                out objectCustomerOrderFromCache
                );
            if (objectCustomerOrderFromCache != null)
            {
                throw new UserFriendlyException("این خودرو را قبلا انتخاب نموده اید");
            }
            if (objectCustomerOrderFromCache == null)
            {
                var commitOrderQuery = _commitOrderRepository.WithDetails();

                var CustomerOrderFromDb = commitOrderQuery
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
                    await _cacheManager.GetCache("CommitOrderimport").
                   SetAsync(
                       userId.ToString() + "_" +
                       SaleDetailDto.Id.ToString()
                       , CustomerOrderFromDb.Id
                       , TimeSpan.FromSeconds(ttl.TotalSeconds));
                    throw new UserFriendlyException("این خودرو را قبلا انتخاب نموده اید.");

                }
            }



        }
        ////////////////////////






        CustomerOrder customerOrder = new CustomerOrder();
        customerOrder.SaleDetailId = (int)SaleDetailDto.Id;
        customerOrder.UserId = (int)userId;
        customerOrder.PriorityId = (PriorityEnum)commitOrderDto.PriorityId;
        customerOrder.OrderStatus = OrderStatusType.RecentlyAdded;
        customerOrder.SaleId = SaleDetailDto.SaleId;
        await _commitOrderRepository.InsertAsync(customerOrder, autoSave: true);
        //CurrentUnitOfWork.SaveChanges();
        //unitOfWork.Complete();
        //}
        await _cacheManager.GetCache("CommitOrder").
                   SetAsync(
               userId.ToString() + "_" +
               commitOrderDto.SaleDetailUId.ToString()
               , customerOrder.Id,
               TimeSpan.FromSeconds(ttl.TotalSeconds));
        //if iran
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            await _cacheManager.GetCache("CommitOrderIran").
              SetAsync(
                  userId.ToString() + "_" +
                  SaleDetailDto.SaleId.ToString()
                , customerOrder.Id
                , TimeSpan.FromSeconds(ttl.TotalSeconds)
              );
        }//vardat
        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            await _cacheManager.GetCache("CommitOrderimport").
                       SetAsync(
                          userId.ToString() + "_" +
                            commitOrderDto.PriorityId.ToString() + "_" +
                            SaleDetailDto.SaleId.ToString()
                           , customerOrder.Id,
                          TimeSpan.FromSeconds(ttl.TotalSeconds));
            await _cacheManager.GetCache("CommitOrderimport").
                 SetAsync(
                     userId.ToString() + "_" +
                     SaleDetailDto.Id.ToString()
                     , customerOrder.Id
                     , TimeSpan.FromSeconds(ttl.TotalSeconds));
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
    [UnitOfWork(false,IsolationLevel.ReadUncommitted)]
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
                y.ESaleTypeId
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
            if (x.OrderStatusCode == 10) // OrderStatusType.RecentlyAdded
                x.Cancelable = true;
            else if (x.OrderStatusCode == 40 && x.DeliveryDateDescription.Contains(cancleableDate, StringComparison.InvariantCultureIgnoreCase)) // OrderStatusType.Winner
                x.Cancelable = true;
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
        _baseInformationAppService.CheckWhiteList(WhiteListEnumType.WhiteListOrder);
        var customerOrder = _commitOrderRepository.WithDetails().FirstOrDefault(x => x.Id == orderId);
        if (customerOrder == null)
            throw new UserFriendlyException("شماره سفارش صحیح نمی باشد");

        if (!(customerOrder.OrderStatus == OrderStatusType.Winner || customerOrder.OrderStatus == OrderStatusType.RecentlyAdded))
            throw new UserFriendlyException("امکان انصراف وجود ندارد");

        if (customerOrder.UserId != userId)
            throw new UserFriendlyException("شماره سفارش صحیح نمی باشد");

        SaleDetailOrderDto saleDetailOrderDto;
        if (_cacheManager.GetCache("SaleDetail").TryGetValue(customerOrder.SaleDetailId.ToString(), out var saleDetailFromCache))
        {
            saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(saleDetailFromCache as SaleDetail);
        }
        else
        {
            var saleDetail = _saleDetailRepository.WithDetails().FirstOrDefault(x => x.Id == customerOrder.SaleDetailId)
                ?? throw new UserFriendlyException("جزئیات برنامه فروش یافت نشد");
            saleDetailOrderDto = ObjectMapper.Map<SaleDetail, SaleDetailOrderDto>(saleDetail);
            await _cacheManager.GetCache("SaleDetail").SetAsync(saleDetailOrderDto.Id.ToString(), saleDetailOrderDto);
        }
        // CheckSaleDetailValidation(saleDetailOrderDto);
        //var currentTime = DateTime.Now;
        //if (currentTime > saleDetailOrderDto.SalePlanEndDate)
        //    throw new UserFriendlyException("امکان انصراف برای سفارشاتی که برنامه فروش مرتبط ،منقضی شده باشد ممکن نیست");
        //if iran
        if (_configuration.GetSection("IsIranCellActive").Value == "7")
        {
            await _cacheManager.GetCache("CommitOrderIran").
              RemoveAsync(
                  userId.ToString() + "_" +
                  customerOrder.SaleId.ToString()
              );
        }//vardat
        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            await _cacheManager.GetCache("CommitOrderimport").
                       RemoveAsync(
                          userId.ToString() + "_" +
                            customerOrder.PriorityId.ToString() + "_" +
                            customerOrder.SaleId.ToString())
                           ;
            await _cacheManager.GetCache("CommitOrderimport").
                 RemoveAsync(
                     userId.ToString() + "_" +
                     customerOrder.SaleDetailId.ToString()
                     );
        }



        customerOrder.OrderStatus = OrderStatusType.Canceled;
        await CurrentUnitOfWork.SaveChangesAsync();
        return ObjectMapper.Map<CustomerOrder, CustomerOrderDto>(customerOrder, new CustomerOrderDto());
    }
    //[Audited]
    [AbpAllowAnonymous]
    //[Audited]
    public async Task<string> TestAdvocacy(string nc)
    {
        //UserRejectionAdvocacy ur;
        UserRejectionAdvocacy userRejectionAdvocacy = new UserRejectionAdvocacy();
        userRejectionAdvocacy.Archived = false;
        userRejectionAdvocacy.BatchId = 1;
        userRejectionAdvocacy.accountNumber = "12568";
        userRejectionAdvocacy.ShabaNumber = "sheba";
        userRejectionAdvocacy.datetime = DateTime.Now;
        userRejectionAdvocacy.NationalCode = nc;
        UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
        // var xx = await _userRejectionAdcocacyRepository.FirstOrDefaultAsync(x => x.NationalCode == nc);
        //   System.Threading.Thread.Sleep(5000);

        unitOfWorkOptions.IsTransactional = false;
        unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        unitOfWorkOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{
        var ur = _userRejectionAdcocacyRepository.WithDetails().FirstOrDefault(x => x.NationalCode == nc);
        await CurrentUnitOfWork.SaveChangesAsync();
        // System.Threading.Thread.Sleep(5000);
        //}
        //unitOfWorkOptions.IsTransactional = false;
        //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        //unitOfWorkOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{

        // await _userRejectionAdcocacyRepository.DeleteAsync(userRejectionAdvocacy);

        // await _userRejectionAdcocacyRepository.InsertAsync(userRejectionAdvocacy);
        // await CurrentUnitOfWork.SaveChangesAsync();
        //    unitOfWork.Complete();

        //}
        //var dbcontext = _ctx.GetDbContext();
        //  dbcontext.UserRejectionAdvocacies.Add(userRejectionAdvocacy);

        unitOfWorkOptions = new UnitOfWorkOptions();
        unitOfWorkOptions.IsTransactional = false;
        unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        unitOfWorkOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{
        await _userRejectionAdcocacyRepository.InsertAsync(userRejectionAdvocacy);
        await CurrentUnitOfWork.SaveChangesAsync();

        //}

        //  var ur = _userRejectionAdcocacyRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.NationalCode == nc);
        unitOfWorkOptions = new UnitOfWorkOptions();
        unitOfWorkOptions.IsTransactional = false;
        unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        unitOfWorkOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{
        await _userRejectionAdcocacyRepository.DeleteAsync(userRejectionAdvocacy);


        await CurrentUnitOfWork.SaveChangesAsync();

        //}




        return nc;

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
        //TODO: fill userMobile, usershaba, userAccountNumber with right data some how
        var (userMobile, userShaba, userAccountNumber) = (string.Empty, string.Empty, string.Empty);

        await _commonAppService.ValidateSMS(userMobile, userNationalCode, userSmsCode, SMSType.UserRejectionAdvocacy);
        //var saleDetail = await _saleDetailRepository.GetAll()
        //    .Select(x => new { x.SalePlanStartDate , x.SalePlanEndDate, x.SaleId})
        //    .FirstOrDefaultAsync(x => x.SalePlanStartDate <= DateTime.Now && x.SalePlanEndDate >= DateTime.Now);
        //if (saleDetail == null)
        //    throw new UserFriendlyException("هیچ برنامه فروش فعالی وجود ندارد");
        var saleId = 1;
        //var order = await _commitOrderRepository
        //    .GetAll()
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
        await _cacheManager.GetCache("UserRejection").RemoveAsync(userNationalCode);

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
        //var result = await _commitOrderRepository.GetAllIncluding(x => x.SaleDetail.CarTip.CarType.CarFamily.Company, x => x.User, x => x.User.BirthCity, x => x.User.HabitationCity, x => x.User.IssuingCity, x => x.User.BirthProvince, x => x.User.HabitationProvince, x => x.User.IssuingProvince)
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
        //var user = await _userRepository.GetAsync(userId);
        //if (!_commonAppService.IsInRole("Company"))
        //    throw new UserFriendlyException("دسترسی کاربر جاری برای دیدن سفارشات کافی نیست");
        //if (!user.CompanyId.HasValue)
        //    throw new UserFriendlyException("کاربرجاری شناسه فعال ندارد.");

        //var compayId = user.CompanyId.Value;
        //var result = await _commitOrderRepository.GetAllIncluding(x => x.SaleDetail.CarTip.CarType.CarFamily.Company, x => x.User, x => x.User.BirthCity, x => x.User.HabitationCity, x => x.User.IssuingCity, x => x.User.BirthProvince, x => x.User.HabitationProvince, x => x.User.IssuingProvince)
        //    .Select(x => new
        //    {

        //        x.User,
        //        x.OrderStatus,
        //        x.OrderRejectionStatus,
        //        CompanyId = x.SaleDetail.CarTip.CarType.CarFamily.Company.Id,
        //        PriorityUser = x.PriorityUser


        //    })
        //    .Where(x => x.OrderRejectionStatus != OrderRejectionType.PhoneNumberAndNationalCodeConflict && x.OrderStatus == OrderStatusType.Winner && x.CompanyId == compayId)
        //    .Select(x => new CustomerOrderPriorityUserDto()
        //    {
        //        PriorityUser = x.PriorityUser,
        //        CustomerInformation = ObjectMapper.Map<User, UserInfoPriorityDto>(x.User, new UserInfoPriorityDto()),

        //    }).ToListAsync();
        //return result;
    }


}
