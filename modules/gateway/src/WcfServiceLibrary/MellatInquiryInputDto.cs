using System.Runtime.Serialization;

namespace WcfServiceLibrary
{
    [DataContract]
    public class MellatInquiryInputDto
    {
        [DataMember]
        public long TerminalId { get; set; }
        [DataMember]
        public string ReportServiceUserName { get; set; }
        [DataMember]
        public string ReportServicePassword { get; set; }
        [DataMember]
        public long OrderId { get; set; }
        [DataMember]
        public int Switch { get; set; }
    }
}
