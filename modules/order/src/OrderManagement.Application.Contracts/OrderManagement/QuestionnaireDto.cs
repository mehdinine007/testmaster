using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class QuestionnaireDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int? WhitListRequirement { get; set; }

        public int Id { get; set; }

        public RelatedEntityType? RelatedEntity { get; set; }

        public long? RelatedEntityId { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }
    }
}
