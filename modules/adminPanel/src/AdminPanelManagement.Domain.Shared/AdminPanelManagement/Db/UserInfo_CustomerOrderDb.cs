using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db
{
    public class UserInfo_CustomerOrderDb
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }

        public string CancellationDate { get; set; }
        public string AdvocacyUsersDate { get; set; }
        public List<CustomerOrderDb> CustomerOrders { get; set; }
    }
}
