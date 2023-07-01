using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class ReportApplicationService : ApplicationService, IReportApplicationService
    {
        private readonly IEsaleGrpcClient _grpcClient;
        private readonly IRepository<SaleDetail, int> _saleDetailRepository;
        private readonly IRepository<AgencySaleDetail, int> _agencySaleDetailRepository;

        public ReportApplicationService(IEsaleGrpcClient grpcClient,
                                        IRepository<SaleDetail, int> saleDetailRepository,
                                        IRepository<AgencySaleDetail, int> agencySaleDetailRepository
            )
        {
            _grpcClient= grpcClient;
            _saleDetailRepository = saleDetailRepository;
            _agencySaleDetailRepository = agencySaleDetailRepository;
        }

        public async Task<SaleDetailReportDto> SaleDetailReport(int saleDetailId)
        {
            var saleDetail = _saleDetailRepository.FirstAsync(x => x.Id == saleDetailId)
                ?? throw new UserFriendlyException("برنامه فروش پیدا نشد");
            
            var saleDetailAgencies = _agencySaleDetailRepository.
            var data = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto
            {
                RelationIdB = saleDetailId,
                IsRelationIdCGroup = true,
                IsRelationIdBGroup = true,
            });

            throw new NotImplementedException();
        }
    }
}
