using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class ActivityRoleCreateOrUpdate
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int RoleId { get; set; }
    }
}
