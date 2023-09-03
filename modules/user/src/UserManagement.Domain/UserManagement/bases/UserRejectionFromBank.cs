using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class UserRejectionFromBank : FullAuditedAggregateRoot<int>
    {
        [Required]
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public int? BanksId { get; set; }
        public virtual bank Banks { get; set; }
        public string CarMaker { get; set; }
    }
}
