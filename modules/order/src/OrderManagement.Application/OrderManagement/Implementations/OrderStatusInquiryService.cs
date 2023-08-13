using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrderStatusInquiryService : ApplicationService, IOrderStatusInquiryService
{
    private readonly IRepository<OrderStatusInquiry, long> _orderStatusInquiryRepository;
    private readonly IRepository<CustomerOrder, int> _customerOrderRepository;

    public OrderStatusInquiryService(IRepository<OrderStatusInquiry, long> orderStatusInquiryRepository,
                                     IRepository<CustomerOrder, int> _customerOrderRepository
        )
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
        var order = (await _customerOrderRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == orderStatusInquiryDto.OrderId)
            ?? throw new UserFriendlyException("سفارش یافت نشد");

        throw new System.NotImplementedException();
    }
}