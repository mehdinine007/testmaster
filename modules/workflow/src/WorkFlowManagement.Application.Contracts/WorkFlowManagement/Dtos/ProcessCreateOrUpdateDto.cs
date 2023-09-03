using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class ProcessCreateOrUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public StateEnum State { get; set; }
        public StatusEnum Status { get; set; }
        public int SchemeId { get; set; }
        public int ActivityId { get; set; }
        public int? PreviousActivityId { get; set; }
        public int OrganizationChartId { get; set; }
        public int CreatedOrganizationChartId { get; set; }
        public int? PreviousOrganizationChartId { get; set; }
        public Guid PersonId { get; set; }
        public Guid? PreviousPersonId { get; set; }
        public Guid CreatedPersonId { get; set; }
    }
}
