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
        var orderDeliveries = (await _orderDeliveryStatusTypeRepository.GetQueryableAsync()).AsNoTracking().ToList();
        var userId = _commonAppService.GetUserId();
        var order = (await _customerOrderRepository.GetQueryableAsync())
            .Include(x => x.SaleDetail)
            .ThenInclude(x => x.Product)
            .FirstOrDefault(x => x.Id == orderStatusInquiryCommitDto.OrderId && x.UserId == userId)
            ?? throw new UserFriendlyException("سفارش یافت نشد");
        var currentOrderDeliveryStatus = order.OrderDeliveryStatus;
        var nationalCode = _commonAppService.GetNationalCode();
        nationalCode = "5580099126"; //جهت دمو - 498729
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
        var orderDeliverStatusList = availableDeliveryStatusList.Select(x => x.OrderDeliverySatusCode).ToList();
        var result = new OrderStatusInquiryResultDto();
        var userRejectionAdvocacy = (await _userRejectionAdvocacyRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.SaleId,
                x.NationalCode,
                x.CreationTime
            })
            .FirstOrDefault(x => x.SaleId == order.SaleId && x.NationalCode == nationalCode);
        if (userRejectionAdvocacy != null)
        {
            result.UserRejectionAdvocacyDate = userRejectionAdvocacy.CreationTime;
        }
        result.AvailableDeliveryStatusList = availableDeliveryStatusList;
        var product = (await _productAndCategoryRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.Code,
                x.Id
            })
            .FirstOrDefault(x => x.Id == order.SaleDetail.ProductId)
            ?? throw new UserFriendlyException("محصول مرتبط یافت نشد");
        var company = (await _productAndCategoryRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.Code,
                x.Id,
                x.Title
            })
            .FirstOrDefault(x => x.Code == product.Code.Substring(0, 4));
        var orderLastLog = (await _orderStatusInquiryRepository.GetQueryableAsync())
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x => x.OrderId == orderStatusInquiryCommitDto.OrderId);
        if (orderLastLog != null)
        {
            var lastLogSubmitDate = orderLastLog.SubmitDate;
            var dateDiff = lastLogSubmitDate.Subtract(DateTime.Now);
            if (dateDiff.Days <= 0)
            {
                result = ObjectMapper.Map<OrderStatusInquiry, OrderStatusInquiryResultDto>(orderLastLog, result);
                //result.OrderDeliveryStatusDescription = (await _orderDeliveryStatusTypeRepository.GetQueryableAsync())
                //    .FirstOrDefault(x => x.Code == (int)result.OrderDeliveryStatus).Description;
                result.AvailableDeliveryStatusList = availableDeliveryStatusList;
                FormatOrderDeliveryStatusDescriptions(result.AvailableDeliveryStatusList, order, orderLastLog, company.Title);
                result.OrderDeliveryStatusDescription = availableDeliveryStatusList.First(x => x.OrderDeliverySatusCode == (int)result.OrderDeliveryStatus).Description;
                return result;
            }
        }
        var companyAccessToken = await _commonAppService.GetIkcoAccessTokenAsync();
        var inquiryFromCompany = await _commonAppService.IkcoOrderStatusInquiryAsync(nationalCode, orderStatusInquiryCommitDto.OrderId, companyAccessToken);
        if (!inquiryFromCompany.Succeeded)
            _auditingManager.Current.Log.Exceptions.Add(new Exception(inquiryFromCompany.Errors));
        var rowContractId = 0;
        decimal? fullAmountPaid = null;
        if (inquiryFromCompany?.Data.Length >= 1)
        {
            var inquiryData = inquiryFromCompany.Data[0];
            rowContractId = inquiryData.ContRowId;
            fullAmountPaid = inquiryData.FinalPrice;
            if (inquiryData.ContRowId > 0)
            {
                orderDeliverStatusList.Add((int)OrderDeliveryStatusType.ReceivingContractRowId);
                var contRowIdItem = availableDeliveryStatusList.FirstOrDefault(x => x.OrderDeliverySatusCode == (int)OrderDeliveryStatusType.ReceivingContractRowId);
                contRowIdItem.SubmitDate = inquiryData.TranDate.ToDateTime();
                currentOrderDeliveryStatus = OrderDeliveryStatusType.ReceivingContractRowId;
            }
            if (inquiryData.FinalPrice > 0)
            {
                orderDeliverStatusList.Add((int)OrderDeliveryStatusType.ReceivingAmountCompleted);
                var recievingAmountItem = availableDeliveryStatusList.FirstOrDefault(x => x.OrderDeliverySatusCode == (int)OrderDeliveryStatusType.ReceivingAmountCompleted);
                recievingAmountItem.SubmitDate = inquiryData.TranDate.ToDateTime();
                currentOrderDeliveryStatus = OrderDeliveryStatusType.ReceivingAmountCompleted;
            }
        }
        //else
        //{
        //    var ordersDeliveryCurrentStatus = order.OrderDeliveryStatus.HasValue
        //        ? order.OrderDeliveryStatus.Value
        //        : OrderDeliveryStatusType.OrderRegistered;
        //    orderDeliverStatusList.Add((int)ordersDeliveryCurrentStatus);
        //}
        var serializedInquiryResponse = JsonConvert.SerializeObject(inquiryFromCompany);
        var orderLog = await _orderStatusInquiryRepository.InsertAsync(new OrderStatusInquiry()
        {
            ClientNationalCode = nationalCode,
            CompanyId = company.Id,
            InquiryFullResponse = serializedInquiryResponse,
            OrderDeliveryStatus = currentOrderDeliveryStatus,
            SubmitDate = DateTime.Now,
            OrderId = order.Id,
            RowContractId = rowContractId,
            FullAmountPaid = fullAmountPaid
        });
        order.OrderDeliveryStatus = currentOrderDeliveryStatus;
        await _customerOrderRepository.AttachAsync(order, x => x.OrderDeliveryStatus);
        result = ObjectMapper.Map<OrderStatusInquiry, OrderStatusInquiryResultDto>(orderLog, result);
        var orderDeliveryStatusDescription = (await _orderDeliveryStatusTypeRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Code == (int)result.OrderDeliveryStatus);
        //result.OrderDeliveryStatusDescription = orderDeliveryStatusDescription.Description;
        FormatOrderDeliveryStatusDescriptions(result.AvailableDeliveryStatusList, order, orderLog, company.Title);
        result.OrderDeliveryStatusDescription = availableDeliveryStatusList.First(x => x.OrderDeliverySatusCode == (int)currentOrderDeliveryStatus).Description;
        return result;
    }

    private void FormatOrderDeliveryStatusDescriptions(List<OrderDeliveryStatusViewModel> orderDeliveryStatusViewModels,
        CustomerOrder order,
        OrderStatusInquiry orderInquiry,
        string companyName)
    {
        orderDeliveryStatusViewModels.ForEach(x =>
        {
            x.Description = x.OrderDeliverySatusCode switch
            {
                (int)OrderDeliveryStatusType.OrderRegistered => string.Format(x.Description, order.Id),
                (int)OrderDeliveryStatusType.Prioritization => string.Empty,
                (int)OrderDeliveryStatusType.ProductDetermination => string.Format(x.Description, order.SaleDetail?.Product?.Title ?? string.Empty),
                (int)OrderDeliveryStatusType.SendingToManufaturer => string.Format(x.Description, companyName),
                (int)OrderDeliveryStatusType.ReceivingContractRowId => orderInquiry.RowContractId != 0
                    ? string.Format(x.Description, orderInquiry.RowContractId)
                    : string.Empty,
                (int)OrderDeliveryStatusType.ReceivingAmountCompleted => orderInquiry.FullAmountPaid.HasValue && orderInquiry.FullAmountPaid.Value > 0
                    ? string.Format(x.Description, orderInquiry.FullAmountPaid.Value)
                    : string.Empty,
                _ => string.Empty
            };
        });
    }
}
