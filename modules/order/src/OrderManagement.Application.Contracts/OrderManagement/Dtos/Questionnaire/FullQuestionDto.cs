using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class FullQuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public QuestionType QuestionType { get; set; }
        public int QuestionnaireId { get; set; }
        public int? QuestionGroupId { get; set; }
        public QuestionGroupDto QuestionGroup { get; set; }
        
        public virtual ICollection<QuestionAnswerDto> Answers { get; set; }
        public virtual ICollection<SubmittedAnswerDto> SubmittedAnswers { get; set; }
        public virtual ICollection<QuestionRelationshipDto> QuestionRelationships { get; set; }
    }
}
