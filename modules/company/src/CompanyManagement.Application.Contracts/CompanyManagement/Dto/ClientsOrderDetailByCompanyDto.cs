using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Domain.Entities;

namespace CompanyManagement.Application.Contracts.CompanyManagement
{
    public class ClientsOrderDetailByCompanyDto: IValidatableObject
    {
        [Required]
        public  long Id { get;  set; }
        [Required]
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
        [Required]
        public string CarDesc { get; set; }
        [Required]
        public string CarCode { get; set; }
        [Required]
        public bool? IsCanceled { get; set; }
        public DateTime? IntroductionDate { get; set; }
        public DateTime? FactorDate { get; set; }
        public virtual ICollection<PaypaidpriceDto> Paypaidprice { get; set; }
        public virtual ICollection<TurnDateDto> TurnDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Id > 0))
            {
                yield return new ValidationResult(
                    "Id Is Required!",
                    new[] { "Id", "Required" }
                );
            }
        }
        //public List<PaypaidpriceDto>? Paypaidprice { get;set; }
        //public List<turnDateDto>? turnDate { get; set; }

    }

}
