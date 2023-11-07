using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos
{
    public class UserInfo_CustomerOrderDto
    {
        public long UserId { get; set; }
        public Guid UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public List<string> CancellationDate { get; set; }
        public List<string> AdvocacyUsersDate { get; set; }
        public List<CustomerOrderDto> CustomerOrders { get; set; }
    }
}
