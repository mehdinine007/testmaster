using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface ICommonAppService: IApplicationService
    {
        Guid GetUserId();
    }
}
