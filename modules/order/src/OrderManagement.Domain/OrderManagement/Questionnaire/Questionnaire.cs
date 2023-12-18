using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class Questionnaire : FullAuditedEntity<int>
{
    private ICollection<Question> _questions;

    private ICollection<QuestionAnswer> _answers;
    private ICollection<QuestionGroup> _questionGroups;

    private ICollection<UnAuthorizedUser> _unAuthorizedUsers;

    public string Title { get; set; }

    public string Description { get; set; }

    public int? WhitListRequirement { get; set; }

    public QuestionnaireType QuestionnaireType { get; set; }

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
    public virtual ICollection<QuestionGroup> QuestionGroups
    {
        get => _questionGroups ?? (_questionGroups = new List<QuestionGroup>());
        protected set => _questionGroups = value;
    }
    public ICollection<UnAuthorizedUser> UnAuthorizedUsers
    {
        get => _unAuthorizedUsers ?? (_unAuthorizedUsers = new List<UnAuthorizedUser>());
        protected set => _unAuthorizedUsers = value;
    }

    public Questionnaire()
    {
        Questions = new HashSet<Question>();
    }
}
