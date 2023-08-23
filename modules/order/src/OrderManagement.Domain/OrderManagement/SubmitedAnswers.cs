using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class SubmittedAnswers : FullAuditedEntity<int>
    {
        public int? AnswerId { get; set; }

        public string AnswerDescription { get; set; }

        public long UserId { get; set; }

        public int QuestionnaireId { get; set; }

        public virtual Questionnaire Questionnaire { get; protected set; }

        public virtual QuestionnaireAnswer QuestionnaireAnswer { get; protected set; }
    }
}
