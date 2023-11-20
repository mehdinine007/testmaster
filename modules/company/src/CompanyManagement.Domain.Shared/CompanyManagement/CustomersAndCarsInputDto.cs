using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Domain.Shared.CompanyManagement
{
    public class CustomersAndCarsInputDto
    {
        public int SaleId { get; set; }
        public int CompanyId { get; set; }
        [Required]
        public int PageNo { get; set; }
    }
}
