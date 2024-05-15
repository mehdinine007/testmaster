using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Agency : FullAuditedEntity<int>
    {
        private ICollection<AgencySaleDetail> _agencySaleDetails;

        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool Visible { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public AgencyTypeEnum AgencyType { get; set; }
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
       
        public virtual ICollection<AgencySaleDetail> AgencySaleDetails
        {
            get => _agencySaleDetails ?? (_agencySaleDetails = new List<AgencySaleDetail>());
            protected set => _agencySaleDetails = value;
        }
    }
}
