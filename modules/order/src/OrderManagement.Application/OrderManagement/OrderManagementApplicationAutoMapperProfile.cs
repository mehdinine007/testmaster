using AutoMapper;

namespace OrderManagement
{
    public class OrderManagementApplicationAutoMapperProfile : Profile
    {
        public OrderManagementApplicationAutoMapperProfile()
        {
            CreateMap<Order, OrderDto>();
        }
    }
}