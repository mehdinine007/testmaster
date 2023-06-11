using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Net;
using System.ServiceModel;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            try
            {
                //var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
                //basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
               // Microsoft.Web.Services3.Design.UsernameTokenProvider
                //ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                //Service serviceProxy = new Service();
                //UsernameToken token = new UsernameToken("irk13", "25279378");
                //serviceProxy.SetClientCredential(token);
                //serviceProxy.SetPolicy("ClientPolicy");
              
                //var s0 = serviceProxy.getTransactionStatusByTerminalIdAndOrderId(80000294, 12536620);
                //var s1 = serviceProxy.getTransactionStatusByTerminalIdAndOrderId(80012408, 12539314);
                //var s2 = serviceProxy.getTransactionStatusByTerminalIdAndOrderId(80012408, 12539391);
                //var s3 = serviceProxy.getTransactionStatusByTerminalIdAndOrderId(80001994, 12541205);

                return string.Format("You entered: {0}", value);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
