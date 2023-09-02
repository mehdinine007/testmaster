using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class RoleOrganizationChartCreateOrUpdateDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int OrganizationChartId { get; set; }
    }
}
