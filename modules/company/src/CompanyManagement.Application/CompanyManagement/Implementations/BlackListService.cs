using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using CompanyManagement.Application.Contracts.CompanyManagement.Enums;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Application.Contracts.Services;
using CompanyManagement.Domain.CompanyManagement;
using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using Permission.Company;
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
        private readonly IOrderGrpcClientService _orderGrpcClientService;
        public BlackListService(
            IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompanyRepository, IOrderGrpcClientService orderGrpcClientService)
        {
            _clientsOrderDetailByCompanyRepository = clientsOrderDetailByCompanyRepository;
            _orderGrpcClientService = orderGrpcClientService;
        }
        [SecuredOperation(BlackListServicePermissionConstants.Inquiry)] 
        public async Task<bool> Inquiry(string nationalCode)
        {
            var clientsOrderDetailByCompany = (await _clientsOrderDetailByCompanyRepository.GetQueryableAsync()).AsNoTracking()
                .Include(x => x.Paypaidprice)
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefault(x => x.NationalCode == nationalCode);
            if (clientsOrderDetailByCompany is not null  && clientsOrderDetailByCompany.Paypaidprice.Any())
                return true;
            var _hasWinner = await _orderGrpcClientService.ExistsWinnerByNationalCode(nationalCode,GrpcProviderEnum.External);
            return _hasWinner;
        }
    }
}
