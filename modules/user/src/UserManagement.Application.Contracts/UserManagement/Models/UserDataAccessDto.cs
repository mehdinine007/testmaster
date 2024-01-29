using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Shared.UserManagement.Enums;

namespace UserManagement.Application.Contracts.UserManagement.Models
{
    public class UserDataAccessDto
    {
        public Guid UserId { get; set; }
        public string Nationalcode { get; set; }
        public RoleTypeEnum RoleTypeId { get; set; }
        public string Data { get; set; }
    }
}
