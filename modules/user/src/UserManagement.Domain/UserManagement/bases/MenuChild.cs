using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.bases
{
    public class MenuChild
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public List<PermissionDefinitionChild> Permissions { get; set; }

        public List<MenuChild> Children { get; set; }
    }
}
