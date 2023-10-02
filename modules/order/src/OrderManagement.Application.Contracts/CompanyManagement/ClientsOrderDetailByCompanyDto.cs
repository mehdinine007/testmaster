using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OrderManagement.Application.Contracts.CompanyManagement
{
    public class ClientsOrderDetailByCompanyDto : Entity<long>
    {
        public string NationalCode { get; set; }

        public int? SaleType { get; set; }

        public int? ModelType { get; set; }


        public DateTime? InviteDate { get; set; }

        public string ContRowId { get; set; }

        public DateTime? ContRowIdDate { get; set; }

        public string Vin { get; set; }

        public string BodyNumber { get; set; }

        public decimal? CooperateBenefit { get; set; }

        public decimal? CancelBenefit { get; set; }

        public decimal? DelayBenefit { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? FinalPrice { get; set; }

        public string CarDesc { get; set; }
        public string CarCode { get; set; }
        public DateTime? IntroductionDate { get; set; }
        public DateTime? FactorDate { get; set; }
        public virtual ICollection<PaypaidpriceDto> Paypaidprice { get; set; }
        public virtual ICollection<TurnDateDto> TurnDate { get; set; }
        //public List<PaypaidpriceDto>? Paypaidprice { get;set; }
        //public List<turnDateDto>? turnDate { get; set; }

    }

}
