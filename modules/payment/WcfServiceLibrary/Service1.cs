using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Net;
using System.ServiceModel;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string MellatGetTransactionStatusByTerminalIdAndOrderId(MellatInputDto input)
        {
            //"irk13", "25279378", 80000294, 12536620

            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            Service serviceProxy = new Service();
            UsernameToken token = new UsernameToken(input.UserName, input.Password, PasswordOption.SendPlainText);
            serviceProxy.SetClientCredential(token);
            serviceProxy.SetPolicy("ClientPolicy");
            return serviceProxy.getTransactionStatusByTerminalIdAndOrderId(input.TerminalId, input.PaymentId);
        }
    }
}
