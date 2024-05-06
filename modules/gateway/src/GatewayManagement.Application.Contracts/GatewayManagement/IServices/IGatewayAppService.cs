using GatewayManagement.Application.Contracts.Dtos;
using Volo.Abp.Application.Services;

namespace GatewayManagement.Application.Contracts.IServices
{
    public interface IGatewayAppService : IApplicationService
    {
        Task<OutputDto> Authenticate(AuthenticateInputDto input);

        #region HandShake
        Task<OutputDto> HandShakeWithIranKish(IranKishHandShakeInputDto input);
        Task<OutputDto> HandShakeWithMellat(MellatHandShakeInputDto input);
        Task<OutputDto> HandShakeWithParsian(ParsianHandShakeInputDto input);
        Task<OutputDto> HandShakeWithPasargad(PasargadHandShakeInputDto input);
        #endregion

        #region Verify
        Task<OutputDto> VerifyToIranKish(IranKishVerifyInputDto input);
        Task<OutputDto> VerifyToMellat(MellatVerifyInputDto input);
        Task<OutputDto> VerifyToParsian(ParsianVerifyInputDto input);
        Task<OutputDto> VerifyToPasargad(PasargadVerifyInputDto input);
        #endregion

        #region Inquiry
        Task<OutputDto> InquiryToIranKish(IranKishInquiryInputDto input);
        Task<OutputDto> InquiryToMellat(MellatInquiryInputDto input);
        Task<OutputDto> InquiryToParsian(ParsianInquiryInputDto input);
        Task<OutputDto> InquiryToPasargad(PasargadInquiryInputDto input);
        #endregion

        #region Reverse
        Task<OutputDto> ReverseToIranKish(IranKishReverseInputDto input);
        Task<OutputDto> ReverseToMellat(MellatReverseInputDto input);
        Task<OutputDto> ReverseToParsian(ParsianReverseInputDto input);
        Task<OutputDto> ReverseToPasargad(PasargadReverseInputDto input);
        #endregion
    }
}
