using IFG.Core.Utility.Results;
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
        Task<IDataResult<string>> ContractSign(int orderId, string title);
    }
}
