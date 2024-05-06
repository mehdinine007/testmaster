using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.CompanyManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CompanyManagement.Application.CompanyManagement.Implementations
{
    public class BlackListService : ApplicationService, IBlackListService
    {

        private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompanyRepository;
        public BlackListService(
            IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompanyRepository)
        {
            _clientsOrderDetailByCompanyRepository = clientsOrderDetailByCompanyRepository;
        }

        public async Task<bool> Inquiry(string nationalCode)
        {
            var clientsOrderDetailByCompany = (await _clientsOrderDetailByCompanyRepository.GetQueryableAsync()).AsNoTracking()
                .Include(x => x.Paypaidprice)
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefault(x => x.NationalCode == nationalCode);
            if (clientsOrderDetailByCompany is  null)
            {
                return false;
            }
            return clientsOrderDetailByCompany.Paypaidprice.Any();
           
        }
    }
}
