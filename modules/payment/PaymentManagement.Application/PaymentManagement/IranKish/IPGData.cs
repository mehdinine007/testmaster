using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Application.IranKish
{
    public partial class IPGData
    {
        private string treminalId;

        private string acceptorId;

        private string passPhrase;

        private string revertURL;

        private string rsaPublicKey;

        private long amount;

        private string paymentId;

        private string requestId;

        private string cmsPreservationId;

        //private string natioanlId;
        private BillInfo billInfo;
        private List<MultiplexParameter> multiplexParameters;
        private string transactionType;
        public string RsaPublicKey { get; set; }//{ get => rsaPublicKey; set => rsaPublicKey = value; }
        public string TreminalId { get; set; }//{ get => treminalId; set => treminalId = value; }
        public string AcceptorId { get; set; }//{ get => acceptorId; set => acceptorId = value; }
        public string PassPhrase { get; set; }//{ get => passPhrase; set => passPhrase = value; }
        public string RevertURL { get; set; }//{ get => revertURL; set => revertURL = value; }
        public long Amount { get; set; }//{ get => amount; set => amount = value; }
        public string PaymentId { get; set; }//{ get => paymentId; set => paymentId = value; }
        public string CmsPreservationId { get; set; }//{ get => cmsPreservationId; set => cmsPreservationId = value; }
        public string TransactionType { get; set; }//{ get => transactionType; set => transactionType = value; }
        public BillInfo BillInfo { get; set; }//{ get => billInfo; set => billInfo = value; }
        public List<MultiplexParameter> MultiplexParameters { get; set; }//{ get => multiplexParameters; set => multiplexParameters = value; }
        public string RequestId { get; set; }//{ get => requestId; set => requestId = value; }
        public string NationalId { get; set; }
        public additionalParameters addParams { get; set; }
    }
    public class additionalParameters
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class MultiplexParameter
    {
        public string Iban { get; set; }
        public int Amount { get; set; }
    }
    public class BillInfo
    {
        public string BillId { get; set; }
        public string billPaymentId { get; set; }
    }
    public struct TransactionType
    {
        public const string Purchase = "Purchase";
        public const string Bill = "Bill";

    }
}
