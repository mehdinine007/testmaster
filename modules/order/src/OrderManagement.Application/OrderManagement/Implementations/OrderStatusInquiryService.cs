using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System.Linq;
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

    public async Task<OrderStatusInquiryDto> GetCurrentUserOrderStatus(string nationalCode, int customerOrderId)
    {
        //(await _orderStatusInquiryRepository.GetQueryableAsync()).Where(x => EF.Functions.)

        throw new System.NotImplementedException();
    }

    public async Task<OrderStatusInquiryDto> Insert(OrderStatusInquiryDto orderStatusInquiryDto)
    {
        var entity = await _orderStatusInquiryRepository.InsertAsync(
                    ObjectMapper.Map<OrderStatusInquiryDto, OrderStatusInquiry>(orderStatusInquiryDto));

        return ObjectMapper.Map<OrderStatusInquiry, OrderStatusInquiryDto>(entity);
    }
}