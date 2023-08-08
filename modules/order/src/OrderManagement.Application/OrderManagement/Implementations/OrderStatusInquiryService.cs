using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrderStatusInquiryService : ApplicationService, IOrderStatusInquiryService
{
    private readonly IRepository<OrderStatusInquiry, long> _orderStatusInquiryRepository;

    public OrderStatusInquiryService(IRepository<OrderStatusInquiry, long> orderStatusInquiryRepository)
    {
        _orderStatusInquiryRepository = orderStatusInquiryRepository;
    }

    public async Task<OrderStatusInquiryDto> Insert(OrderStatusInquiryDto orderStatusInquiryDto)
    {
        var entity = await _orderStatusInquiryRepository.InsertAsync(
                    ObjectMapper.Map<OrderStatusInquiryDto, OrderStatusInquiry>(orderStatusInquiryDto));

        return ObjectMapper.Map<OrderStatusInquiry, OrderStatusInquiryDto>(entity);
    }
}