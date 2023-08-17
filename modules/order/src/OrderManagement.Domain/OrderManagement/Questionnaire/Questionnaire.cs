using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class Questionnaire : FullAuditedEntity<int>
{
    private ICollection<Question> _questions;

    private ICollection<QuestionAnswer> _answers;

    public string Title { get; set; }

    public string Description { get; set; }

    public int? WhitListRequirement { get; set; }

    public virtual ICollection<Question> Questions
    {
        get => _questions ?? (_questions = new List<Question>());
        protected set => _questions = value;
    }

    public virtual ICollection<QuestionAnswer> Answers
    {
        get => _answers ?? (_answers = new List<QuestionAnswer>());
        protected set => _answers = value;
    }
}
