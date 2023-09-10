using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.bases;

namespace UserManagement.Application.Contracts.UserManagement.UserDto
{
    public class PermissionDefinitionChildDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public List<PermissionDefinitionChildDto> Children { get; set; }
    }
}
