using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts;

public class QuestionnaireTreeDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public QuestionnaireType QuestionnaireType { get; set; }

    public int? WhitListRequirement { get; set; }

    public virtual ICollection<FullQuestionDto> Questions { get; set; }
}
