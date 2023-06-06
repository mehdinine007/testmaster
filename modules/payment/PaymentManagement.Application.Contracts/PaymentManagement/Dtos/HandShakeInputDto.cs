using System.ComponentModel.DataAnnotations;

namespace PaymentManagement.Application.Contracts.Dtos
{
    public class HandShakeInputDto
    {
        public int PspAccountId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public string CallBackUrl { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string AdditionalData { get; set; }
        public int? FilterParam1 { get; set; }
        public int? FilterParam2 { get; set; }
        public int? FilterParam3 { get; set; }
        public int? FilterParam4 { get; set; }

    }
}