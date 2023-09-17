using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class OrganizationChartDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public virtual ICollection<OrganizationChartDto> Childrens { get; set; }
        public string TypeTitle { get; set; }
        public OrganizationTypeEnum OrganizationType { get; set; }

    }
    }

