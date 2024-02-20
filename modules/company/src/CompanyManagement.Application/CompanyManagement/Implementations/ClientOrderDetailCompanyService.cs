using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Permissions;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.CompanyManagement;
using Esale.Share.Authorize;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace CompanyManagement.Application.CompanyManagement.Implementations
{
    public class ClientOrderDetailCompanyService : ApplicationService, IClientOrderDetailCompanyService
    {
        private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompanyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompany;
        private readonly IRepository<CompanyPaypaidPrices, long> _companyPaypaidPricesRepository;

        public ClientOrderDetailCompanyService(
            IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompanyRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompany,
            IRepository<CompanyPaypaidPrices, long> companyPaypaidPricesRepository)
        {
            _clientsOrderDetailByCompanyRepository = clientsOrderDetailByCompanyRepository;
            _httpContextAccessor = httpContextAccessor;
            _clientsOrderDetailByCompany = clientsOrderDetailByCompany;
            _companyPaypaidPricesRepository = companyPaypaidPricesRepository;
        }

        [UnitOfWork(false)]
        [RemoteService(false)]
        public async Task<OrderDelivaryDto> GetOrderDelivary(string nationalCode, long orderId)
        {
            var OrderDetailByCompany = await _clientsOrderDetailByCompany.GetQueryableAsync();
            var orderDelay = OrderDetailByCompany.GroupJoin((await _companyPaypaidPricesRepository.GetQueryableAsync()),
                   x => x.Id,
                   y => y.ClientsOrderDetailByCompanyId,
                   (dco, d) => new OrderDelivaryDto
                   {
                       Id = dco.Id,
                       NationalCode = dco.NationalCode,
                       //  TranDate = dco.TranDate,
                       PayedPrice = d.Any() ? d.Sum(x => x.PayedPrice) : 0,
                       ContRowId = dco.ContRowId.ToString(),
                       Vin = dco.Vin,
                       BodyNumber = dco.BodyNumber,
                       DeliveryDate = dco.DeliveryDate,
                       FinalPrice = dco.FinalPrice,
                       CarDesc = dco.CarDesc,
                       OrderId = dco.OrderId,
                       ContRowIdDate = dco.ContRowIdDate
                   })
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault(x => x.NationalCode == nationalCode); //&& x.OrderId == orderId);

            return orderDelay;
        }


        public async Task<List<ClientOrderDetailDto>> GetList(string nationalCode)
        {
            var clientsOrderDetailByCompany = (await _clientsOrderDetailByCompanyRepository.GetQueryableAsync())
                .Include(x => x.Paypaidprice)
                .Include(x => x.TurnDate)
                .Where(x => x.NationalCode == nationalCode)
                .GroupBy(x => x.CompanyId)
                .Select(x => x.OrderByDescending(o => o.Id).FirstOrDefault())
                .ToList();
            var clientsOrderDetailByCompnayDto = ObjectMapper.Map<List<ClientsOrderDetailByCompany>, List<ClientOrderDetailDto>>(clientsOrderDetailByCompany, new List<ClientOrderDetailDto>());

            return clientsOrderDetailByCompnayDto;

        }
        //[SecuredOperation(ClientOrderDetailCompanyPermission.Save)]
        public async Task<bool> Save(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
        {
            var clientsOrderDetailByCompnay = ObjectMapper.Map<List<ClientsOrderDetailByCompanyDto>, List<ClientsOrderDetailByCompany>>(clientsOrderDetailByCompnayDtos, new List<ClientsOrderDetailByCompany>());
            var companyId = GetCompanyId();
            clientsOrderDetailByCompnay.ForEach(x =>
            {
                x.CompanyId = int.Parse(companyId);
            });
            await _clientsOrderDetailByCompanyRepository.InsertManyAsync(clientsOrderDetailByCompnay);
            return true;
        }


        private string GetCompanyId()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var CompanyId = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();
            if (CompanyId == null)
            {
                throw new UserFriendlyException("کد شرکت تعریف نشده است");
            }
            return CompanyId;
        }
    }
}
