#nullable disable
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class QuestionAnswer : FullAuditedEntity<long>
{
    private ICollection<SubmittedAnswer> _submittedAnswers;

    public string Description { get; set; } 

    public string Hint { get; set; }

    public int Value { get; set; }

    public string CustomValue { get; set; }

    public int QuestionId { get; set; }

    public int? QuestionnaireId { get; set; }

    public virtual Questionnaire Questionnaire { get; set; }

    public virtual Question Question { get; set; }

    public virtual ICollection<SubmittedAnswer> SubmittedAnswers
    {
        get => _submittedAnswers ?? (_submittedAnswers = new List<SubmittedAnswer>());
        protected set => _submittedAnswers = value;
    }
}
