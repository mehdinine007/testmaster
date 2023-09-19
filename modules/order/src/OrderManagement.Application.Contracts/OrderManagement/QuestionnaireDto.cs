namespace OrderManagement.Application.Contracts
{
    public class QuestionnaireDto
    {
        public string QuestiuonTitle { get; set; }

        public int AnswerComponentId { get; set; }

        public int Id { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }
    }
}
