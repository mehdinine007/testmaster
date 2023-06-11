﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.42.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name = "ServiceSoap", Namespace = "http://tempuri.org/")]
public partial class Service : Microsoft.Web.Services3.WebServicesClientProtocol
{


    /// <remarks/>
    public Service()
    {


        this.Url = "http://bos.bpm.bankmellat.ir/backoffice2/Services/bpm/TransactionService.asmx";

    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getTransactionById", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string getTransactionById(long referenceId)
    {
        object[] results = this.Invoke("getTransactionById", new object[1] { referenceId });

        return ((string)(results[0]));
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getTransactionByBillIdAndPayId", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string getTransactionByBillIdAndPayId(long billId, long payId)
    {
        object[] results = this.Invoke("getTransactionByBillIdAndPayId", new object[2] { billId, payId });
        return ((string)(results[0]));
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getTransactionByCustomerId", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string getTransactionByCustomerId(long customerId, int fromDate, int toDate, long fromRecordId)
    {
        object[] results = this.Invoke("getTransactionByCustomerId", new object[4] { customerId, fromDate, toDate, fromRecordId });
        return ((string)(results[0]));
    }


    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getTransactionByDate", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string getTransactionByDate(long TerminalId, int FromDate, int ToDate)
    {
        object[] results = this.Invoke("getTransactionByDate", new object[3] { TerminalId, FromDate, ToDate });
        return ((string)(results[0]));
    }

    //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getOnlineTransactionByDate", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    //public string getOnlineTransactionByDate(long terminalId, long afterRowNumber, int date)
    //{
    //    object[] results = this.Invoke("getOnlineTransactionByDate", new object[3] { terminalId, afterRowNumber, date });
    //    return ((string)(results[0]));
    //}

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getOnlineTransactionByDate", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    [return: System.Xml.Serialization.XmlElementAttribute("getOnlineTransactionByDate")]
    public string getOnlineTransactionByDate(long terminalId, int date, long afterRowNumber)
    {
        object[] results = this.Invoke("getOnlineTransactionByDate", new object[3] { terminalId,
                        date,
                        afterRowNumber });
        return ((string)(results[0]));
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bpmellat.co/getTransactionStatusByTerminalIdAndOrderId", RequestNamespace = "http://bpmellat.co/", ResponseNamespace = "http://bpmellat.co/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string getTransactionStatusByTerminalIdAndOrderId(long terminalId, long orderId)
    {
        object[] results = this.Invoke("getTransactionStatusByTerminalIdAndOrderId", new object[2] { terminalId, orderId });
        return this.ResponseSoapContext.Envelope.InnerText;
    }
}
