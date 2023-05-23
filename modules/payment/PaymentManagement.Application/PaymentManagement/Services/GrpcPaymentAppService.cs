using PaymentManagement.Application.Contracts.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PaymentManagement.Payments
{
    public class GrpcPaymentAppService : ApplicationService, IGrpcPaymentAppService
    {
        public async Task<List<PaymentDto>> GetListAsync()
        {
            return new List<PaymentDto>
        {
            new PaymentDto { Id = Guid.NewGuid(), Name = "Payment 1" },
            new PaymentDto { Id = Guid.NewGuid(), Name = "Payment 2" },
        };
        }

    }
}
