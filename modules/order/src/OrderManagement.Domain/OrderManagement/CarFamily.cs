using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CarFamily : FullAuditedEntity<int>
    {
        private ICollection<CarType> _carTypes;

        public string Title { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; protected set; }

        public ICollection<CarType> CarTypes
        {
            get => _carTypes ?? (_carTypes = new List<CarType>());
            protected set => _carTypes = value;
        }
    }

}
