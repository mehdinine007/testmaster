using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Agency : FullAuditedEntity<int>
    {
        private ICollection<Agency_SaleDetail_Map> _agencySaleDetailMaps;

        public string Name { get; set; }

        public int CityId { get; set; }

        public int SaleDetailId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Agency_SaleDetail_Map> Agency_SaleDetail_Maps
        {
            get => _agencySaleDetailMaps ?? (_agencySaleDetailMaps = new List<Agency_SaleDetail_Map>());
            protected set => _agencySaleDetailMaps = value;
        }
    }
}
