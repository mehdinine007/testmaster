using static UserManagement.Application.UserManagement.Implementations.UserAppService;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using OrderManagement.Domain;
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
        public UserHandler(
            IUserAppService UserAppService
            )
        {
            _userAppService = UserAppService;
        }
        public async Task HandleEventAsync(UserSQL eventData)
        {
            var productId = eventData.BankId;


            try
            {
                await _userAppService.UpsertUserIntoSqlServer(eventData);

            }
            catch (Exception ex)
            {
                throw ex;
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
