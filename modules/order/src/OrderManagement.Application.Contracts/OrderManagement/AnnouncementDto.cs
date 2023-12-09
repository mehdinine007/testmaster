using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Notice { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CompanyId { get; set; }
        public string? AttachmentType { get; set; } = null;
        public string? AttachmentLocation { get; set; } = null;
        public List<AttachmentViewModel> Attachments { get; set; }
    }
}
