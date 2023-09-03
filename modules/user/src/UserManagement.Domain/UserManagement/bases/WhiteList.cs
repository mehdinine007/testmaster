using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class WhiteList : FullAuditedEntity<int>
    {
        public string NationalCode { get; set; }
        public  WhiteListEnumType WhiteListType { get; set; }
    }
    
    public enum WhiteListEnumType
    {
        WhiteListBeforeLogin = 1,
        WhiteListOrder = 2,
       
    }
}
