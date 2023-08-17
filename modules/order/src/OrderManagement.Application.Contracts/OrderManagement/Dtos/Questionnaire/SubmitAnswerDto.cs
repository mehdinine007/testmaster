#nullable disable
namespace OrderManagement.Application.Contracts.Dtos
{
    public class SubmitAnswerDto
    {
        public int QuestionId { get; set; }

        public int QuestionAnswerId { get; set; }

        public string CustomAnswerValue { get; set; }
    }
}
