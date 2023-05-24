using PaymentManagement.Application.Contracts.PaymentManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Application.Contracts.PaymentManagement.IServices
{
    [ServiceContract]
    public interface IGrpcPaymentAppService
    {
        Task<List<InquiryWithFilterParamDto>> GetPaymentStatusList(PaymentStatusDto paymentStatusDto);

    }
}
