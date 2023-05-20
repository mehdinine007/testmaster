using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Volo.Abp.Domain.Entities;
using OrderManagement.Application.OrderManagement.Utitlities;
using System.Runtime.CompilerServices;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class SaleService : ApplicationService , ISaleService
{
    private readonly IRepository<PreSale> _preSaleRepository;
    private readonly IRepository<SaleDetail, int> _saleDetailRepository;
    private readonly IRepository<Gallery, int> _galleriesRepository;
    private readonly IRepository<Company, int> _companyRepository;
    private readonly IRepository<CarTip, int> _carTipRepository;
    private readonly IRepository<CarType, int> _carTypeRepository;
    private readonly IRepository<CarFamily, int> _carFamilyRepository;
    private readonly IRepository<CarTip_Gallery_Mapping, int> _carTip_GalleryMappingRepository;
    private readonly IRepository<ESaleType, int> _esaleTypeRepository;
    private readonly IRepository<CustomerOrder, int> _customerOrderRepository;
    //private readonly IRepository<User, long> _userRepository;
    private IConfiguration _configuration { get; set; }
    //public SaleService(IRepository<ESaleType, int> esaleTypeRepository,
    //                   IRepository<SaleDetail, int> saleDetailRepository,
    //                   IRepository<PreSale> preSaleRepository
    //    )
    //{
    //    _esaleTypeRepository = esaleTypeRepository;
    //    _saleDetailRepository = saleDetailRepository;
    //    _preSaleRepository = preSaleRepository;
    //}

    public SaleService(IRepository<PreSale> PreSaleRepository,
                       IRepository<SaleDetail, int> saleDetailRepository,
                       IRepository<Gallery, int> galleryRepository,
                       IRepository<Company, int> companyRepository,
                       IRepository<CarTip, int> carTipRepository,
                       IRepository<CarType, int> carTypeRepository,
                       IRepository<CarFamily, int> carFamilyRepository,
                       IRepository<CarTip_Gallery_Mapping, int> carTip_GalleryMappingRepository,
                       IRepository<ESaleType, int> esaleTypeRepository,
                       IRepository<CustomerOrder, int> CustomerOrderRepository,
                       IConfiguration configuration
        )
    {
        _preSaleRepository = PreSaleRepository;
        _saleDetailRepository = saleDetailRepository;
        _galleriesRepository = galleryRepository;
        _companyRepository = companyRepository;
        _carFamilyRepository = carFamilyRepository;
        _carTipRepository = carTipRepository;
        _carTypeRepository = carTypeRepository;
        _carTip_GalleryMappingRepository = carTip_GalleryMappingRepository;
        _esaleTypeRepository = esaleTypeRepository;
        _customerOrderRepository = CustomerOrderRepository;
        _configuration = configuration;
    }

    public async Task<List<PreSaleDto>> GetPreSales()
    {
        return ObjectMapper.Map<List<PreSale>,List<PreSaleDto>>
            (
                _preSaleRepository.WithDetails().ToList()
            );
    }

    public async Task<SaleDetailDto> GetSaleDetail(Guid uid)
    {
        var saleDetailQuery = _saleDetailRepository.WithDetails(x => x.CarTip,
                x => x.CarTip.CarType,
                x => x.CarTip.CarType.CarFamily,
                x => x.CarTip.CarType.CarFamily.Company,
                x => x.CarTip.CarTip_Gallery_Mappings,
                x => x.CarTip.CarType.CarFamily.Company.GalleryBanner,
                x => x.CarTip.CarType.CarFamily.Company.GalleryLogo,
                x => x.CarTip.CarType.CarFamily.Company.GalleryLogoInPage).OrderBy(x => x.ESaleTypeId);

        var saleDetail = saleDetailQuery.SingleOrDefault(x => x.UID == uid)
            ?? throw new EntityNotFoundException(typeof(SaleDetail), uid);

        var company = await _galleriesRepository.GetAsync(saleDetail.CarTip.CarType.CarFamily.Company.Id);
        var galleryIds = saleDetail.CarTip.CarTip_Gallery_Mappings.Select(x => x.GalleryId).ToList();
        var carGalleries = _galleriesRepository.WithDetails(x => galleryIds.Any(y => y == x.Id));
        var saleDetailDto = ObjectMapper.Map<SaleDetail, SaleDetailDto>(saleDetail, new SaleDetailDto());
        saleDetailDto.CarTipImageUrls = carGalleries.Select(x => x.ImageUrl).ToList();
        saleDetailDto.CompanyImageUrl = company.ImageUrl;
        return saleDetailDto;
    }

    public async Task<List<SaleDetailDto>> GetSaleDetails(int tipId, int typeId, int familyId, int companyId, int esaleTypeId)
    {
        var saleDetailQuery = _saleDetailRepository.WithDetails(x => x.CarTip,
                x => x.CarTip.CarType,
                x => x.CarTip.CarType.CarFamily,
                x => x.CarTip.CarType.CarFamily.Company,
                x => x.CarTip.CarTip_Gallery_Mappings,
                x => x.CarTip.CarType.CarFamily.Company.GalleryBanner,
                x => x.CarTip.CarType.CarFamily.Company.GalleryLogo,
                x => x.CarTip.CarType.CarFamily.Company.GalleryLogoInPage);
        var currentTime = DateTime.Now;
        saleDetailQuery = saleDetailQuery.Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible);
        if (esaleTypeId > 0)
            saleDetailQuery = saleDetailQuery.Where(x => x.ESaleTypeId == esaleTypeId);
        if (tipId > 0)
            saleDetailQuery = saleDetailQuery.Where(x => x.CarTip.Id == tipId);
        if (typeId > 0)
            saleDetailQuery = saleDetailQuery.Where(x => x.CarTip.CarType.Id == typeId);
        if (familyId > 0)
            saleDetailQuery = saleDetailQuery.Where(x => x.CarTip.CarType.CarFamily.Id == familyId);
        if (companyId > 0)
            saleDetailQuery = saleDetailQuery.Where(x => x.CarTip.CarType.CarFamily.Company.Id == companyId);
        var queryResult = saleDetailQuery.ToList();
        if (!queryResult.Any())
            return new List<SaleDetailDto>();
        var carTipImageBaseUrls = _configuration.GetSection("CarTipImageBaseUrls").Value;

        var saleDetailDtos = await queryResult.MapSaleDetailsToDto(_galleriesRepository, ObjectMapper, carTipImageBaseUrls);
        return saleDetailDtos.OrderBy(x => x.SalePlanDescription).ToList();
    }

    public async Task<List<SalePlanDto>> GetSalePlans(int companyId)
    {
        throw new NotImplementedException();
        //var salePlanQuery = _salePlanRepository.GetAllIncluding(x => x.CarTip.CarType.CarFamily.Company,
        //    x => x.CarTip.CarTip_Gallery_Mappings,
        //    x => x.ESaleType);

        //if (companyId > 0)
        //    salePlanQuery = salePlanQuery.Where(x => x.CarTip.CarType.CarFamily.Company.Id == companyId);
        //var queryResult = await salePlanQuery.ToListAsync();

        //var carTipGalleryImageRelations = new Dictionary<int, List<int>>();//cartipId //galleryRecordIds
        //var allRelateGalleryImageIds = new List<int>();
        //queryResult.ForEach(x =>
        //{
        //    if (!carTipGalleryImageRelations.TryGetValue(x.CarTipId, out var _))
        //    {
        //        var galleryIds = x.CarTip.CarTip_Gallery_Mappings.Select(y => y.GalleryId).ToList();
        //        carTipGalleryImageRelations.Add(x.CarTipId, galleryIds);
        //        allRelateGalleryImageIds.AddRange(galleryIds);
        //    }
        //});
        //var allReltaedGAlleryImages = await _galleriesRepository.GetAllListAsync(x => allRelateGalleryImageIds.Any(y => y == x.Id));
        //var salePlanDtos = ObjectMapper.Map<List<SalePlan>, List<SalePlanDto>>(queryResult, new List<SalePlanDto>());
        //salePlanDtos.ForEach(x =>
        //{
        //    if (carTipGalleryImageRelations.TryGetValue(x.CarTipId, out List<int> relatedImageIds))
        //    {
        //        x.CarTipImageUrls = allReltaedGAlleryImages.Where(y => relatedImageIds.Any(z => z == y.Id)).Select(y => y.ImageUrl).ToList();
        //    }
        //});
        //return salePlanDtos;
    }

    public async Task<List<ESaleTypeDto>> GetSaleTypes()
    {
        var esaleTypes = _esaleTypeRepository.WithDetails().ToList();
        return ObjectMapper.Map<List<ESaleType>, List<ESaleTypeDto>>(esaleTypes);
    }

    public async Task UserValidationByBirthDate(int saleId)
    {
        throw new NotImplementedException();
        //var users = await _customerOrderRepository.Query(
        //x => x.Join(_userRepository.GetAllIncluding(),
        //x => x.UserId,
        //x => x.Id, (x, y) => new { nationalCode = y.NationalCode, birthDate = y.BirthDate, saleId = x.SaleId, saleDetailId = x.SaleDetailId, orderStatus = x.OrderStatus }
        //).Where(x => x.saleId == saleId && x.orderStatus != OrderStatusType.Canceled).Select(x => new UserOrderDto
        //{
        //    NationalCode = x.nationalCode,
        //    BirthDate = x.birthDate,
        //    SaleDetailId = x.saleDetailId,
        //    SaleId = x.saleId,
        //}).ToListAsync());

    }

    public async Task UserValidationByMobile(int saleId)
    {
        throw new NotImplementedException();

       // var users = await _customerOrderRepository.Query(
       //x => x.Join(_userRepository.GetAllIncluding(),
       //x => x.UserId,
       //x => x.Id, (x, y) => new { nationalCode = y.NationalCode, mobile = y.Mobile, saleDetailId = x.SaleDetailId, saleId = x.SaleId, orderStatus = x.OrderStatus }
       //).Where(x => x.saleId == saleId && x.orderStatus != OrderStatusType.Canceled).Select(x => new UserOrderMobileDto
       //{
       //    NationalCode = x.nationalCode,
       //    Mobile = x.mobile,
       //    SaleDetailId = x.saleDetailId,
       //    SaleId = x.saleId,
       //}).ToListAsync());

    }
}
