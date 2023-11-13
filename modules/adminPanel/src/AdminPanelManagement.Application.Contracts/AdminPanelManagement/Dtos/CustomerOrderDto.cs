using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos
{
    public class CustomerOrderDto
    {
        public int Id { get; set; }
        public int SaleDetailId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderRejectionStatus { get; set; }
        public string CompanyName { get; set; }
        public string CarName { get; set; }
        public string EsaleType { get; set; }
        public int? PriorityId { get; set; }
        public string OrderRegistrationPersianDate { get; set; }
        public string DeliveryDateDescription { get; set; }
        public string LastModificationTime { get; set; }
    }
}
