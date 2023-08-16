using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class SubmittedAnswer : FullAuditedEntity<long>
{
    public virtual Question Question { get; protected set; }

    public int QuestionId { get; set; }

    public virtual QuestionAnswer QuestionAnswer { get; set; }

    public long? QuestionAnswerId { get; set; }

    public string CustomAnswerValue { get; set; }

    public int UserId { get; set; }
}
