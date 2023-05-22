using System.ComponentModel.DataAnnotations;

namespace PaymentManagement.Application.Contracts
{
    public class HandShakeInputDto
    {
        public int PspAccountId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public string CallBackUrl { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public int FilterParam { get; set; }
    }
}