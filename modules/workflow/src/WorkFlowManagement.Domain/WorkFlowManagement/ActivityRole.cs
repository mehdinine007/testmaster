using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    [Table("ActivityRoles", Schema = "Flow")]
    public class ActivityRole: FullAuditedEntity<int>
    {
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }




    }
}
