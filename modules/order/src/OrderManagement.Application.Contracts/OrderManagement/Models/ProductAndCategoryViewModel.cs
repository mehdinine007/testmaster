using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Models
{
    public class ProductAndCategoryViewModel
    {

        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public int LevelId { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationDto Organization { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }


    }
}
