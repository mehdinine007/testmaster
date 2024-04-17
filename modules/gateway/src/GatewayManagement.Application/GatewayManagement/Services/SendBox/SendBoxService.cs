#region NS
using Esale.SendBox.Providers.Magfa;
using GatewayManagement.Application.Contracts.Dtos.Esale.IranSign;
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

        public async  Task<CreateIranSignOutput> CreateSign(CreateIranSignDto createIranSignDto)
        {
            var _iranConfig = _configuration.GetSection("SendBoxConfig:Sign:IranSign").Get<IranSignConfig>();
            var _iranSign = new IranSign(_iranConfig);
            var _createDto = new RequestCreateIranSign()
            {
                workflowInfo = new WorkFlowInfo()
                {
                    ownerUsername = _iranConfig.OwnerUserName,
                    title = createIranSignDto.Title,
                    description = createIranSignDto.Description,
                    workflowLanguage = "PERSIAN",
                    workflowPolicyType = "REJECT_WF_IF_REJECT_REQ",
                    workflowType = "INDIVIDUAL",
                    document = new Document()
                    {
                        documentData = new DocumentData()
                        {
                            name = createIranSignDto.DocumentName,
                            data = createIranSignDto.DocumentData
                        }
                    }

                },
                workflowRecipients = new List<WorkFlowRecipients>()
                {
                    new WorkFlowRecipients()
                    {
                        recipientOrder = 0,
                        recipientUsername = createIranSignDto.RecipientUsername,
                        recipientRole = "SIGNER",
                        reminder = new Reminder()
                        {
                            reminderHour = 1
                        },
                        certificate = new Certificate()
                        {
                            productUid = 1,
                            keyStoreType = "pkcs12"
                        },
                        recipientConfig = new RecipientConfig()
                        {
                            downloadPermission = true
                        },
                        documentParameter = JsonConvert.DeserializeObject<DocumentParameter>(createIranSignDto.DocumentParameter)
                    }
                }
            };
            var _ret = await _iranSign.Create(_createDto);
            return new CreateIranSignOutput()
            {
                Message = _ret.message,
                Success = _ret.Success,  
                ResultCode = _ret.resultCode,
                ResponseBody = JsonConvert.SerializeObject(_ret.responseBody),
                WorkflowRecipients = JsonConvert.SerializeObject(_ret.workflowRecipients)
            };
     
        }

        public async Task<ResponseInquiryIranSign> InquirySign(Guid workflowTicket)
        {
            var _iranConfig = _configuration.GetSection("SendBoxConfig:Sign:IranSign").Get<IranSignConfig>();
            var _iranSign = new IranSign(_iranConfig);
            var _ret = await _iranSign.Inquiry(workflowTicket);
            return _ret;
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
