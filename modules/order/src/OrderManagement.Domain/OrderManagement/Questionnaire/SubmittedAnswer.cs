using OrderManagement.Domain.Shared;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class SubmittedAnswer : FullAuditedEntity<long>
{
    public virtual Question Question { get; protected set; }

    public virtual QuestionAnswer QuestionAnswer { get; set; }

    public int QuestionId { get; set; }

    public long? QuestionAnswerId { get; set; }

    public string CustomAnswerValue { get; set; }

    public Guid? UserId { get; set; }

    public long? RelatedEntityId { get; set; }
}
