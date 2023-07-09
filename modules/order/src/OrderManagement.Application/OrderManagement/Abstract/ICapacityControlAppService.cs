using Esale.Core.Utility.Results;
using OrderManagement.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement
{
    public interface ICapacityControlAppService
    {
        Task<IResult> SaleDetail();
        Task<IResult> Payment();
        Task<IDataResult<List<PaymentStatusModel>>> Validation(int saleDetaild,int? agencyId);
        Task<IResult> AgencyValidation(int saledetailId, int? agencyId, List<PaymentStatusModel> paymentDtos);
        Task<bool> ValidationBySaleDetailUId(Guid saleDetailUId);
        Task GrpcPaymentTest();
    }
}
