using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain
{
    public class Bank : Entity<int>
    {
        private ICollection<AdvocacyUsersFromBank> _advocacyUsersFromBanks;

        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public int? LogoId { get; set; }
        public virtual ICollection<AdvocacyUsersFromBank> AdvocacyUsersFromBank 
        {
            get => _advocacyUsersFromBanks ?? (_advocacyUsersFromBanks = new List<AdvocacyUsersFromBank>());
            set => _advocacyUsersFromBanks = value;
        }
        public virtual Gallery Gallery { get; set; }
    }
}
