using Esale.Share.Authorize;
using IFG.Core.DataAccess;
using IFG.Core.Utility.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client.Sign;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Sign;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class SignService : ApplicationService, ISignService
    {
        private readonly ISignGrpcClient _signGrpcClient;
        private readonly IOrderReportService _orderReportService;
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<CustomerOrder, int> _customerOrderRepository;
        private readonly IConfiguration _configuration;
        private readonly ICommonAppService _commonAppService;
        public SignService(ISignGrpcClient signGrpcClient, IOrderReportService orderReportService, IRepository<CustomerOrder, int> customerOrderRepository, IConfiguration configuration, ICommonAppService commonAppService, IOrderAppService orderAppService)
        {
            _signGrpcClient = signGrpcClient;
            _orderReportService = orderReportService;
            _customerOrderRepository = customerOrderRepository;
            _configuration = configuration;
            _commonAppService = commonAppService;
            _orderAppService = orderAppService;
        }

        [SecuredOperation(OrderAppServicePermissionConstants.GetOrderDetailById)]
        public async Task<Guid> ContractSign(ContractSignDto contractSignDto)
        {
            var contractReport = await _orderReportService.RptOrderDetail(contractSignDto.OrderId, OrderConstant.ContractReportName);
            var documentParameter = _configuration.GetSection("SignConfig:Contract:IranSign:DocumentParameter").Get<DocumentParameterSign>();
            var nationalCode = "0001001000";//_commonAppService.GetNationalCode();
            var signContract = await _signGrpcClient.CreateSign(new CreateSignGrpcClientRequest()
            {
                Title = "",
                Description = "",
                DocumentName = "Contract.pdf",
                DocumentData = contractReport,
                DocumentParameter = JsonConvert.SerializeObject(documentParameter),
                RecipientUsername = nationalCode
            });
            if (signContract.Success)
            {
                var customerOrder = (await _customerOrderRepository.GetQueryableAsync())
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == contractSignDto.OrderId);
                var responseBody = JsonConvert.DeserializeObject<List<CreateSignResponseBodies>>(signContract.ResponseBody);
                var result = responseBody.FirstOrDefault();
                Guid signTicketId = Guid.Parse(result.workflowTicket);
                var customerOrderDto = new CustomerOrderDto
                {
                    Id = contractSignDto.OrderId,
                    SignStatus = SignStatusEnum.AwaitingSignature,
                    SignTicketId = signTicketId
                };
                var customerOrderMap = ObjectMapper.Map<CustomerOrderDto, CustomerOrder>(customerOrderDto, customerOrder);
                await _customerOrderRepository.AttachAsync(customerOrderMap, x => x.SignStatus, x => x.SignTicketId);
                return signTicketId;
            }
            else
            {
                throw new UserFriendlyException(signContract.Message, signContract.ResultCode.ToString());
            }
        }

        public async Task<IDataResult<InquirySignDto>> Inquiry(Guid ticketId)
        {
            var _inquiry = await _signGrpcClient.InquirySign(ticketId);
            if (_inquiry.Success)
            {
                return new SuccsessDataResult<InquirySignDto>(new InquirySignDto()
                {
                    State = _inquiry.State,
                    DocumentLink = _inquiry.DocumentLink,
                });
            }
            return new ErrorDataResult<InquirySignDto>(_inquiry.Message, messageId: _inquiry.ResultCode.ToString());
        }

        public async Task<bool> CheckSignStatus()
        {
            var customerOrders = (await _customerOrderRepository.GetQueryableAsync())
                .AsNoTracking();
            var awaitingSignatureOrders = customerOrders
               .Where(x => x.SignStatus == SignStatusEnum.AwaitingSignature)
               .ToList();
            foreach (var awaitingSignatureOrder in awaitingSignatureOrders)
            {
                var signStatus = await Inquiry(awaitingSignatureOrder.SignTicketId.Value);
                if (signStatus.Success)
                {
                    IranSignStateEnum signstatus;
                    Enum.TryParse(signStatus.Data.State, out signstatus);
                    await _orderAppService.UpdateStatus(new CustomerOrderDto()
                    {
                        Id = awaitingSignatureOrder.Id,
                        OrderStatus = (int)awaitingSignatureOrder.OrderStatus,
                        SignStatus = signstatus == IranSignStateEnum.COMPLETED ? SignStatusEnum.Signed : signstatus == IranSignStateEnum.CANCELED ? SignStatusEnum.Canceled : SignStatusEnum.AwaitingSignature
                    });
                }
            }

            var signatureIntervalDay = _configuration.GetSection("SignConfig:ReadyForSignatureDay").Value ?? "0";
            var intervalDay = Int32.Parse(signatureIntervalDay);
            var preparingContractOrders = customerOrders
                .Where(x => x.SignStatus == SignStatusEnum.PreparingContract && x.CreationTime <= DateTime.Today.AddDays(-intervalDay))
                .ToList();
            foreach (var preparingContractOrder in preparingContractOrders)
            {
                await _orderAppService.UpdateStatus(new CustomerOrderDto()
                {
                    Id = preparingContractOrder.Id,
                    OrderStatus = (int)preparingContractOrder.OrderStatus,
                    SignStatus = SignStatusEnum.ReadyForSignature
                });
            }
            return true;
        }

    }
}
