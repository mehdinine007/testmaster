using static UserManagement.Application.UserManagement.Implementations.UserAppService;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.Models;
using Volo.Abp.EventBus;
using UserManagement.Domain.UserManagement.Authorization.Users;

namespace UserService.Host.RabbitHandlers
{
    public class UserHandler
      : IDistributedEventHandler<UserSQL>,
        ITransientDependency
    {
        private readonly IUserAppService _userAppService;
        private IConfiguration _configuration { get; set; }
        public UserHandler(
            IUserAppService UserAppService
, IConfiguration configuration)
        {
            _userAppService = UserAppService;
            _configuration = configuration;
        }
        public async Task HandleEventAsync(UserSQL eventData)
        {
            var productId = eventData.BankId;


            try
            {
                if (_configuration.GetSection("HasUserRabbit").Value == "1")
                    await _userAppService.UpsertUserIntoSqlServer(eventData);

            }
            catch (Exception ex)
            {
            }
        }
    }
    //public class DistributedEventReceivedHandler : ILocalEventHandler<DistributedEventReceived>, ITransientDependency
    //{
    //    public async Task HandleEventAsync(DistributedEventReceived eventData)
    //    {
    //        // TODO: IMPLEMENT YOUR LOGIC...
    //    }
    //}

}
