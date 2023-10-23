using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class ProcessDto
    {
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public StateEnum State { get; set; }
        public string StateTitle { get; set; }
        public StatusEnum Status { get; set; }
        public string StatusTitle { get; set; }
        public int SchemeId { get; set; }
        public  SchemeDto Scheme { get; set; }
        public int ActivityId { get; set; }
        public  ActivityDto Activity { get; set; }

        public int? PreviousActivityId { get; set; }
        public  ActivityDto PreviousActivity { get; set; }

        public int OrganizationChartId { get; set; }
        public  OrganizationChartDto OrganizationChart { get; set; }

        public int CreatedOrganizationChartId { get; set; }
        public  OrganizationChartDto CreatedOrganizationChart { get; set; }
        public int? PreviousOrganizationChartId { get; set; }
        public  OrganizationChartDto PreviousOrganizationChart { get; set; }

        public Guid PersonId { get; set; }
        public Guid? PreviousPersonId { get; set; }
        public Guid CreatedPersonId { get; set; }
        public Guid? ParentId { get; set; }
        public ProcessDto Parent { get; set; }
        public string FullName { get; set; }

    }
}
