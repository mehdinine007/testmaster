using Esale.Core.Utility.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement
{
    public interface ICapacityControlAppService
    {
        Task<IResult> SaleDetail();
        Task<IResult> Payment();
        Task<IResult> SaleDetailValidation(Guid saleDetailId,int? agancyId);
        Task GrpcPaymentTest();
    }
}
