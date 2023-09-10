using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class ExecuteQueryDto
    {
        public Guid ProcessId { get; set; }
        public StateEnum Action { get; set; }
    }
}
