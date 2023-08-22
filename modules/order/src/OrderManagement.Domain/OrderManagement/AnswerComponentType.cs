using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class AnswerComponentType : FullAuditedEntity<int>
    {

        public AnswerComponentType(int id , string componentName)
        {
            Id = id;
            ComponentName = componentName;
        }

        private ICollection<Questionnaire> _questionnaire;

        public string ComponentName { get; set; }

        public virtual ICollection<Questionnaire> Questionnaire
        {
            get => _questionnaire ?? (_questionnaire = new List<Questionnaire>()); 
            protected set => _questionnaire = value;
        }
    }
}
