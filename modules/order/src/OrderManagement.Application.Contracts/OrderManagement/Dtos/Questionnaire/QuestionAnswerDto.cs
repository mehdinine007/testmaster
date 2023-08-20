namespace OrderManagement.Application.Contracts
{
    public class QuestionAnswerDto
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public string Hint { get; set; }

        public int Value { get; set; }

        public string CustomValue { get; set; }

        public int QuestionId { get; set; }

        public int? QuestionnaireId { get; set; }
    }
}
