using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class TransitionCreateOrUpdateDto
    {
        public int Id { get; set; }
        public int ActivitySourceId { get; set; }
        public int ActivityTargetId { get; set; }
        public int SchemeId { get; set; }

    }
}
