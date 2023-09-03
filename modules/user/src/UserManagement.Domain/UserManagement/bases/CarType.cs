using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace UserManagement.Domain.UserManagement.Bases
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
