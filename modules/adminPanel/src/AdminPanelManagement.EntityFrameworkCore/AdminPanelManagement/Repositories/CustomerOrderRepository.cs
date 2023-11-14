using AdminPanelManagement.Domain.AdminPanelManagement;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using dminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories
{
    public class CustomerOrderRepository : EfCoreRepository<AdminPanelManagementDbContext, Test, int>, ICustomerOrderRepository
    {


        private readonly IConfiguration _configuration;
        private string _connectionStrings;
        public CustomerOrderRepository(IConfiguration configuration, IDbContextProvider<AdminPanelManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _configuration = configuration;
            _connectionStrings = _configuration.GetConnectionString("AdminPanelManagement");

        }

        public async Task<UserInfoDb> UserInfo(string nationalCode)
        {
            var _dbContext = await GetDbContextAsync();
            var userInfoParameters = new[] {
                new SqlParameter("@nationalCode", SqlDbType.NVarChar,10) { Direction = ParameterDirection.Input, Value = nationalCode },
            };
            var userInfo = await _dbContext.Set<UserInfoDb>().FromSqlRaw(string.Format("EXEC {0} {1}", "spUserInfo", "@nationalCode"), userInfoParameters).ToListAsync();
            var userFirst = userInfo.FirstOrDefault();
            return userFirst;
        }

        public async Task<List<CustomerOrderDb>> GetCustomerOrderList(Guid uid)
        {
            var _dbContext = await GetDbContextAsync();
            var customerOrdersParameters = new[] {
                new SqlParameter("@userId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Input, Value = uid } };
            var customerOrders = await _dbContext.Set<CustomerOrderDb>().FromSqlRaw(string.Format("EXEC {0} {1}", "spGetCustomerOrderList", "@userId"), customerOrdersParameters).ToListAsync();
            return customerOrders;
        }


        public async Task<List<OrderRejectionTypeReadOnly>> GetOrderRejectionType()
        {
            var _dbContext = await GetDbContextAsync();
            var res = await _dbContext.Set<OrderRejectionTypeReadOnly>().FromSqlRaw(string.Format("EXEC {0}", "spGetOrderRejectionTypeReadOnly")).ToListAsync();
            return res;
        }

        public async Task<List<OrderStatusTypeReadOnly>> GetOrderStatusType()
        {
            var _dbContext = await GetDbContextAsync();
            var res = await _dbContext.Set<OrderStatusTypeReadOnly>().FromSqlRaw(string.Format("EXEC {0}", "spGetOrderStatusTypeReadOnly")).ToListAsync();
            return res;
        }


        public async Task<List<UserRejectionAdvocacyDb>> GetUserRejectionAdvocacy(string nationalCode)
        {
            var _dbContext = await GetDbContextAsync();
            var userRejectionAdvocacy = new[] {
                new SqlParameter("@nationalCode", SqlDbType.NVarChar,10) { Direction = ParameterDirection.Input, Value = nationalCode } };
            var res = await _dbContext.Set<UserRejectionAdvocacyDb>().FromSqlRaw(string.Format("EXEC {0} {1}", "spGetUserRejectionAdvocacy", "@nationalCode"), userRejectionAdvocacy).ToListAsync();
            return res;
        }

        public async Task<List<AdvocacyUsersFromBankDb>> GetAdvocacyUsersFromBank(string nationalCode)
        {
            var _dbContext = await GetDbContextAsync();
            var userRejectionAdvocacy = new[] {
                new SqlParameter("@nationalCode", SqlDbType.NVarChar,10) { Direction = ParameterDirection.Input, Value = nationalCode } };
            var res = await _dbContext.Set<AdvocacyUsersFromBankDb>().FromSqlRaw(string.Format("EXEC {0} {1}", "spGetAdvocacyUsersFromBank", "@nationalCode"), userRejectionAdvocacy).ToListAsync();
            return res;
        }

        public async Task<List<UserRejectionFromBankDto>> GetUserRejectionFromBank(string nationalCode)
        {
            var _dbContext = await GetDbContextAsync();
            var parameters = new[] {
                new SqlParameter("@nationalCode", SqlDbType.NVarChar) { Direction = ParameterDirection.Input, Value = (object)nationalCode },

            };
            //var res = await _dbContext.Database.SqlQueryRaw<UserRejectionFromBankDto>(" exec dbo.spGetUserREjectionFromBank  @nationalCode", parameters).ToListAsync<UserRejectionFromBankDto>();
            var res = await _dbContext.Set<UserRejectionFromBankDto>().FromSqlRaw(string.Format("EXEC {0} {1}", "spGetUserREjectionFromBank", "@nationalCode"), parameters).ToListAsync();

            return res;
        }



    }

}
