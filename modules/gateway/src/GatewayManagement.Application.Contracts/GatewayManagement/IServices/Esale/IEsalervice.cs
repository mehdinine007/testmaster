using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GatewayManagement.Application.Contracts.GatewayManagement.IServices
{
    public interface ICaptchaService: IApplicationService
    {
        Task<HttpResponseMessageDto> GetCaptcha(ContentInputDto Content);
    }
}
