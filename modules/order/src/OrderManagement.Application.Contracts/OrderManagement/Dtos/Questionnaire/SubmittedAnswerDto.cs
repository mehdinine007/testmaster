namespace OrderManagement.Application.Contracts
{
    public class SubmittedAnswerDto
    {
        public long Id { get; set; }

        public int QuestionId { get; set; }

        public long? QuestionAnswerId { get; set; }

        public string CustomAnswerValue { get; set; }

        public Guid UserId { get; set; }
    }
}
