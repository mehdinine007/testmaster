using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Contracts.UserManagement.Models.User
{
    public class UserRoleDto
    {
        public ObjectId userid { get; set; }
        public List<string> roleCode { get; set; }
    }
}
