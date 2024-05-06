#region NS
using Esale.SendBox.Providers.Magfa;
using GatewayManagement.Application.Contracts.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using GatewayManagement.Application.Contracts.GatewayManagement.Enums;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using GatewayManagement.Application.GatewayManagement.Services.SendBox.Sign.Provider;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Volo.Abp.Application.Services;
#endregion

namespace GatewayManagement.Application.GatewayManagement.Services.SendBox
{
    public class SendBoxService : ApplicationService, ISendBoxService
    {
        private IConfiguration _configuration;

        public SendBoxService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CreateSignOutputDto> CreateSign(CreateSignDto createSignDto)
        {
            var _iranConfig = _configuration.GetSection("SendBoxConfig:Sign:IranSign").Get<IranSignConfig>();
            var _iranSign = new IranSign(_iranConfig);
            var _createDto = new RequestCreateIranSign()
            {
                workflowInfo = new WorkFlowInfo()
                {
                    ownerUsername = _iranConfig.OwnerUserName,
                    title = createSignDto.Title,
                    description = createSignDto.Description,
                    workflowLanguage = "PERSIAN",
                    workflowPolicyType = "REJECT_WF_IF_REJECT_REQ",
                    workflowType = "INDIVIDUAL",
                    document = new Document()
                    {
                        documentData = new DocumentData()
                        {
                            name = createSignDto.DocumentName,
                            data = createSignDto.DocumentData
                        }
                    }

                },
                workflowRecipients = new List<WorkFlowRecipients>()
                {
                    new WorkFlowRecipients()
                    {
                        recipientOrder = 0,
                        recipientUsername = createSignDto.RecipientUsername,
                        recipientRole = "SIGNER",
                        reminder = new Reminder()
                        {
                            reminderHour = 1
                        },
                        certificate = new Certificate()
                        {
                            productUid = _iranConfig.ProductUid,
                            keyStoreType = _iranConfig.KeyStoreType
                        },
                        recipientConfig = new RecipientConfig()
                        {
                            downloadPermission = true
                        },
                        documentParameter = JsonConvert.DeserializeObject<DocumentParameter>(createSignDto.DocumentParameter)
                    }
                }
            };
            var _ret = await _iranSign.Create(_createDto);
            return new CreateSignOutputDto()
            {
                Message = _ret.message,
                Success = _ret.Success,
                ResultCode = _ret.resultCode,
                ResponseBody = JsonConvert.SerializeObject(_ret.responseBody),
            };

        }

        public async Task<InquirySignOutputDto> InquirySign(Guid workflowTicket)
        {
            var _iranConfig = _configuration.GetSection("SendBoxConfig:Sign:IranSign").Get<IranSignConfig>();
            var _iranSign = new IranSign(_iranConfig);
            var result = await _iranSign.Inquiry(workflowTicket);
            if (result.Success)
            {
                return new InquirySignOutputDto
                {
                    Message = result.message,
                    Success = true,
                    State = result.responseBody.state,
                    DocumentLink = result.responseBody.documentInfo.documentLink,
                    SignedDocumentLink = result.responseBody.signedDocumentInfo.signedDocumentLink
                };
            }
            return new InquirySignOutputDto
            {
                Message = result.message,
                Success = result.Success
            };
        }
        public async Task<SendBoxServiceDto> SendService(SendBoxServiceInput sendBoxService)
        {
            if (sendBoxService.Provider == ProviderSmsTypeEnum.Magfa && sendBoxService.Type == TypeMessageEnum.Sms)
            {
                var _magfaConfig = _configuration.GetSection("SendBoxConfig:Sms:Magfa").Get<MagfaConfig>();
                var _magfa = new MagfaSendSms(_magfaConfig);
                var _ret = await _magfa.Send(sendBoxService.Text, sendBoxService.Recipient);
                return _ret;
            }
            return new SendBoxServiceDto
            {
                Success = false,
                DataResult = "None Service Send",
                MessageCode = 97
            };
        }
    }
}
