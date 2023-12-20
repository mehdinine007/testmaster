using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class Question : FullAuditedEntity<int>
{
    private ICollection<QuestionAnswer> _answers;

    private ICollection<SubmittedAnswer> _submittedAnswers;

    public string Title { get; set; }

    public QuestionType QuestionType { get; set; }

    public int QuestionnaireId { get; set; }

    public virtual Questionnaire Questionnaire { get; protected set; }
    public int? QuestionGroupId { get; set; }
    public virtual QuestionGroup QuestionGroup { get; set; }
    public virtual ICollection<QuestionAnswer> Answers
    {
        get => _answers ?? (_answers = new List<QuestionAnswer>());
        protected set => _answers = value;
    }

    public virtual ICollection<SubmittedAnswer> SubmittedAnswers
    {
        get => _submittedAnswers ?? (_submittedAnswers = new List<SubmittedAnswer>());
        protected set => _submittedAnswers = value;
    }

    public Question()
    {
        Answers = new HashSet<QuestionAnswer>();
        SubmittedAnswers = new HashSet<SubmittedAnswer>();
    }

}
