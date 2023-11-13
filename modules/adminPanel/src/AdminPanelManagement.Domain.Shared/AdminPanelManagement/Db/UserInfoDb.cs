using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db
{
    public class UserInfoDb
    {

        public long Id { get; set; }
        public Guid UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }

    }
}
