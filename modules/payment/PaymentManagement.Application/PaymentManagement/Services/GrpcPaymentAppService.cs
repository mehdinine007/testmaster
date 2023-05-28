using PaymentManagement.Application.Contracts;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.Contracts.PaymentManagement.Dtos;
using PaymentManagement.Application.Contracts.PaymentManagement.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PaymentManagement.Application.PaymentManagement.Services
{
    public class GrpcPaymentAppService : ApplicationService, IGrpcPaymentAppService
    {
        private readonly IPaymentAppService _paymentAppService;
        public GrpcPaymentAppService(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        public List<InquiryWithFilterParamDto> GetPaymentStatusList(PaymentStatusDto paymentStatusDto)
        {
            return _paymentAppService.InquiryWithFilterParam(paymentStatusDto.RelationId, paymentStatusDto.RelationIdB, paymentStatusDto.RelationIdC, paymentStatusDto.RelationIdD);
        }
    }
}
