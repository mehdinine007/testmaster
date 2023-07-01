using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.Helpers;
using OrderManagement.Domain;
using OrderManagement.Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<OrderStatusTypeReadOnly, int> _orderStatusTypeReadOnlyRepository;
        private readonly IRepository<Agency, int> _agencyRepository;




        public ReportApplicationService(IEsaleGrpcClient grpcClient,
                                        IRepository<SaleDetail, int> saleDetailRepository,
                                        IRepository<AgencySaleDetail, int> agencySaleDetailRepository, IRepository<OrderStatusTypeReadOnly, int> orderStatusTypeReadOnlyRepository, IRepository<Agency, int> agencyRepository
            )
        {
            _grpcClient = grpcClient;
            _saleDetailRepository = saleDetailRepository;
            _agencySaleDetailRepository = agencySaleDetailRepository;
            _orderStatusTypeReadOnlyRepository = orderStatusTypeReadOnlyRepository;
            _agencyRepository = agencyRepository;

        }

        public async Task<List<SaleDetailReportDto>> SaleDetailReport(int saleDetailId)
        {

            var saleDetail = _saleDetailRepository.WithDetails(x => x.AgencySaleDetails).FirstOrDefault(x => x.Id == saleDetailId)
                ?? throw new UserFriendlyException("برنامه فروش پیدا نشد");
            var agencyIds = saleDetail.AgencySaleDetails.Select(x => x.AgencyId).ToList();
            var agencies = _agencyRepository.WithDetails().Where(x => agencyIds.Any(y => y == x.Id));

            var data = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto
            {
                RelationIdB = saleDetailId,
                IsRelationIdCGroup = true,
                IsRelationIdBGroup = true,
                
            });
            var result = data.Select(x =>
            {
                var currentAgency = agencies.FirstOrDefault(y => y.Id == x.F2);
                return new SaleDetailReportDto()
                {
                    AgencyName = currentAgency.Name,
                    Count = x.Count,
                    PaymentStatus = x.Message,
                    SaleDetailTitle = saleDetail.SalePlanDescription
                };
            }).ToList();
            return result;
        }

    }
}
