#nullable disable
namespace OrderManagement.Application.Contracts.Dtos
{
    public class SubmitAnswerTreeDto
    {
        public int QuestionnaireId { get; set; }

        public List<SubmitAnswerDto> SubmitAnswerDto { get; set; }

        public long? RelatedEntity { get; set; }

    }

    public class QuestionnaireAnalysisDto
    {
        public int QuestionId { get; set; }

        public double Rate { get; set; }

        public string QuestionTitle { get; set; }
    }

    public class SubmitAnswerDto
    {
        public int QuestionId { get; set; }

        public long? QuestionAnswerId { get; set; }

        public string CustomAnswerValue { get; set; }
    }
}
