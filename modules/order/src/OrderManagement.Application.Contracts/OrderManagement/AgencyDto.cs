using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.Application.Contracts
{
    public class AgencyDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProvinceId { get; set; }
        public string ProvinceTitle { get; set; }
        public string AgencyCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool Visible { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int CityId { get; set; }
        public string CityIdTitle { get; set; }
        public AgencyTypeEnum AgencyType { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }

    }
}