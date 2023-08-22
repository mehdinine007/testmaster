using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class QuestionnaireAnswer : FullAuditedEntity<int>
    {
        private ICollection<SubmittedAnswers> _submittedAnswers;

        public string Description { get; set; }

        public int QuestionnaireId { get; set; }

        public virtual Questionnaire Questionnaire { get; protected set; }

        public virtual ICollection<SubmittedAnswers> SubmittedAnswers
        {
            get => _submittedAnswers ?? (_submittedAnswers = new List<SubmittedAnswers>());
            protected set => _submittedAnswers = value;
        }
    }
}
