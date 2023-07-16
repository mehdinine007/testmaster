using Microsoft.Web.Services3.Security.Tokens;
using System.Net;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string MellatGetTransactionStatusByTerminalIdAndOrderId(MellatInquiryInputDto input)
        {
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            UsernameToken token = new UsernameToken(input.ReportServiceUserName, input.ReportServicePassword, PasswordOption.SendPlainText);

            if (input.Switch == 1)
            {
                Service serviceProxy = new Service();
                serviceProxy.SetClientCredential(token);
                serviceProxy.SetPolicy("ClientPolicy");
                return serviceProxy.getTransactionStatusByTerminalIdAndOrderId(input.TerminalId, input.OrderId).Replace("\n", "").Replace("\r", "");
            }
            else if (input.Switch == 2)//Switch = 2,"irk13", "25279378", 80000294, 12536620
            {
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                Service2 serviceProxy = new Service2();
                serviceProxy.SetClientCredential(token);
                serviceProxy.SetPolicy("ClientPolicy");
                return serviceProxy.getTransactionStatusByTerminalIdAndOrderId(input.TerminalId, input.OrderId).Replace("\n", "").Replace("\r", "");
            }

            return string.Empty;
        }
    }
}
