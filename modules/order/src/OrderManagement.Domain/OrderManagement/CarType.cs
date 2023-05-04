using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CarType : FullAuditedAggregateRoot<int>
    {
        private ICollection<CarTip> _carTips;

        public string Title { get; set; }

        public int CarFamilyId { get; set; }

        public CarFamily CarFamily { get; set; }

        public virtual ICollection<CarTip> CarTips
        {
            get => _carTips ?? (_carTips = new List<CarTip>());
            protected set => _carTips = value;
        }
    }

}
