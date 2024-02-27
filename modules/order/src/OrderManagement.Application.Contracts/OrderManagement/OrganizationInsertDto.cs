using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OrganizationInsertDto
    {
       
        public string Title { get; set; }
        public string Url { get; set; }
        public string EncryptKey { get; set; }
        public string SupportingPhone { get; set; }
        public string UrlSite { get; set; }
        public bool IsActive { get; set; }
    }
}
