using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class RoleOrganizationChartDto
    {
        public int RoleId { get; set; }
        public int OrganizationChartId { get; set; }
        public  RoleDto Role { get; set; }
        public  OrganizationChartDto OrganizationChart { get; set; }

    }
}
