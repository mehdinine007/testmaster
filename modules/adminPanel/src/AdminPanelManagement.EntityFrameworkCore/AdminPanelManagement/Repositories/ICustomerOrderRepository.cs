
using AdminPanelManagement.Domain.AdminPanelManagement;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories
{
    public interface ICustomerOrderRepository
    {

        public Task<List<CustomerOrderDb>> GetCustomerOrderList(Guid uid);
        public Task<UserInfoDb> UserInfo(string nationalCode);
        public Task<List<OrderRejectionTypeReadOnly>> GetOrderRejectionType();
        public Task<List<OrderStatusTypeReadOnly>> GetOrderStatusType();
        public Task<List<UserRejectionAdvocacyDb>> GetUserRejectionAdvocacy(string nationalCode);
        public Task<List<AdvocacyUsersFromBankDb>> GetAdvocacyUsersFromBank(string nationalCode);
        public Task<List<UserRejectionFromBankDto>> GetUserRejectionFromBank(string nationalCode);


    }
}
