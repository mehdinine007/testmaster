using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class QuestionnaireAnswer : FullAuditedEntity<int>
    {
        private ICollection<SubmitedAnswers> _submitedAnswers;

        public string Description { get; set; }

        public int QuestionnaireId { get; set; }

        public virtual Questionnaire Questionnaire { get; protected set; }

        public virtual ICollection<SubmitedAnswers> SubmitedAnswers
        {
            get => _submitedAnswers ?? (_submitedAnswers = new List<SubmitedAnswers>());
            protected set => _submitedAnswers = value;
        }
    }
}
