namespace OrderManagement.Application.Contracts
{
    public class SubmiteAnswerDto
    {
        public int AnswerId { get; set; }

        public string AnswerDescription { get; set; }

        public long UserId { get; set; }

        public int Id { get; set; }
    }
}
