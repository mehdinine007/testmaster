using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Dto
{
    public class ClientOrderDetailDto
    {
        public DateTime? DeliveryDate { get; set; }
        public DateTime? FactorDate { get; set; }
        public DateTime? IntroductionDate { get; set; }
        public int? SaleType { get; set; }
        public string CarCode { get; set; }
        public string CarDesc { get; set; }
        public int? ModelType { get; set; }
        public List<PaypaidPriceDto> PaypaidPrice { get; set; }
        public List<TurnDateDto> TurnDate { get; set; }

    }
}
