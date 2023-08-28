using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class OrganizationChartDto
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
       }
    }

