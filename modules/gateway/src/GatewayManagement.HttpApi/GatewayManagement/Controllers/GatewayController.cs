using Microsoft.AspNetCore.Mvc;
using GatewayManagement.Application.Contracts.IServices;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using GatewayManagement.Application.Contracts.Dtos;

namespace GatewayManagement
{
    [RemoteService]
    [Area("GatewayManagement")]
    [Route("api/services/app/GatewayService/[action]")]
    public class GatewayController : AbpController
    {
        private readonly IGatewayAppService _gatewayAppService;

        public GatewayController(IGatewayAppService gatewayAppService)
        {
            _gatewayAppService = gatewayAppService;
        }

        [HttpPost]
        public async Task<OutputDto> HandShakeWithIranKish(IranKishHandShakeInputDto input) => await _gatewayAppService.HandShakeWithIranKish(input);

        [HttpPost]
        public async Task<OutputDto> HandShakeWithMellat(MellatHandShakeInputDto input) => await _gatewayAppService.HandShakeWithMellat(input);

        [HttpPost]
        public async Task<OutputDto> VerifyToIranKish(IranKishVerifyInputDto input) => await _gatewayAppService.VerifyToIranKish(input);

        [HttpPost]
        public async Task<OutputDto> VerifyToMellat(MellatVerifyInputDto input) => await _gatewayAppService.VerifyToMellat(input);

        [HttpPost]
        public async Task<OutputDto> InquiryToIranKish(IranKishInquiryInputDto input) => await _gatewayAppService.InquiryToIranKish(input);

        [HttpPost]
        public async Task<OutputDto> InquiryToMellat(MellatInquiryInputDto input) => await _gatewayAppService.InquiryToMellat(input);

        [HttpPost]
        public async Task<OutputDto> ReverseToIranKish(IranKishReverseInputDto input) => await _gatewayAppService.ReverseToIranKish(input);

        [HttpPost]
        public async Task<OutputDto> ReverseToMellat(MellatReverseInputDto input) => await _gatewayAppService.ReverseToMellat(input);

    }
}
