using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Inqueries;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
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
        private readonly IRepository<Agency, int> _agencyRepository;

        public ReportApplicationService(IEsaleGrpcClient grpcClient,
                                        IRepository<SaleDetail, int> saleDetailRepository,
                                        IRepository<Agency, int> agencyRepository
            )
        {
            _grpcClient = grpcClient;
            _saleDetailRepository = saleDetailRepository;
            _agencyRepository = agencyRepository;
        }

        public async Task<CustomPagedResultDto<SaleDetailResultDto>> SaleDetailReport(SaleDetailReportInquery input)
        {
            input.MaxResultCount = 10;
            var saleDetailQuery = await _saleDetailRepository.GetQueryableAsync();
            var now = DateTime.Now;
            saleDetailQuery = input.ActiveOrDeactive
                ? /* actives */ saleDetailQuery.Where(x => x.SalePlanEndDate >= now)
                : /* deactives */ saleDetailQuery.Where(x => x.SalePlanStartDate < now && x.SalePlanEndDate > now);
            var totalCount = await saleDetailQuery.CountAsync();
            if (totalCount <= 0)
                return new CustomPagedResultDto<SaleDetailResultDto>(new List<SaleDetailResultDto>(), totalCount);

            saleDetailQuery = saleDetailQuery.PageBy(input);
            var saleDetailIds = saleDetailQuery.Select(x => x.Id).ToList();
            List<SaleDetailResultDto> saleDetailInqueries = new(saleDetailIds.Count);
            foreach (var saleDetailId in saleDetailIds)
            {
                saleDetailInqueries.Add(await GetSaleDetailReport(saleDetailId));
            }
            var saleDetailReport = saleDetailInqueries.Select(x => x).ToList();
            return new CustomPagedResultDto<SaleDetailResultDto>(saleDetailReport, totalCount);
        }

        private async Task<SaleDetailResultDto> GetSaleDetailReport(int saleDetailId)
        {
            var saleDetail = _saleDetailRepository.WithDetails(x => x.AgencySaleDetails).FirstOrDefault(x => x.Id == saleDetailId)
                ?? throw new UserFriendlyException("برنامه فروش پیدا نشد");
            var agencyIds = saleDetail.AgencySaleDetails.Select(x => x.AgencyId).ToList();
            var agencies = _agencyRepository.WithDetails().Where(x => agencyIds.Any(y => y == x.Id));

            var data = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto
            {
                RelationId = saleDetailId,
                IsRelationIdGroup = true,
                IsRelationIdBGroup = true,

            });
            var reports = data.Select(x =>
            {
                var currentAgency = agencies.FirstOrDefault(y => y.Id == x.F2);
                if(currentAgency != null)
                {
                    return new SaleDetailReportDto()
                    {
                        AgencyName = currentAgency.Name,
                        Count = x.Count,
                        PaymentStatus = x.Message,
                        SaleDetailTitle = saleDetail.SalePlanDescription
                    };
                }
                else
                {
                    return new SaleDetailReportDto();
                }
               
            }).ToList();
            var result = new SaleDetailResultDto()
            {
                Reports = reports,
                SaleDetailId = saleDetailId
            };
            return result;
        }
    }
}
