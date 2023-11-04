using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared
{
    public enum AttachmentLocationEnum
    {
        [EnumProperty(Description = "نامشخص")]
        None = 0,
        [EnumProperty(Description = "بالای صفحه")]
        TopPage = 1,    
    }
}
