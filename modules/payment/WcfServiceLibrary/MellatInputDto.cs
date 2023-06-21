using System.Runtime.Serialization;

namespace WcfServiceLibrary
{
    [DataContract]
    public class MellatInputDto
    {
        [DataMember]
        public int Switch { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public long TerminalId { get; set; }
        [DataMember]
        public long PaymentId { get; set; }
    }
}
