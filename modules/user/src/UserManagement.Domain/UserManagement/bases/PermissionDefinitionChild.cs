using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.bases
{
    public class PermissionDefinitionChild
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public List<PermissionDefinitionChild> Children { get; set; }
    }
}
