using JetBrains.Annotations;

namespace PaymentManagement.Application.Contracts.Dtos
{
    public class PaymentLogDto
    {
        public int PaymentId { get; set; }
        [NotNull]
        public string Psp { get; set; }
        public string Message { get; set; }
        public string? Parameter { get; set; }
    }
}
