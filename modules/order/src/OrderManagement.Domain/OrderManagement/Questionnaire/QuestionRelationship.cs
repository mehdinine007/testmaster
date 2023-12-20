
using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class QuestionRelationship : FullAuditedEntity<int>
{
    public int QuestionRelationId { get; set; }
    public OperatorFilterEnum OperationType { get; set; }
    public long QuestionAnswerId { get; set; }
    public virtual QuestionAnswer QuestionAnswer { get; set; }
    public int QuestionId { get; set; }
    public virtual Question Question { get; set; }


}
