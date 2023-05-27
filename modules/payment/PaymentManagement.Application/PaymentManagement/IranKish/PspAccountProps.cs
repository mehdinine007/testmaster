using PaymentManagement.Domain.Models;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace PaymentManagement.Application.IranKish
{
    public class PspAccountProps
    {
        public string TerminalId { get; set; }
        public string AcceptorId { get; set; }
        public string PassPhrase { get; set; }
        public string RsaPublicKey { get; set; }
    }
}
