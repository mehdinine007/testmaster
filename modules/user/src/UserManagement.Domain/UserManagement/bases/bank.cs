using Abp.Domain.Entities;
using System.Collections.Generic;
using UserManagement.Domain.Authorization.Users;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class bank : Entity<int>
    {
        public string Title { get; set; }
        public virtual ICollection<User> User { get; set; }
        public int? LogoId { get; set; }
        public virtual ICollection<Advocacy.AdvocacyUsersFromBank> AdvocacyUsersFromBank { get; set; }
        public virtual Gallery Gallery { get; set; }
    }

}
