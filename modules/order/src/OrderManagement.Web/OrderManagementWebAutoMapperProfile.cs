using AutoMapper;
using OrderManagement.Pages.OrderManagement.Orders;

namespace OrderManagement
{
    public class OrderManagementWebAutoMapperProfile : Profile
    {
        public OrderManagementWebAutoMapperProfile()
        {
            CreateMap<CreateModel.OrderCreateViewModel, CreateOrderDto>();
            CreateMap<OrderDto, EditModel.OrderEditViewModel>();
        }
    }
}