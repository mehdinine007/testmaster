using Microsoft.AspNetCore.Http;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class BaseInformationService : ApplicationService, IBaseInformationService
{

    private readonly IRepository<Company, int> _companyRepository;
    //private readonly IRepository<User, long> _userRepository;
    private readonly IRepository<CarTip, int> _carTipRepository;
    private readonly IRepository<Gallery, int> _galleryRepository;
    private readonly IRepository<CarMakerBlackList, long> _carMakerBlackListRepository;
    private readonly IRepository<AdvocacyUsersFromBank, int> _advocacyUsersFromBankRepository;
    private readonly IRepository<Province, int> _provinceRepository;
    private readonly IRepository<City, int> _cityRepository;
    private readonly IRepository<WhiteList, int> _whiteListRepository;
    private readonly IRepository<AdvocacyUser, int> _advocacyUsersRepository;
    //private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IEsaleGrpcClient _esaleGrpcClient;

    private Microsoft.Extensions.Configuration.IConfiguration _configuration { get; set; }
    private IHttpContextAccessor _httpContextAccessor;

    private readonly ICommonAppService _commonAppService;

    //    public BaseInformationService(IEsaleGrpcClient esaleGrpcClient,
    //                                  IRepository<Company, int> companyRepository
    //                                  , IRepository<CarTip, int> carTipRepository
    //,
    //IRepository<Gallery, int> galleryRepository = null)
    //    {
    //        _esaleGrpcClient = esaleGrpcClient;
    //        _companyRepository = companyRepository;
    //        _carTipRepository = carTipRepository;
    //        _galleryRepository = galleryRepository;
    //    }

    public BaseInformationService(IRepository<Company, int> companyRepository,
                                  IRepository<CarTip, int> carTipRepsoitory,
                                  IRepository<Gallery, int> galleryRepository,
                                  ICommonAppService CommonAppService,
                                  IHttpContextAccessor HttpContextAccessor,
                                  IRepository<CarMakerBlackList, long> CarMakerBlackListRepository,
                                  IRepository<Province, int> ProvinceRepository,
                                  IRepository<WhiteList, int> WhiteListRepository,
                                  IRepository<AdvocacyUser, int> AdvocacyUsersRepository,
                                  //IPasswordHasher<User> PasswordHasher,
                                  Microsoft.Extensions.Configuration.IConfiguration Configuration,
                                  IRepository<City, int> CityRepository,
                                  IRepository<AdvocacyUsersFromBank, int> AdvocacyUsersFromBankRepository,
                                  IEsaleGrpcClient esaleGrpcClient
        )
    {
        _esaleGrpcClient = esaleGrpcClient;
        _companyRepository = companyRepository;
        _carTipRepository = carTipRepsoitory;
        _galleryRepository = galleryRepository;
        _advocacyUsersFromBankRepository = AdvocacyUsersFromBankRepository;
        _carMakerBlackListRepository = CarMakerBlackListRepository;
        _commonAppService = CommonAppService;
        _provinceRepository = ProvinceRepository;
        _cityRepository = CityRepository;
        _httpContextAccessor = HttpContextAccessor;
        _whiteListRepository = WhiteListRepository;
        _configuration = Configuration;
        _advocacyUsersRepository = AdvocacyUsersRepository;
        //_passwordHasher = PasswordHasher;
    }

    [RemoteService(false)]
    public async Task CheckAdvocacyPrice(decimal MinimumAmountOfProxyDeposit)
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        string Nationalcode = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                           .Select(c => c.Value).SingleOrDefault();
        if (Nationalcode == null)
        {
            throw new UserFriendlyException("کد ملی صحیح نمی باشد");
        }

        //UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
        //unitOfWorkOptions.IsTransactional = false;
        //unitOfWorkOptions.IsolationLevel = System.Transactions.IsolationLevel.Snapshot;
        //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{

        var advocacyuser = _advocacyUsersFromBankRepository
            .WithDetails()
            .Select(x => new { x.price, x.nationalcode })
            .FirstOrDefault(x => x.nationalcode == Nationalcode);
        if (advocacyuser == null)
        {
            await UnitOfWorkManager.Current.CompleteAsync();
            throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
        }
        else
        {
            if (advocacyuser.price < MinimumAmountOfProxyDeposit)
            {
                await UnitOfWorkManager.Current.CompleteAsync();
                throw new UserFriendlyException("موجودی حساب وکالتی برای خودروی انتخابی کافی نمی باشد");
            }
        }
        await UnitOfWorkManager.Current.CompleteAsync();
        //}



    }
    [RemoteService(false)]
    public void CheckWhiteList(WhiteListEnumType whiteListEnumType, string Nationalcode = "")
    {
        if (_configuration.GetSection(whiteListEnumType.ToString()).Value == "1")
        {
            if (string.IsNullOrEmpty(Nationalcode))
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                // Get the claims values
                string nc = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();
                if (Nationalcode == null)
                {

                    throw new UserFriendlyException("کد ملی صحیح نمی باشد");
                }
                Nationalcode = nc;
            }

            //UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
            //unitOfWorkOptions.IsTransactional = false;
            //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
            //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
            //{
            var WhiteList = _whiteListRepository
           .WithDetails()
           .Select(x => new { x.NationalCode, x.WhiteListType })
           .FirstOrDefault(x => x.NationalCode == Nationalcode
           && x.WhiteListType == whiteListEnumType);
            if (WhiteList == null)
            {
                //unitOfWork.Complete();
                throw new UserFriendlyException(_configuration.GetSection(whiteListEnumType.ToString() + "Message").Value);
            }
            //    unitOfWork.Complete();
            //}

        }

    }

    //[UnitOfWork(System.Transactions.IsolationLevel.Unspecified)]
    public void CheckBlackList(int esaleTypeId)
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        string Nationalcode = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                           .Select(c => c.Value).SingleOrDefault();

        if (Nationalcode == null)
        {
            throw new UserFriendlyException("کد ملی صحیح نمی باشد");
        }
        var blackList = _carMakerBlackListRepository
            .WithDetails()
            .Select(x => new { x.Nationalcode, x.EsaleTypeId })
            .FirstOrDefault(x =>
                x.Nationalcode == Nationalcode
                && x.EsaleTypeId == esaleTypeId);
        if (blackList != null)
        {
            throw new UserFriendlyException("شما در گذشته از خودروسازان خرید داشته اید و امکان سفارش مجدد ندارید");
        }

    }

    public async Task RegistrationValidation(RegistrationValidationDto input)
    {
        throw new System.Exception();
        //await _commonAppService.ValidateVisualizeCaptcha(new CommonService.Dto.VisualCaptchaInput(input.CK, input.CIT));

        //// await _commonAppService.ValidateVisualizeCaptcha(new CommonService.Dto.VisualCaptchaInput(input.CT,input.CK, input.CIT));


        //var advocacyuser = _advocacyUsersRepository.WithDetails()
        //    .Select(x => new
        //    {
        //        x.shabaNumber,
        //        x.accountNumber,
        //        x.Id,
        //        x.nationalcode,
        //        x.BanksId
        //    })
        //    .OrderByDescending(x => x.Id).FirstOrDefault(x => x.nationalcode == input.Nationalcode);
        //if (advocacyuser == null)
        //{
        //    throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
        //}


        //var user = _userRepository.GetAll()
        // .Select(x => x.NationalCode)
        // .FirstOrDefault(x => x == input.Nationalcode);
        //if (user != null)
        //{

        //    throw new UserFriendlyException("این کد ملی قبلا ثبت نام شده است");
        //}


    }
    public void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input)
    {


        var advocacyuser = _advocacyUsersRepository.WithDetails()
            .Select(x => new
            {
                x.shabaNumber,
                x.accountNumber,
                x.Id,
                x.nationalcode,
                x.BanksId
            })
            .OrderByDescending(x => x.Id).FirstOrDefault(x => x.nationalcode == input.Nationalcode);

        if (advocacyuser == null)
        {
            throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
        }

    }

    public async Task<List<CarTipDto>> GetCarTipsByCompanyId(int companyId)
    {
        var carTipQuery = _carTipRepository.WithDetails(x => x.CarType.CarFamily.Company,
            x => x.CarTip_Gallery_Mappings);
        var carTips = carTipQuery.Where(x => x.CarType.CarFamily.Company.Id == companyId)
            .ToList();
        var carTipGalleryImageRelations = new Dictionary<int, List<int>>();//cartipId //galleryRecordIds
        var allRelateGalleryImageIds = new List<int>();
        carTips.ForEach(x =>
        {
            if (!carTipGalleryImageRelations.TryGetValue(x.Id, out var _))
            {
                var galleryIds = x.CarTip_Gallery_Mappings.Select(y => y.GalleryId).ToList();
                carTipGalleryImageRelations.Add(x.Id, galleryIds);
                allRelateGalleryImageIds.AddRange(galleryIds);
            }
        });
        var allReltaedGAlleryImages = _galleryRepository.WithDetails().Where(x => allRelateGalleryImageIds.Any(y => y == x.Id));
        var carTipDtos = ObjectMapper.Map<List<CarTip>, List<CarTipDto>>(carTips, new List<CarTipDto>());
        carTipDtos.ForEach(x =>
        {
            if (carTipGalleryImageRelations.TryGetValue(x.Id, out List<int> relatedImageIds))
            {
                x.CarImageUrls = allReltaedGAlleryImages.Where(y => relatedImageIds.Any(z => z == y.Id)).Select(y => y.ImageUrl).ToList();
            }
        });
        return carTipDtos;
    }

    public List<CompanyDto> GetCompanies()
    {
        var companiesQuery = _companyRepository.WithDetails(x => x.GalleryLogo, x => x.GalleryBanner, x => x.GalleryLogoInPage);
        var companies = companiesQuery.Where(x => x.Visible).ToList();
        return ObjectMapper.Map<List<Company>, List<CompanyDto>>(companies, new List<CompanyDto>());
    }
    public List<PublicDto> GetProvince()
    {
        return ObjectMapper.Map<List<Province>, List<PublicDto>>(_provinceRepository.WithDetails().ToList());

    }
    public List<PublicDto> GetCities(int ProvienceId)
    {
        return ObjectMapper.Map<List<City>, List<PublicDto>>(_cityRepository.WithDetails().Where(y => y.ProvinceId == ProvienceId).ToList());

    }

    public async Task<UserDto> GrpcTest()
    {
        var dd = await _esaleGrpcClient.GetUserAdvocacyByNationalCode(_commonAppService.GetNationalCode());
        return await _esaleGrpcClient.GetUserById(_commonAppService.GetUserId());
    }
}
