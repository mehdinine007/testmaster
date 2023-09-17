using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.CommonService.Dto.Getway;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IGetwayGrpcClient : IApplicationService
    {
        Task<HttpResponseMessageDto> GetCaptcha(ContentInputDto Content);
        Task<SendBoxServiceDto> SendService(SendBoxServiceInput input);
    }
}
