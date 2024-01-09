using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AdvertisementDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int AdvertisementId { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
        public int Priority { get; set; }
        public  DateTime CreationTime { get; set; }

    }
}
