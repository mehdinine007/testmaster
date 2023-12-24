using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class QuestionGroupDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public int QuestionnaireId { get; set; }
    }
}
