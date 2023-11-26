using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OrganizationInsertDto
    {
       
        public int Code { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string EncryptKey { get; set; }
    }
}
