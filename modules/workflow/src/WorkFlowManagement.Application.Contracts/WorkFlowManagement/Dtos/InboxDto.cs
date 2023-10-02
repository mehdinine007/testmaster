using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class InboxDto
    {
        public int Id { get; set; }
        public Guid ProcessId { get; set; }
        public  ProcessDto Process { get; set; }

        public Guid PersonId { get; set; }
        public int OrganizationChartId { get; set; }
        public  OrganizationChartDto OrganizationChart { get; set; }
        public int OrganizationPositionId { get; set; }
        public  OrganizationPositionDto OrganizationPosition { get; set; }
        public StateEnum State { get; set; }
        public string StateTitle { get; set; }
        public InboxStatusEnum Status { get; set; }
        public string InboxStatusTilte { get; set; }
        public string ReferenceDescription { get; set; }
        public int? ParentId { get; set; }
        public  InboxDto Parent { get; set; }
        public string FullName { get; set; }
    }
}
