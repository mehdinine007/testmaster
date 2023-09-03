using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class CarFamily : FullAuditedAggregateRoot<int>
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
