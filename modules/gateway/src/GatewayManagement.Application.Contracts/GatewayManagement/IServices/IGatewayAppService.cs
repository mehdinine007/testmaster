using GatewayManagement.Application.Contracts.Dtos;
using Volo.Abp.Application.Services;

namespace GatewayManagement.Application.Contracts.IServices
{
    public interface IGatewayAppService : IApplicationService
    {
        Task<OutputDto> Authenticate(AuthenticateInputDto input);
        Task<OutputDto> HandShakeWithIranKish(IranKishHandShakeInputDto input);
        Task<OutputDto> HandShakeWithMellat(MellatHandShakeInputDto input);
        Task<OutputDto> HandShakeWithParsian(ParsianHandShakeInputDto input);
        Task<OutputDto> HandShakeWithPasargad(PasargadHandShakeInputDto input);
        Task<OutputDto> VerifyToIranKish(IranKishVerifyInputDto input);
        Task<OutputDto> VerifyToMellat(MellatVerifyInputDto input);
        Task<OutputDto> VerifyToParsian(ParsianVerifyInputDto input);
        Task<OutputDto> VerifyToPasargad(PasargadVerifyInputDto input);
        Task<OutputDto> InquiryToIranKish(IranKishInquiryInputDto input);
        Task<OutputDto> InquiryToMellat(MellatInquiryInputDto input);
        Task<OutputDto> InquiryToParsian(ParsianInquiryInputDto input);
        Task<OutputDto> InquiryToPasargad(PasargadInquiryInputDto input);
        Task<OutputDto> ReverseToIranKish(IranKishReverseInputDto input);
        Task<OutputDto> ReverseToMellat(MellatReverseInputDto input);
        Task<OutputDto> ReverseToParsian(ParsianReverseInputDto input);
        Task<OutputDto> ReverseToPasargad(PasargadReverseInputDto input);
    }
}
