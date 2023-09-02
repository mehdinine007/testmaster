using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class RoleCreateOrUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public List<SecurityTypeEnum> Security { get; set; }
    }
}
