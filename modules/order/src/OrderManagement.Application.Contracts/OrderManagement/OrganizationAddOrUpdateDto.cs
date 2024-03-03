using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OrganizationAddOrUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string EncryptKey { get; set; }
        public string SupportingPhone { get; set; }
        public string UrlSite { get; set; }
        public bool IsActive { get; set; }
    }
}
