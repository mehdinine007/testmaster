using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db
{
    public class CustomerOrderDb
    {

        public int SaleDetailId { get; set; }
        public int Id { get; set; }
        public int? OrderRejectionStatus { get; set; }
        public int OrderStatus { get; set; }
        public string DeliveryDateDescription { get; set; }
        public PriorityEnum? PriorityId { get; set; }
        public string CreationTime { get; set; }
        public int EsaleTypeId { get; set; }
        public string CarName { get; set; }
        public string CompanyName { get; set; }
        public string LastModificationTime { get; set; }
    }
}
