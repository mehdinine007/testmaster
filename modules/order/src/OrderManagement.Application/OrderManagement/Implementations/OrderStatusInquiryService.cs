using Esale.Core.DataAccess;
using Esale.Core.Utility.Tools;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.Bases;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrderStatusInquiryService : ApplicationService, IOrderStatusInquiryService
{
    private readonly IRepository<OrderStatusInquiry, long> _orderStatusInquiryRepository;
    private readonly IRepository<CustomerOrder, int> _customerOrderRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IAuditingManager _auditingManager;
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private readonly IReadOnlyRepository<OrderDeliveryStatusTypeReadOnly, int> _orderDeliveryStatusTypeRepository;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdvocacyRepository;

    public OrderStatusInquiryService(IRepository<OrderStatusInquiry, long> orderStatusInquiryRepository,
                                     IRepository<CustomerOrder, int> customerOrderRepository,
                                     ICommonAppService commonAppService,
                                     IAuditingManager auditingManager,
                                     IRepository<ProductAndCategory, int> productAndCategoryRepository,
                                     IReadOnlyRepository<OrderDeliveryStatusTypeReadOnly, int> orderDeliveryStatusTypeRepository,
                                     IRepository<UserRejectionAdvocacy, int> userRejectionAdvocacyRepository
        )
    {
        _orderStatusInquiryRepository = orderStatusInquiryRepository;
        _commonAppService = commonAppService;
        _customerOrderRepository = customerOrderRepository;
        _auditingManager = auditingManager;
        _productAndCategoryRepository = productAndCategoryRepository;
        _orderDeliveryStatusTypeRepository = orderDeliveryStatusTypeRepository;
        _userRejectionAdvocacyRepository = userRejectionAdvocacyRepository;
    }

    public async Task<OrderStatusInquiryDto> GetCurrentUserOrderStatus(string nationalCode, int customerOrderId)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderStatusInquiryResultDto> CommitOrderDeilveryLog(OrderStatusInquiryCommitDto orderStatusInquiryCommitDto)
    {
        var orderDeliverStatusList = new List<int>();
        var orderDeliveries = (await _orderDeliveryStatusTypeRepository.GetQueryableAsync()).AsNoTracking().ToList();
        var userId = _commonAppService.GetUserId();
        var order = (await _customerOrderRepository.GetQueryableAsync())
            .Include(x => x.SaleDetail)
            .FirstOrDefault(x => x.Id == orderStatusInquiryCommitDto.OrderId && x.UserId == userId)
            ?? throw new UserFriendlyException("سفارش یافت نشد");
        var availableDeliveryStatusList = orderDeliveries.OrderBy(x => x.Code).Select(x =>
        {
            OrderDeliveryStatusViewModel statusModel = new()
            {
                Description = x.Description,
                OrderDeliverySatusCode = x.Code,
                Title = x.Title
            };
            statusModel.SubmitDate = x.Code switch
            {
                1 => order.CreationTime,
                2 => order.PrioritizationDate,
                3 => order.VehicleSelectDate,
                4 => order.SendToManufacturerDate,
                _ => null
            };
            return statusModel;
        }).ToList();
        var product = (await _productAndCategoryRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.Code,
                x.Id
            })
            .FirstOrDefault(x => x.Id == order.SaleDetail.ProductId)
            ?? throw new UserFriendlyException("محصول مرتبط یافت نشد");
        var orderLastLog = (await _orderStatusInquiryRepository.GetQueryableAsync())
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x => x.OrderId == orderStatusInquiryCommitDto.OrderId);
        if (orderLastLog != null)
        {
            var lastLogSubmitDate = orderLastLog.SubmitDate;
            var dateDiff = lastLogSubmitDate.Subtract(DateTime.Now);
            if (dateDiff.Days <= 0)
            {
                var oldResult = ObjectMapper.Map<OrderStatusInquiry, OrderStatusInquiryResultDto>(orderLastLog);
                oldResult.OrderDeliveryStatusDescription = (await _orderDeliveryStatusTypeRepository.GetQueryableAsync())
                    .FirstOrDefault(x => x.Code == (int)oldResult.OrderDeliveryStatus).Description;
                oldResult.AvailableDeliveryStatusList = availableDeliveryStatusList;
                return oldResult;
            }
        }
        var nationalCode = _commonAppService.GetNationalCode();
        var companyAccessToken = await _commonAppService.GetIkcoAccessTokenAsync();
        var inquiryFromCompany = await _commonAppService.IkcoOrderStatusInquiryAsync(nationalCode, orderStatusInquiryCommitDto.OrderId, companyAccessToken);
        if (!inquiryFromCompany.Succeeded)
            _auditingManager.Current.Log.Exceptions.Add(new Exception(inquiryFromCompany.Errors));
        var company = (await _productAndCategoryRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.Code,
                x.Id
            })
            .FirstOrDefault(x => x.Code == product.Code.Substring(0, 4));

        if (inquiryFromCompany?.Data.Length >= 1)
        {
            var inquiryData = inquiryFromCompany.Data[0];
            if (inquiryData.ContRowId > 0)
            {
                orderDeliverStatusList.Add((int)OrderDeliveryStatusType.ReceivingContractRowId);
                var contRowIdItem = availableDeliveryStatusList.FirstOrDefault(x => x.OrderDeliverySatusCode == (int)OrderDeliveryStatusType.ReceivingContractRowId);
                contRowIdItem.SubmitDate = inquiryData.TranDate.ToDateTime();
            }
            if (inquiryData.FinalPrice > 0)
            {
                orderDeliverStatusList.Add((int)OrderDeliveryStatusType.ReceivingAmountCompleted);
                var recievingAmountItem = availableDeliveryStatusList.FirstOrDefault(x => x.OrderDeliverySatusCode == (int)OrderDeliveryStatusType.ReceivingAmountCompleted);
                recievingAmountItem.SubmitDate = inquiryData.TranDate.ToDateTime();
            }
        }
        else
        {
            var ordersDeliveryCurrentStatus = order.OrderDeliveryStatus.HasValue
                ? order.OrderDeliveryStatus.Value
                : OrderDeliveryStatusType.OrderRegistered;
            orderDeliverStatusList.Add((int)ordersDeliveryCurrentStatus);
        }
        var serializedInquiryResponse = JsonConvert.SerializeObject(inquiryFromCompany);
        var orderLog = await _orderStatusInquiryRepository.InsertAsync(new OrderStatusInquiry()
        {
            ClientNationalCode = nationalCode,
            CompanyId = company.Id,
            InquiryFullResponse = serializedInquiryResponse,
            OrderDeliveryStatus = (OrderDeliveryStatusType)orderDeliverStatusList.Max(),
            SubmitDate = DateTime.Now,
            OrderId = order.Id,
        });
        order.OrderDeliveryStatusDesc = serializedInquiryResponse;
        await _customerOrderRepository.AttachAsync(order, x => x.OrderDeliveryStatusDesc, x => x.OrderDeliveryStatus);
        var result = ObjectMapper.Map<OrderStatusInquiry, OrderStatusInquiryResultDto>(orderLog);
        var orderDeliveryStatusDescription = (await _orderDeliveryStatusTypeRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Code == (int)result.OrderDeliveryStatus);
        result.OrderDeliveryStatusDescription = orderDeliveryStatusDescription.Description;
        return result;
    }
}