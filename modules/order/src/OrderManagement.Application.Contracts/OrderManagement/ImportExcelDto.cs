using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ImportExcelDto
    {
        public IFormFile File { get; set; }
        public int ProductId { get; set; }
        
    }
}
