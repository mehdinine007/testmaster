namespace OrderManagement.Application.Contracts
{
    public class BankDto
    {
        public int Id { get; set; }

        public string LogoUrl { get; set; }

        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
        public int Priority { get; set; }
    }
}