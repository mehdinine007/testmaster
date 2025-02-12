﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class OrganizationPositionDto
    {
        public int Id { get; set; }
        public int OrganizationChartId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public virtual OrganizationChartDto OrganizationChart { get; protected set; }
        public string FullName { get; set; }

    }
}
