using System.Xml.Serialization;

namespace PaymentManagement.Application.Mellat
{
    public class MellatXMLOutputDto
    {
        public string Id { get; set; }
        public string customerId { get; set; }
        public string accountNumber { get; set; }
        public string terminalId { get; set; }
        public string channelType { get; set; }
        public string internalReferenceId { get; set; }
        public string encryptedRefrenceId { get; set; }//
        public string transactionType { get; set; }
        public string transactionDate { get; set; }
        public string transactionTime { get; set; }
        public decimal amount { get; set; }
        public string originalAmount { get; set; }//
        public string pan { get; set; }
        public string payerId { get; set; }
        public string billId { get; set; }
        public string paymentId { get; set; }
        public int saleOrderId { get; set; }
        public string additionalData { get; set; }
        public string settlementDate { get; set; }
        public string settlementTime { get; set; }
        public string cardType { get; set; }//
        public string transactionStatus { get; set; }
        public string offerPercent { get; set; }
        public string offerGroup { get; set; }
        public string CHID { get; set; }
    }

    [XmlRoot(ElementName = "field")]
    public class Field
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "record")]
    public class Record
    {
        [XmlElement(ElementName = "field")]
        public List<Field> Field { get; set; }
    }

    [XmlRoot(ElementName = "response")]
    public class Response
    {
        [XmlElement(ElementName = "record")]
        public List<Record> Record { get; set; }
    }
}