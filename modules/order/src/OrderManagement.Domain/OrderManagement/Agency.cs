using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Agency : FullAuditedEntity<int>
    {
        private ICollection<AgencySaleDetail> _agencySaleDetails;

        public string Name { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<AgencySaleDetail> AgencySaleDetails
        {
            get => _agencySaleDetails ?? (_agencySaleDetails = new List<AgencySaleDetail>());
            protected set => _agencySaleDetails = value;
        }
    }
}
