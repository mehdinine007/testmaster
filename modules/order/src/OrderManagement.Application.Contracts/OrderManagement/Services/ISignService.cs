using IFG.Core.Utility.Results;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Sign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ISignService : IApplicationService
    {
        Task<Guid> ContractSign(ContractSignDto contractSignDto);
        Task<IDataResult<InquirySignDto>> Inquiry(Guid ticketId);
    }
}
