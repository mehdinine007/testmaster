using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public FlowTypeEnum FlowType { get; set; }
        public EntityTypeEnum Entity { get; set; }
        public TypeEnum Type { get; set; }
        public int SchemeId { get; set; }
        public virtual SchemeDto Scheme { get; set; }
    }
}
