using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace CompanyManagement.Application.CompanyManagement.Implementations
{
    public class GrpcOrderDeliveryService: ApplicationService, IOrderDeliveryService
    {
        private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompany;
        private readonly IRepository<CompanyPaypaidPrices, long> _companyPaypaidPricesRepository;

        public GrpcOrderDeliveryService(IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompany, 
                                    IRepository<CompanyPaypaidPrices, long> companyPaypaidPricesRepository)
        {
            _clientsOrderDetailByCompany = clientsOrderDetailByCompany;
            _companyPaypaidPricesRepository = companyPaypaidPricesRepository;
        }
        [UnitOfWork(false)]
        [RemoteService(false)]
        public async Task<OrderDelivaryDto> GetOrderDelivary(string nationalCode, long orderId)
        {
            System.Diagnostics.Debugger.Launch();
            var OrderDetailByCompany = await _clientsOrderDetailByCompany.GetQueryableAsync();
            var orderDelay = OrderDetailByCompany.GroupJoin((await _companyPaypaidPricesRepository.GetQueryableAsync()),
                   x => x.Id,
                   y => y.ClientsOrderDetailByCompanyId,
                   (dco, d) => new OrderDelivaryDto
                   {
                       Id = dco.Id,
                       NationalCode = dco.NationalCode,
                       TranDate = d.Max(x => x.TranDate),
                       PayedPrice = d.Any() ? d.Sum(x => x.PayedPrice) : 0,
                       ContRowId = dco.ContRowId.ToString(),
                       Vin = dco.Vin,
                       BodyNumber = dco.BodyNumber,
                       DeliveryDate = dco.DeliveryDate,
                       FinalPrice = dco.FinalPrice,
                       CarDesc = dco.CarDesc,
                       OrderId = dco.OrderId
                   })
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault(x => x.NationalCode == nationalCode && x.OrderId == orderId);
            if (orderDelay == null)
            {
                throw new UserFriendlyException("سفارشی وجود ندارد.");
            }
            return orderDelay;
        }
    }
}
