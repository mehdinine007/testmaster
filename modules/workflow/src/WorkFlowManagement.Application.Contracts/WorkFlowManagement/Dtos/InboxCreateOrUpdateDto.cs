using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class InboxCreateOrUpdateDto
    {
        public int Id { get; set; }
        public Guid ProcessId { get; set; }
        public Guid PersonId { get; set; }
        public int OrganizationChartId { get; set; }
        public int OrganizationPositionId { get; set; }
        public StateEnum State { get; set; }
        public InboxStatusEnum Status { get; set; }
        public string ReferenceDescription { get; set; }
        public int? ParentId { get; set; }


    }
}
