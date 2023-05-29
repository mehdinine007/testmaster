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
        Task<IResult> Validation(int saleDetaild,int? agencyId);
        Task<bool> ValidationBySaleDetailUId(Guid saleDetailUId);
        Task GrpcPaymentTest();
    }
}
