using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    [Table("Person", Schema = "Flow")]
    public class Person: FullAuditedEntity<Guid>
    {
        [Column(TypeName = "NVARCHAR(300)")]
        public string Title { get; set; }
        [Column(TypeName = "NCHAR(10)")]
        public string NationalCode { get; set; }
        public virtual ICollection<OrganizationPosition> OrganizationPositions { get; set; }

    }
}
