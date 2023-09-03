using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Advocacy;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class Company : FullAuditedAggregateRoot<int>
    {
        private ICollection<CarFamily> _carFamilies;

        public string Name { get; set; }

        public int? LogoId { get; set; }

        public int? LogoInPageId { get; set; }

        public int? BannerId { get; set; }

        public virtual Gallery GalleryLogo { get; set; }

        public virtual Gallery GalleryLogoInPage { get; set; }

        public virtual Gallery GalleryBanner { get; set; }

        public bool Visible { get; set; }

        public virtual ICollection<CarFamily> CarFamilies
        {
            get => _carFamilies ?? (_carFamilies = new List<CarFamily>());
            protected set => _carFamilies = value;
        }

        public virtual ICollection<AdvocacyUsersFromBank> AdvocacyUsersFromBanks { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
