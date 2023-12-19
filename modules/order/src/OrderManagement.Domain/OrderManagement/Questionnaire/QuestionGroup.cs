using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class QuestionGroup :FullAuditedEntity<int>
    {
        public int? Code { get; set; }
        public string Title { get; set; }
        public int QuestionnaireId { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }

    }
}
