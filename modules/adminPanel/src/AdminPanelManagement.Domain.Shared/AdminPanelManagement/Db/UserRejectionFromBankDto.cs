using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db
{
    public class UserRejectionFromBankDto
    {
        public int Id { get; set; }
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public int? BanksId { get; set; }
        //public virtual bank Banks { get; set; }
        public string CarMaker { get; set; }
    }
}
