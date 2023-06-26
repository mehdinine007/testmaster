using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class SubmitedAnswers : FullAuditedEntity<int>
    {
        public int? AnswerId { get; set; }

        public string AnswerDescription { get; set; }

        public long UserId { get; set; }

        public virtual QuestionnaireAnswer QuestionnaireAnswer { get; protected set; }
    }
}
