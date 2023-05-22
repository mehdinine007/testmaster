//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PaymentManagement.Application.PaymentManagement.Utilities
//{
//    #region Help:  Introduction to the script task
//    /* The Script Task allows you to perform virtually any operation that can be accomplished in
//     * a .Net application within the context of an Integration Services control flow. 
//     * 
//     * Expand the other regions which have "Help" prefixes for examples of specific ways to use
//     * Integration Services features within this script task. */
//    #endregion


//    #region Namespaces
//    using System;
//    using System.Data;
//    using Microsoft.SqlServer.Dts.Runtime;
//    using System.Windows.Forms;
//    using System.Data.SqlClient;
//    using System.Diagnostics;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Xml;
//    using ST_912aff988b0348ffa830b867ca5d2b99.GWSrv;
//    using System.Threading;
//    using global::PaymentManagement.Application.IranKish;
//    using System.Net;

//    #endregion

//    namespace ST_912aff988b0348ffa830b867ca5d2b99
//    {
//        /// <summary>
//        /// ScriptMain is the entry point class of the script.  Do not change the name, attributes,
//        /// or parent of this class.
//        /// </summary>
//        [Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
//        public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
//        {
//            #region Help:  Using Integration Services variables and parameters in a script
//            /* To use a variable in this script, first ensure that the variable has been added to 
//             * either the list contained in the ReadOnlyVariables property or the list contained in 
//             * the ReadWriteVariables property of this script task, according to whether or not your
//             * code needs to write to the variable.  To add the variable, save this script, close this instance of
//             * Visual Studio, and update the ReadOnlyVariables and 
//             * ReadWriteVariables properties in the Script Transformation Editor window.
//             * To use a parameter in this script, follow the same steps. Parameters are always read-only.
//             * 
//             * Example of reading from a variable:
//             *  DateTime startTime = (DateTime) Dts.Variables["System::StartTime"].Value;
//             * 
//             * Example of writing to a variable:
//             *  Dts.Variables["User::myStringVariable"].Value = "new value";
//             * 
//             * Example of reading from a package parameter:
//             *  int batchId = (int) Dts.Variables["$Package::batchId"].Value;
//             *  
//             * Example of reading from a project parameter:
//             *  int batchId = (int) Dts.Variables["$Project::batchId"].Value;
//             * 
//             * Example of reading from a sensitive project parameter:
//             *  int batchId = (int) Dts.Variables["$Project::batchId"].GetSensitiveValue();
//             * */

//            #endregion

//            #region Help:  Firing Integration Services events from a script
//            /* This script task can fire events for logging purposes.
//             * 
//             * Example of firing an error event:
//             *  Dts.Events.FireError(18, "Process Values", "Bad value", "", 0);
//             * 
//             * Example of firing an information event:
//             *  Dts.Events.FireInformation(3, "Process Values", "Processing has started", "", 0, ref fireAgain)
//             * 
//             * Example of firing a warning event:
//             *  Dts.Events.FireWarning(14, "Process Values", "No values received for input", "", 0);
//             * */
//            #endregion

//            #region Help:  Using Integration Services connection managers in a script
//            /* Some types of connection managers can be used in this script task.  See the topic 
//             * "Working with Connection Managers Programatically" for details.
//             * 
//             * Example of using an ADO.Net connection manager:
//             *  object rawConnection = Dts.Connections["Sales DB"].AcquireConnection(Dts.Transaction);
//             *  SqlConnection myADONETConnection = (SqlConnection)rawConnection;
//             *  //Use the connection in some code here, then release the connection
//             *  Dts.Connections["Sales DB"].ReleaseConnection(rawConnection);
//             *
//             * Example of using a File connection manager
//             *  object rawConnection = Dts.Connections["Prices.zip"].AcquireConnection(Dts.Transaction);
//             *  string filePath = (string)rawConnection;
//             *  //Use the connection in some code here, then release the connection
//             *  Dts.Connections["Prices.zip"].ReleaseConnection(rawConnection);
//             * */
//            #endregion


//            SqlConnection connection_FAVA;
//            GWSrv.BasicHttpBinding_IGatewayService client;
//            int isComplete = 0;
//            /// <summary>
//            /// This method is called when this script task executes in the control flow.
//            /// Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
//            /// To open Help, press F1.
//            /// </summary>
//            /// 

//            public void Mellat()
//            {

//                #region Mellat

//                var _payInProgress = GetMellatPayInProgress();
//                if (_payInProgress == null)
//                {
//                    return;
//                }

//                new ClsUtility().LogNew("1m- Mellat Candidate count : " + _payInProgress.Count);

//                foreach (var mellatPayment in _payInProgress)
//                {


//                    //if ((DateTime.Now - mellatPayment.CreationTime).TotalMinutes > 17 && (DateTime.Now - mellatPayment.CreationTime).TotalMinutes < 30)
//                    //{

//                    //}


//                    //if( DateTime.Now - mellatPayment.CreationTime > 17 )//&& DateTime.Now - mellatPayment.CreationTime <30)
//                    //اگر کاربر مرورگر خود را ببندد در صفحه تکميل خريد - يعني پول را داده ولي تکميل خريد نزده
//                    if (string.IsNullOrEmpty(mellatPayment.TransactionCode) || mellatPayment.TransactionCode == "0")
//                    {
//                        ApplyPaymentChangesToDB(
//                            mellatPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "MellatSSISNotVerified-TransactionCodeIsNullOrZero");

//                        new ClsUtility().LogNew("2M- Mellat Candidate TransactionCode is null or empty or 0 , paymentresultId: " + mellatPayment.PaymentResultId);
//                    }

//                    //در صورتي که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشي نباشد و ظرفيت برنامه فروش تمام شده باشد
//                    //استعلام نمي گيريم تا پس از 20 دقيقه تراکنش توسط بانک اتو ريورس شود
//                    //بنابراين وضعيت تراکنش را ناموفق مي کنيم
//                    //else if (mellatPayment.OrderId != 0 && mellatPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(mellatPayment.SaleDetailId.Value))
//                    else if (mellatPayment.OrderId != 0 && !HasCapacityBySold(mellatPayment.SaleDetailId.Value))
//                    {
//                        ApplyPaymentChangesToDB(
//                            mellatPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "MellatSSISNotVerified-SaleDetailHasNoCapacity");

//                        new ClsUtility().LogNew("3M- Mellat Candidate AutoReverse And Failed, SaleDetail has no capacity, paymentresultId: " + mellatPayment.PaymentResultId);
//                    }
//                    else
//                    {
//                        #region Verify

//                        try
//                        {
//                            string[] paramArray =
//                            {
//                                "Mellat",
//                                mellatPayment.TerminalId,
//                                mellatPayment.UserName,
//                                mellatPayment.Password,
//                                mellatPayment.PaymentResultId.ToString(),
//                                mellatPayment.PaymentResultId.ToString(),
//                                mellatPayment.TransactionCode
//                            };

//                            GWSrv.Input _input = new GWSrv.Input();
//                            _input.BankType = (GWSrv.BankType)(1);
//                            _input.Params = paramArray;

//                            new ClsUtility().LogNew("4M- Mellat Candidate try for verfiy by paymentresultId: " + mellatPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                            ApplyPaymentChangesToDB(
//                                mellatPayment.PaymentResultId,
//                                null,
//                                "MellatSSISVerifyStart",
//                                string.Join(",", paramArray));

//                            var output = client.VerifyForSSIS(paramArray);

//                            new ClsUtility().LogNew("5M- Mellat verfiy for paymentresultId: " + mellatPayment.PaymentResultId + " message : " + output.Message);

//                            ApplyPaymentChangesToDB(
//                                mellatPayment.PaymentResultId,
//                                null,
//                                "MellatSSISVerifyResult",
//                                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                            if (output.Result == Result.ResponseSuccess)
//                            {
//                                string verifyResCode = output.Message;

//                                if (verifyResCode == "0" || verifyResCode == "43")
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        mellatPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.WaittingForSend,
//                                        string.Empty);

//                                    new ClsUtility().LogNew("6m- Mellat verfiy success with verifyResCode = 0 or 43  PaymentResultId : " + mellatPayment.PaymentResultId);
//                                }

//                                else if (verifyResCode == "415")
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        mellatPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.Unknown,
//                                        string.Empty);

//                                    new ClsUtility().LogNew("7m- Mellat verify Unknown with verifyResCode = 415 PaymentResultId : " + mellatPayment.PaymentResultId);
//                                }
//                                else
//                                {
//                                    ApplyPaymentChangesToDB(
//                                       mellatPayment.PaymentResultId,
//                                       (int)Enums.PaymentResultStatus.Failed,
//                                       string.Empty);

//                                    new ClsUtility().LogNew("8m- Mellat verfiry Failed PaymentResultId : " + mellatPayment.PaymentResultId);
//                                }
//                            }
//                            else
//                            {
//                                ApplyPaymentChangesToDB(
//                                    mellatPayment.PaymentResultId,
//                                    (int)Enums.PaymentResultStatus.Unknown,
//                                    string.Empty);

//                                new ClsUtility().LogNew("9m- Mellat verify Unknown PaymentResultId : " + mellatPayment.PaymentResultId);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            new ClsUtility().LogNew("10m- Mellat verify " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                            ApplyPaymentChangesToDB(
//                                mellatPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Unknown,
//                                "MellatSSISVerifyException",
//                                ex.Message);
//                        }

//                        #endregion
//                    }
//                }

//                #endregion

//                #region MellatInquiry
//                //var _mellatInquiry = GetMellatPayInProgressForInquiry();
//                //if(_mellatInquiry == null)
//                //{
//                //    return;
//                //}
//                //new ClsUtility().LogNew("1m- MellatInquiry Candidate count : " + _mellatInquiry.Count);

//                //foreach (var mellatPayment in _mellatInquiry)
//                //{
//                //    //اگر کاربر مرورگر خود را ببندد در صفحه تکميل خريد - يعني پول را داده ولي تکميل خريد نزده
//                //    if (string.IsNullOrEmpty(mellatPayment.TransactionCode) || mellatPayment.TransactionCode == "0")
//                //    {
//                //        ApplyPaymentChangesToDB(
//                //            mellatPayment.PaymentResultId,
//                //            (int)Enums.PaymentResultStatus.Failed,
//                //            "MellatSSISNotInquiry-TransactionCodeIsNullOrZero");

//                //        new ClsUtility().LogNew("2M- Mellat inquery Candidate TransactionCode is null or empty or 0 , paymentresultId: " + mellatPayment.PaymentResultId);
//                //    }

//                //    else
//                //    {
//                //        #region Inquiry

//                //        try
//                //        {
//                //            string[] paramArray =
//                //            {
//                //                    "Mellat",
//                //                    mellatPayment.TerminalId,
//                //                    mellatPayment.UserName,
//                //                    mellatPayment.Password,
//                //                    mellatPayment.PaymentResultId.ToString(),
//                //                    mellatPayment.PaymentResultId.ToString(),
//                //                    mellatPayment.TransactionCode
//                //                };

//                //            GWSrv.Input _input = new GWSrv.Input();
//                //            _input.BankType = (GWSrv.BankType)(1);
//                //            _input.Params = paramArray;

//                //            new ClsUtility().LogNew("4M- Mellat Candidate try for Inquiry by paymentresultId: " + mellatPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                //            ApplyPaymentChangesToDB(
//                //                mellatPayment.PaymentResultId,
//                //                null,
//                //                "MellatSSISInquiryStart",
//                //                string.Join(",", paramArray));

//                //            var output = client.InquiryForSSIS(paramArray);

//                //            new ClsUtility().LogNew("5M- Mellat Inquiry for paymentresultId: " + mellatPayment.PaymentResultId + " message : " + output.Message);

//                //            ApplyPaymentChangesToDB(
//                //                mellatPayment.PaymentResultId,
//                //                null,
//                //                "MellatSSISInquiryResult",
//                //                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                //            if (output.Result == Result.ResponseSuccess)
//                //            {
//                //                string InquiryResCode = output.Message;

//                //                if (InquiryResCode == "0" || InquiryResCode == "43")
//                //                {

//                //                    if (mellatPayment.OrderId != 0 && !HasCapacityBySold(mellatPayment.SaleDetailId.Value))
//                //                    {

//                //                        ApplyPaymentChangesToDB(
//                //                            mellatPayment.PaymentResultId,
//                //                            (int)Enums.PaymentResultStatus.NoCapacity,
//                //                            "MellatSSISInquiry-SaleDetailHasNoCapacity");

//                //                        new ClsUtility().LogNew("3M- Mellat Candidate Success but NoCapacity, SaleDetail has no capacity, paymentresultId: " + mellatPayment.PaymentResultId);
//                //                    }
//                //                    else
//                //                    {
//                //                        ApplyPaymentChangesToDB(
//                //                                                mellatPayment.PaymentResultId,
//                //                                                (int)Enums.PaymentResultStatus.WaittingForSend,
//                //                                                string.Empty);

//                //                        new ClsUtility().LogNew("6m- Mellat Inquiry success with InquiryResCode = 0 or 43  PaymentResultId : " + mellatPayment.PaymentResultId);
//                //                    }


//                //                }


//                //                else
//                //                {
//                //                    ApplyPaymentChangesToDB(
//                //                       mellatPayment.PaymentResultId,
//                //                       (int)Enums.PaymentResultStatus.Failed,
//                //                       string.Empty);

//                //                    new ClsUtility().LogNew("8m- Mellat Inquiry Failed PaymentResultId : " + mellatPayment.PaymentResultId);
//                //                }
//                //            }
//                //            else
//                //            {
//                //                ApplyPaymentChangesToDB(
//                //                    mellatPayment.PaymentResultId,
//                //                    (int)Enums.PaymentResultStatus.Unknown,
//                //                    string.Empty);

//                //                new ClsUtility().LogNew("9m- Mellat Inquiry Unknown PaymentResultId : " + mellatPayment.PaymentResultId);
//                //            }
//                //        }
//                //        catch (Exception ex)
//                //        {
//                //            new ClsUtility().LogNew("10m- Mellat Inquiry " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                //            ApplyPaymentChangesToDB(
//                //                mellatPayment.PaymentResultId,
//                //                (int)Enums.PaymentResultStatus.Unknown,
//                //                "MellatSSISInquiryException",
//                //                ex.Message);
//                //        }

//                //        #endregion
//                //    }
//                //}
//                #endregion



//            }
//            public void Sadad()
//            {
//                #region Sadad
//                try
//                {
//                    var _sadadPayInProgress = GetSadadPayInProgress();
//                    if (_sadadPayInProgress == null)
//                    {

//                        return;
//                    }
//                    new ClsUtility().LogNew("1sadad- sadad Candidate count : " + _sadadPayInProgress.Count);

//                    foreach (var sadadPayment in _sadadPayInProgress)
//                    {
//                        if (string.IsNullOrEmpty(sadadPayment.Token))
//                        {
//                            ApplyPaymentChangesToDB(
//                                sadadPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Failed,
//                                "SadadSSISNotVerified-TokenIsNullOrEmpty");

//                            new ClsUtility().LogNew("2sadad- sadad Candidate Token is null or empty, paymentresultId: " + sadadPayment.PaymentResultId);
//                        }

//                        //در صورتی که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشی نباشد و ظرفیت برنامه فروش تمام شده باشد
//                        //استعلام نمی گیریم تا پس از 20 دقیقه تراکنش توسط بانک اتو ریورس شود
//                        //بنابراین وضعیت تراکنش را ناموفق می کنیم
//                        //else if (sadadPayment.OrderId != 0 && sadadPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(sadadPayment.SaleDetailId.Value))
//                        else if (sadadPayment.OrderId != 0 && !HasCapacityBySold(sadadPayment.SaleDetailId.Value))
//                        {
//                            ApplyPaymentChangesToDB(
//                                sadadPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Failed,
//                                "SadadSSISNotVerified-SaleDetailHasNoCapacity");

//                            new ClsUtility().LogNew("3sadad- sadad Candidate Failed, SaleDetail has no capacity, paymentresultId: " + sadadPayment.PaymentResultId);
//                        }
//                        else
//                        {
//                            #region "Settle"

//                            try
//                            {
//                                string[] paramArray =
//                                {
//                                "Sadad",
//                                sadadPayment.MerchantKey,
//                                sadadPayment.Token
//                            };

//                                GWSrv.Input _input = new GWSrv.Input();
//                                _input.BankType = (GWSrv.BankType)(6);
//                                _input.Params = paramArray;
//                                new ClsUtility().LogNew("4sadad- sadad Candidate try for settle by paymentresultId: " + sadadPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                                ApplyPaymentChangesToDB(
//                                    sadadPayment.PaymentResultId,
//                                    null,
//                                    "SadadSSISVerifyStart",
//                                    string.Join(",", paramArray));

//                                var output = client.SettleForSSIS(paramArray);

//                                new ClsUtility().LogNew("5sadad- sadad settle for paymentresultId: " + sadadPayment.PaymentResultId + " message : " + output.Message);

//                                ApplyPaymentChangesToDB(
//                                    sadadPayment.PaymentResultId,
//                                    null,
//                                    "SadadSSISVerifyResult",
//                                    "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                                if (output.Result == Result.ResponseSuccess)
//                                {
//                                    var result = output.Message.Split(",".ToCharArray());

//                                    var additionalData =
//                                        "<VerifyResCode>" + result[0] + "</VerifyResCode>" +
//                                        "<Description>" + result[1] + "</Description>" +
//                                        "<SystemTraceNo>" + result[3] + "</SystemTraceNo>" +
//                                        "<CardNo>" + result[4] + "</CardNo>" +
//                                        "<HashedCardNo>" + result[5] + "</HashedCardNo>";

//                                    var transactionCode = result[2];

//                                    if (result[0] == "0")
//                                    {
//                                        ApplyPaymentChangesToDB(
//                                            sadadPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.WaittingForSend,
//                                            string.Empty,
//                                            transactionCode: transactionCode,
//                                            additionalData: additionalData);

//                                        new ClsUtility().LogNew("6sadad- sadad settle success  PaymentResultId : " + sadadPayment.PaymentResultId);
//                                    }
//                                    else if (result[0] == "101")
//                                    {
//                                        ApplyPaymentChangesToDB(
//                                            sadadPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.Unknown,
//                                            string.Empty,
//                                            transactionCode: transactionCode,
//                                            additionalData: additionalData);

//                                        new ClsUtility().LogNew("7sadad- sadad settle Unknown VerifyResCode = 101, PaymentResultId : " + sadadPayment.PaymentResultId);
//                                    }
//                                    else
//                                    {
//                                        ApplyPaymentChangesToDB(
//                                            sadadPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.Failed,
//                                            string.Empty,
//                                            transactionCode: transactionCode,
//                                            additionalData: additionalData);

//                                        new ClsUtility().LogNew("8sadad- sadad Candidate VerifyResCode= " + result[0] + ", paymentresultId: " + sadadPayment.PaymentResultId);
//                                    }
//                                }
//                                else
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        sadadPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.Unknown,
//                                        string.Empty);

//                                    new ClsUtility().LogNew("9sadad- sadad settle Unknown PaymentResultId : " + sadadPayment.PaymentResultId);
//                                }
//                            }
//                            catch (Exception ex)
//                            {

//                                new ClsUtility().LogNew("10sadad- sadad settle " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                                ApplyPaymentChangesToDB(
//                                    sadadPayment.PaymentResultId,
//                                    (int)Enums.PaymentResultStatus.Unknown,
//                                    "SadadSSISVerifyException",
//                                    ex.Message);
//                            }

//                            #endregion
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {

//                }


//                #endregion

//                #region SadadInquiry

//                //var _sadadPayInProgressInquiry = GetSadadPayInProgressForInquiry();
//                //ClsUtility.Log("1sadad- sadadInquiry Candidate count : " + _sadadPayInProgressInquiry.Count);

//                //foreach (var sadadPayment in _sadadPayInProgressInquiry)
//                //{
//                //    if (string.IsNullOrEmpty(sadadPayment.Token))
//                //    {
//                //        ApplyPaymentChangesToDB(
//                //            sadadPayment.PaymentResultId,
//                //            (int)Enums.PaymentResultStatus.Failed,
//                //            "SadadSSISNotInquiry-TokenIsNullOrEmpty");

//                //        ClsUtility.Log("2sadad- sadadInquiry Candidate Token is null or empty, paymentresultId: " + sadadPayment.PaymentResultId);
//                //    }

//                //    else
//                //    {
//                //        #region "Inquiry"

//                //        try
//                //        {
//                //            string[] paramArray =
//                //            {
//                //                    "Sadad",
//                //                    sadadPayment.MerchantKey,
//                //                    sadadPayment.Token
//                //                };

//                //            GWSrv.Input _input = new GWSrv.Input();
//                //            _input.BankType = (GWSrv.BankType)(6);
//                //            _input.Params = paramArray;
//                //            ClsUtility.Log("4sadad- sadadInquiry Candidate try for settle by paymentresultId: " + sadadPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                //            ApplyPaymentChangesToDB(
//                //                sadadPayment.PaymentResultId,
//                //                null,
//                //                "SadadSSISInquiryStart",
//                //                string.Join(",", paramArray));

//                //            var output = client.InquiryForSSIS(paramArray);

//                //            ClsUtility.Log("5sadad- sadad settle for paymentresultId: " + sadadPayment.PaymentResultId + " message : " + output.Message);

//                //            ApplyPaymentChangesToDB(
//                //                sadadPayment.PaymentResultId,
//                //                null,
//                //                "SadadSSISInquiryResult",
//                //                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                //            if (output.Result == Result.ResponseSuccess)
//                //            {
//                //                var result = output.Message.Split(",".ToCharArray());

//                //                var additionalData =
//                //                    "<InquiryResCode>" + result[0] + "</InquiryResCode>" +
//                //                    "<Description>" + result[1] + "</Description>" +
//                //                    "<SystemTraceNo>" + result[3] + "</SystemTraceNo>" +
//                //                    "<CardNo>" + result[4] + "</CardNo>" +
//                //                    "<HashedCardNo>" + result[5] + "</HashedCardNo>";

//                //                var transactionCode = result[2];

//                //                if (result[0] == "0")
//                //                {

//                //                    if (sadadPayment.OrderId != 0 && !HasCapacityBySold(sadadPayment.SaleDetailId.Value))
//                //                    {
//                //                        ApplyPaymentChangesToDB(
//                //                            sadadPayment.PaymentResultId,
//                //                            (int)Enums.PaymentResultStatus.NoCapacity,
//                //                            "SadadSSISNotInquiry-SaleDetailHasNoCapacity");

//                //                        ClsUtility.Log("3sadad- sadadInquiry Success but NoCapacity, SaleDetail has no capacity, paymentresultId: " + sadadPayment.PaymentResultId);
//                //                    }
//                //                    else
//                //                    {
//                //                        ApplyPaymentChangesToDB(
//                //                        sadadPayment.PaymentResultId,
//                //                        (int)Enums.PaymentResultStatus.WaittingForSend,
//                //                        string.Empty,
//                //                        transactionCode: transactionCode,
//                //                        additionalData: additionalData);

//                //                        ClsUtility.Log("6sadad- sadad Inquiry success  PaymentResultId : " + sadadPayment.PaymentResultId);
//                //                    }

//                //                }
//                //                else if (result[0] == "101")
//                //                {
//                //                    ApplyPaymentChangesToDB(
//                //                        sadadPayment.PaymentResultId,
//                //                        (int)Enums.PaymentResultStatus.Unknown,
//                //                        string.Empty,
//                //                        transactionCode: transactionCode,
//                //                        additionalData: additionalData);

//                //                    ClsUtility.Log("7sadad- sadad Inquiry Unknown InquiryResCode = 101, PaymentResultId : " + sadadPayment.PaymentResultId);
//                //                }
//                //                else
//                //                {
//                //                    ApplyPaymentChangesToDB(
//                //                        sadadPayment.PaymentResultId,
//                //                        (int)Enums.PaymentResultStatus.Failed,
//                //                        string.Empty,
//                //                        transactionCode: transactionCode,
//                //                        additionalData: additionalData);

//                //                    ClsUtility.Log("8sadad- sadadInquiry Candidate InquiryResCode= " + result[0] + ", paymentresultId: " + sadadPayment.PaymentResultId);
//                //                }
//                //            }
//                //            else
//                //            {
//                //                ApplyPaymentChangesToDB(
//                //                    sadadPayment.PaymentResultId,
//                //                    (int)Enums.PaymentResultStatus.Unknown,
//                //                    string.Empty);

//                //                ClsUtility.Log("9sadad- sadad Inquiry Unknown PaymentResultId : " + sadadPayment.PaymentResultId);
//                //            }
//                //        }
//                //        catch (Exception ex)
//                //        {
//                //            ClsUtility.Log("10sadad- sadad Inquiry " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                //            ApplyPaymentChangesToDB(
//                //                sadadPayment.PaymentResultId,
//                //                (int)Enums.PaymentResultStatus.Unknown,
//                //                "SadadSSISInquiryException",
//                //                ex.Message);
//                //        }

//                //        #endregion
//                //    }
//                //}

//                #endregion

//            }
//            public void Parsian()
//            {
//                #region Parsian

//                var _parsianPayInProgress = GetParsianPayInProgress();
//                if (_parsianPayInProgress == null)
//                {
//                    return;
//                }
//                new ClsUtility().LogNew("1p- Parsian Candidate count : " + _parsianPayInProgress.Count);

//                foreach (var parsianPayment in _parsianPayInProgress)
//                {
//                    if (string.IsNullOrEmpty(parsianPayment.Token) || parsianPayment.Token == "0")
//                    {
//                        ApplyPaymentChangesToDB(
//                            parsianPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "ParsianSSISNotVerified-TokenIsNullOrZero");

//                        new ClsUtility().LogNew("2p- Parsian Candidate Token is null or empty or 0 , paymentresultId : " + parsianPayment.PaymentResultId);
//                    }
//                    //در صورتي که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشي نباشد و ظرفيت برنامه فروش تمام شده باشد
//                    //استعلام نمي گيريم تا پس از 30 دقيقه تراکنش توسط بانک اتو ريورس شود
//                    //بنابراين وضعيت تراکنش را ناموفق مي کنيم 
//                    //else if (parsianPayment.OrderId != 0 && parsianPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(parsianPayment.SaleDetailId.Value))
//                    else if (parsianPayment.OrderId != 0 && !HasCapacityBySold(parsianPayment.SaleDetailId.Value))
//                    {
//                        ApplyPaymentChangesToDB(
//                            parsianPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "ParsianSSISNotVerified-SaleDetailHasNoCapacity");

//                        new ClsUtility().LogNew("3p- Parsian Candidate AutoReversed and Failed, SaleDetail has no capacity, paymentresultId : " + parsianPayment.PaymentResultId);
//                    }
//                    else
//                    {
//                        #region "Settle"

//                        try
//                        {
//                            string[] paramArray =
//                                {
//                                    "Parsian",
//                                    parsianPayment.Pin,
//                                    parsianPayment.Token
//                                };

//                            new ClsUtility().LogNew("10p- Parsian Candidate try for settle by paymentresultId  : " + parsianPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                            ApplyPaymentChangesToDB(
//                                parsianPayment.PaymentResultId,
//                                null,
//                                "ParsianSSISVerifyStart",
//                                string.Join(",", paramArray));

//                            Output output = client.SettleForSSIS(paramArray);

//                            new ClsUtility().LogNew("11p- Parsian settle message : " + output.Message);

//                            ApplyPaymentChangesToDB(
//                                parsianPayment.PaymentResultId,
//                                null,
//                                "ParsianSSISVerifyResult",
//                                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                            if (output.Result == Result.ResponseSuccess)
//                            {
//                                if (!string.IsNullOrEmpty(output.Message))
//                                {
//                                    string[] result = output.Message.Split(",".ToCharArray());

//                                    //موفق بودن وضعيت تراکنش
//                                    if (result[0] == "0" || result[0] == "-1533")
//                                    {
//                                        ApplyPaymentChangesToDB(
//                                            parsianPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.WaittingForSend,
//                                            string.Empty,
//                                            transactionCode: result[1],
//                                            additionalData: "<ConfirmStatus>" + result[0] + "</ConfirmStatus><RRN>" + result[1] + "</RRN><Token>" + result[2] + "</Token><CardNumberMasked>" + result[3] + "</CardNumberMasked>");

//                                        new ClsUtility().LogNew("12p- Parsian settle success result transactionCode : " + output.Message);
//                                    }
//                                    else
//                                    {
//                                        ApplyPaymentChangesToDB(
//                                            parsianPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.Failed,
//                                            string.Empty,
//                                            additionalData: "<ConfirmStatus>" + result[0] + "</ConfirmStatus><RRN>" + result[1] + "</RRN><Token>" + result[2] + "</Token><CardNumberMasked>" + result[3] + "</CardNumberMasked>");

//                                        new ClsUtility().LogNew("13p- Parsian settle Failed paymentresultId : " + parsianPayment.PaymentResultId);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                ApplyPaymentChangesToDB(
//                                    parsianPayment.PaymentResultId,
//                                    (int)Enums.PaymentResultStatus.Unknown,
//                                    string.Empty);

//                                new ClsUtility().LogNew("14p- Parsian settle Unknown  PaymentResultId : " + parsianPayment.PaymentResultId);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            ApplyPaymentChangesToDB(
//                                parsianPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Unknown,
//                                "ParsianSSISVerifyException",
//                                ex.Message);

//                            new ClsUtility().LogNew("15p-Parsian settle Unknown Exception : " + ex.Message + ", inner Exception :  " + ex.InnerException + ", PaymentResultId : " + parsianPayment.PaymentResultId);
//                        }

//                        #endregion
//                    }
//                }

//                #endregion

//                #region ParsianInquery

//                //var _parsianPayInProgressInquery = GetParsianPayInProgressForInquiry();
//                //if (_parsianPayInProgressInquery == null)
//                //{
//                //    new ClsUtility().LogNew("1p- ParsianInquiry Candidate count : 0" );

//                //    return;
//                //}
//                //new ClsUtility().LogNew("1p- ParsianInquiry Candidate count : " + _parsianPayInProgressInquery.Count);

//                //foreach (var parsianPayment in _parsianPayInProgressInquery)
//                //{
//                //    if (string.IsNullOrEmpty(parsianPayment.Token) || parsianPayment.Token == "0")
//                //    {
//                //        ApplyPaymentChangesToDB(
//                //            parsianPayment.PaymentResultId,
//                //            (int)Enums.PaymentResultStatus.Failed,
//                //            "ParsianSSISNotInquiry-TokenIsNullOrZero");

//                //        new ClsUtility().LogNew("2p- Parsian Candidate Token is null or empty or 0 , paymentresultId : " + parsianPayment.PaymentResultId);
//                //    }

//                //    else
//                //    {
//                //        #region "Inquiry"

//                //        try
//                //        {
//                //            string[] paramArray =
//                //                {
//                //                        "Parsian",
//                //                        parsianPayment.PaymentResultId.ToString(),
//                //                        parsianPayment.Token
//                //                    };

//                //            new ClsUtility().LogNew("10p- ParsianInquiry Candidate try for Inquiry by paymentresultId  : " + parsianPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                //            ApplyPaymentChangesToDB(
//                //                parsianPayment.PaymentResultId,
//                //                null,
//                //                "ParsianSSISInquiryStart",
//                //                string.Join(",", paramArray));

//                //            Output output = client.InquiryForSSIS(paramArray);

//                //            new ClsUtility().LogNew("11p- Parsian Inquiry message : " + output.Message);

//                //            ApplyPaymentChangesToDB(
//                //                parsianPayment.PaymentResultId,
//                //                null,
//                //                "ParsianSSISInquiryResult",
//                //                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);


//                //            if (output.Result == Result.ResponseSuccess)
//                //            {
//                //                if (!string.IsNullOrEmpty(output.Message))
//                //                {
//                //                    string[] result = output.Message.Split(",".ToCharArray());

//                //                    //موفق بودن وضعيت تراکنش
//                //                    if (result[0] == "0")
//                //                    {

//                //                        if (parsianPayment.OrderId != 0 && !HasCapacityBySold(parsianPayment.SaleDetailId.Value))
//                //                        {
//                //                            ApplyPaymentChangesToDB(
//                //                                parsianPayment.PaymentResultId,
//                //                                (int)Enums.PaymentResultStatus.NoCapacity,
//                //                                "ParsianSSISInquiry-SaleDetailHasNoCapacity");

//                //                            new ClsUtility().LogNew("3p- ParsianInquiry Candidate  Success but NoCapacity, SaleDetail has no capacity, paymentresultId : " + parsianPayment.PaymentResultId);
//                //                        }

//                //                        else
//                //                        {
//                //                            ApplyPaymentChangesToDB(
//                //                          parsianPayment.PaymentResultId,
//                //                          (int)Enums.PaymentResultStatus.WaittingForSend,
//                //                          string.Empty,
//                //                          transactionCode: result[1],
//                //                          additionalData: "<Status>" + result[0] + "</Status><RRN>" + result[1] + "</RRN><Token>" + result[2] + "</Token>");

//                //                            new ClsUtility().LogNew("12p- Parsian Inquiry success result transactionCode : " + output.Message);
//                //                        }


//                //                    }
//                //                    else
//                //                    {
//                //                        ApplyPaymentChangesToDB(
//                //                            parsianPayment.PaymentResultId,
//                //                            (int)Enums.PaymentResultStatus.Failed,
//                //                            string.Empty,
//                //                            additionalData: "<ConfirmStatus>" + result[0] + "</ConfirmStatus><RRN>" + result[1] + "</RRN><Token>" + result[2] + "</Token>");

//                //                        new ClsUtility().LogNew("13p- Parsian Inquiry Failed paymentresultId : " + parsianPayment.PaymentResultId);
//                //                    }
//                //                }
//                //            }
//                //            else
//                //            {
//                //                ApplyPaymentChangesToDB(
//                //                    parsianPayment.PaymentResultId,
//                //                    (int)Enums.PaymentResultStatus.Unknown,
//                //                    string.Empty);

//                //                new ClsUtility().LogNew("14p- Parsian Inquiry Unknown  PaymentResultId : " + parsianPayment.PaymentResultId);
//                //            }
//                //        }
//                //        catch (Exception ex)
//                //        {
//                //            ApplyPaymentChangesToDB(
//                //                parsianPayment.PaymentResultId,
//                //                (int)Enums.PaymentResultStatus.Unknown,
//                //                "ParsianSSISInquiryException",
//                //                ex.Message);

//                //            new ClsUtility().LogNew("15p-Parsian Inquiry Unknown Exception : " + ex.Message + ", inner Exception :  " + ex.InnerException + ", PaymentResultId : " + parsianPayment.PaymentResultId);
//                //        }

//                //        #endregion
//                //    }
//                //}

//                #endregion
//            }
//            public void Saman()
//            {
//                #region Saman
//                try
//                {
//                    var _samanPayInProgress = GetSamanPayInProgress();
//                    if (_samanPayInProgress == null)
//                    {
//                        return;
//                    }
//                    new ClsUtility().LogNew("1s- saman Candidate count : " + _samanPayInProgress.Count);

//                    foreach (var samanPayment in _samanPayInProgress)
//                    {
//                        //اگر کاربر مرورگر خود را ببندد در صفحه تکمیل خرید - یعنی پول را داده ولی تکمیل خرید نزده
//                        if (string.IsNullOrEmpty(samanPayment.RefNum))
//                        {
//                            ApplyPaymentChangesToDB(
//                                samanPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Failed,
//                                "SamanSSISNotVerified-RefNumIsNullOrEmpty");

//                            new ClsUtility().LogNew("2s- saman Candidate RefNum is null or empty, paymentresultId: " + samanPayment.PaymentResultId);
//                        }

//                        //در صورتی که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشی نباشد و ظرفیت برنامه فروش تمام شده باشد
//                        //استعلام نمی گیریم تا پس از 30 دقیقه تراکنش توسط بانک اتو ریورس شود
//                        //بنابراین وضعیت تراکنش را ناموفق می کنیم
//                        //else if (samanPayment.OrderId != 0 && samanPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(samanPayment.SaleDetailId.Value))
//                        else if (samanPayment.OrderId != 0 && !HasCapacityBySold(samanPayment.SaleDetailId.Value))
//                        {
//                            ApplyPaymentChangesToDB(
//                               samanPayment.PaymentResultId,
//                               (int)Enums.PaymentResultStatus.Failed,
//                               "SamanSSISNotVerified-SaleDetailHasNoCapacity");

//                            new ClsUtility().LogNew("3s- saman Candidate Failed, SaleDetail has no capacity, paymentresultId: " + samanPayment.PaymentResultId);
//                        }
//                        else
//                        {
//                            #region "Settle"

//                            try
//                            {
//                                string[] paramArray =
//                                {
//                                "Saman",
//                                samanPayment.RefNum,
//                                samanPayment.MID
//                            };

//                                GWSrv.Input _input = new GWSrv.Input();
//                                _input.BankType = (GWSrv.BankType)(5);
//                                _input.Params = paramArray;
//                                new ClsUtility().LogNew("4s- saman Candidate try for settle by paymentresultId: " + samanPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                                ApplyPaymentChangesToDB(
//                                    samanPayment.PaymentResultId,
//                                    null,
//                                    "SamanSSISVerifyStart",
//                                    string.Join(",", paramArray));

//                                var output = client.SettleForSSIS(paramArray);

//                                new ClsUtility().LogNew("5s- saman settle for paymentresultId: " + samanPayment.PaymentResultId + " message : " + output.Message);

//                                ApplyPaymentChangesToDB(
//                                    samanPayment.PaymentResultId,
//                                    null,
//                                    "SamanSSISVerifyResult",
//                                    "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                                if (output.Result == Result.ResponseSuccess)
//                                {
//                                    //باید چك شود كه مبلغ دریافتی از بانك با مبلغ ركورد پرداخت یكی باشد
//                                    decimal result = decimal.Parse(output.Message);
//                                    if (samanPayment.PaidPrice == result)
//                                    {
//                                        ApplyPaymentChangesToDB(
//                                            samanPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.WaittingForSend,
//                                            string.Empty);

//                                        new ClsUtility().LogNew("6s- saman settle success  PaymentResultId : " + samanPayment.PaymentResultId);
//                                    }
//                                    else
//                                    {
//                                        var res = GetVerifyResultErrorMessage((int)result);

//                                        ApplyPaymentChangesToDB(
//                                            samanPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.Failed,
//                                            string.Empty,
//                                            additionalData: "<VerifyResult>" + result + "</VerifyResult>" + "<VerifyMessage>" + res + "</VerifyMessage>");

//                                        new ClsUtility().LogNew("7s- saman settle Failed with result: " + result + " and resultMessage: " + res + " PaymentResultId : " + samanPayment.PaymentResultId);
//                                    }
//                                }
//                                else
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        samanPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.Unknown,
//                                        string.Empty);

//                                    new ClsUtility().LogNew("8s- saman settle Unknown PaymentResultId : " + samanPayment.PaymentResultId);
//                                }
//                            }
//                            catch (Exception ex)
//                            {

//                                new ClsUtility().LogNew("9s- saman settle " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                                ApplyPaymentChangesToDB(
//                                    samanPayment.PaymentResultId,
//                                    (int)Enums.PaymentResultStatus.Unknown,
//                                    "SamanSSISVerifyException",
//                                    ex.Message);
//                            }

//                            #endregion
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                }


//                #endregion
//            }
//            public void Pasargad()
//            {
//                #region Pasargad

//                var _pasargadPayInProgress = GetPasargadPayInProgress();

//                new ClsUtility().LogNew("1pas- Pasargad Candidate count : " + _pasargadPayInProgress.Count);

//                foreach (var pasargadPayment in _pasargadPayInProgress)
//                {
//                    //در صورتی که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشی نباشد و ظرفیت برنامه فروش تمام شده باشد
//                    //استعلام نمی گیریم تا پس از 10 دقیقه تراکنش توسط بانک اتو ریورس شود
//                    //بنابراین وضعیت تراکنش را ناموفق می کنیم
//                    //if (pasargadPayment.OrderId != 0 && pasargadPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(pasargadPayment.SaleDetailId.Value))
//                    if (pasargadPayment.OrderId != 0 && !HasCapacityBySold(pasargadPayment.SaleDetailId.Value))
//                    {
//                        ApplyPaymentChangesToDB(
//                            pasargadPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "PasargadSSISNotVerified-SaleDetailHasNoCapacity");

//                        new ClsUtility().LogNew("2pas- Pasargad Candidate AutoReverse and Failed, SaleDetail has no capacity, paymentresultId : " + pasargadPayment.PaymentResultId);
//                    }
//                    else
//                    {
//                        #region Inquiry

//                        try
//                        {
//                            string[] inquiryParams =
//                            {
//                                "Pasargad",
//                                pasargadPayment.PaymentResultId.ToString(),
//                                pasargadPayment.InvoiceDate,
//                                pasargadPayment.PaidPrice.ToString(),
//                                pasargadPayment.Merchant,
//                                pasargadPayment.Terminal,
//                                pasargadPayment.PrivateKey
//                            };
//                            GWSrv.Input _input = new GWSrv.Input();
//                            _input.Params = inquiryParams;
//                            new ClsUtility().LogNew("3pas- Pasargad Candidate try for inquiry by paymentresultId  : " + pasargadPayment.PaymentResultId + " params : " + string.Join(",", inquiryParams));

//                            ApplyPaymentChangesToDB(
//                                 pasargadPayment.PaymentResultId,
//                                 null,
//                                 "PasargadSSISInquiryStart",
//                                 string.Join(",", inquiryParams));

//                            var output = client.InquiryForSSIS(inquiryParams);

//                            new ClsUtility().LogNew("4pas- Pasargad inquiry paymentresultId: " + pasargadPayment.PaymentResultId + " message : " + output.Message);

//                            ApplyPaymentChangesToDB(
//                                pasargadPayment.PaymentResultId,
//                                null,
//                                "PasargadSSISInquiryResult",
//                                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                            if (output.Result == Result.ResponseSuccess)
//                            {
//                                if (!string.IsNullOrEmpty(output.Message))
//                                {
//                                    XmlDocument oXml = new XmlDocument();
//                                    oXml.LoadXml(output.Message);

//                                    var result = bool.Parse(oXml.SelectSingleNode("//result").InnerText);
//                                    var tref = oXml.SelectSingleNode("//transactionReferenceID").InnerText;

//                                    if (result && (!string.IsNullOrEmpty(tref) || !tref.Equals("0")))
//                                    {
//                                        #region Verify

//                                        try
//                                        {
//                                            string[] verfiyParams =
//                                            {
//                                                "Pasargad",
//                                                tref
//                                            };
//                                            GWSrv.Input _inputVerify = new GWSrv.Input();
//                                            _inputVerify.Params = verfiyParams;
//                                            new ClsUtility().LogNew("5pas- Pasargad Candidate try for verify by paymentresultId  : " + pasargadPayment.PaymentResultId + " params : " + string.Join(",", verfiyParams));

//                                            ApplyPaymentChangesToDB(
//                                                  pasargadPayment.PaymentResultId,
//                                                  null,
//                                                  "PasargadSSISVerifyStart",
//                                                  string.Join(",", verfiyParams));

//                                            var outputVerify = client.VerifyForSSIS(verfiyParams);

//                                            new ClsUtility().LogNew("6pas- Pasargad verify paymentresultId: " + pasargadPayment.PaymentResultId + " message : " + outputVerify.Message);

//                                            ApplyPaymentChangesToDB(
//                                                pasargadPayment.PaymentResultId,
//                                                null,
//                                                "PasargadSSISVerifyResult",
//                                                "Result:" + outputVerify.Result.ToString() + "-" + "Message:" + outputVerify.Message);

//                                            //در صورتی که تراکنشی انجام نشده باشد فایلی از بانک برگشت داده نمی شود 
//                                            if (outputVerify.Result == Result.ResponseSuccess)
//                                            {
//                                                if (!string.IsNullOrEmpty(outputVerify.Message))
//                                                {
//                                                    XmlDocument oXmlVerify = new XmlDocument();
//                                                    oXmlVerify.LoadXml(outputVerify.Message);

//                                                    var resultVerify = bool.Parse(oXmlVerify.SelectSingleNode("//result").InnerText);
//                                                    var action = oXmlVerify.SelectSingleNode("//action").InnerText;
//                                                    var invoiceNumber = oXmlVerify.SelectSingleNode("//invoiceNumber").InnerText;
//                                                    var invoiceDate = oXmlVerify.SelectSingleNode("//invoiceDate").InnerText;
//                                                    var terminalCode = oXmlVerify.SelectSingleNode("//terminalCode").InnerText;
//                                                    var merchantCode = oXmlVerify.SelectSingleNode("//merchantCode").InnerText;

//                                                    //اطمینان از موفق بودن تراکنش 
//                                                    if (resultVerify && action == "1003" &&
//                                                        int.Parse(invoiceNumber) == pasargadPayment.PaymentResultId &&
//                                                        invoiceDate == pasargadPayment.InvoiceDate &&
//                                                        merchantCode == pasargadPayment.Merchant &&
//                                                        terminalCode == pasargadPayment.Terminal)
//                                                    {
//                                                        ApplyPaymentChangesToDB(
//                                                            pasargadPayment.PaymentResultId,
//                                                            null,
//                                                            string.Empty,
//                                                            transactionCode: oXmlVerify.SelectSingleNode("//referenceNumber").InnerText);

//                                                        #region Settle

//                                                        try
//                                                        {
//                                                            string[] settleParams =
//                                                            {
//                                                                "Pasargad",
//                                                                pasargadPayment.PaymentResultId.ToString(),
//                                                                pasargadPayment.InvoiceDate,
//                                                                pasargadPayment.PaidPrice.ToString(),
//                                                                pasargadPayment.Merchant,
//                                                                pasargadPayment.Terminal,
//                                                                pasargadPayment.PrivateKey
//                                                            };

//                                                            GWSrv.Input _inputSettle = new GWSrv.Input();
//                                                            _inputSettle.Params = settleParams;
//                                                            new ClsUtility().LogNew("7pas- Pasargad Candidate try for settle by paymentresultId  : " + pasargadPayment.PaymentResultId + " params : " + string.Join(",", settleParams));

//                                                            ApplyPaymentChangesToDB(
//                                                                pasargadPayment.PaymentResultId,
//                                                                null,
//                                                                "PasargadSSISSettleStart",
//                                                                string.Join(",", settleParams));

//                                                            var outputSettle = client.SettleForSSIS(settleParams);

//                                                            new ClsUtility().LogNew("8pas- Pasargad settle paymentresultId: " + pasargadPayment.PaymentResultId + " message : " + outputSettle.Message);

//                                                            ApplyPaymentChangesToDB(
//                                                                pasargadPayment.PaymentResultId,
//                                                                null,
//                                                                "PasargadSSISSettleResult",
//                                                                "Result:" + outputSettle.Result.ToString() + "-" + "Message:" + outputSettle.Message);

//                                                            if (outputSettle.Result == Result.ResponseSuccess)
//                                                            {
//                                                                if (!string.IsNullOrEmpty(outputSettle.Message))
//                                                                {
//                                                                    XmlDocument oXmlSettle = new XmlDocument();
//                                                                    oXmlSettle.LoadXml(outputSettle.Message);

//                                                                    var resultSettle = bool.Parse(oXmlSettle.SelectSingleNode("//result").InnerText);
//                                                                    if (resultSettle)
//                                                                    {
//                                                                        ApplyPaymentChangesToDB(
//                                                                            pasargadPayment.PaymentResultId,
//                                                                            (int)Enums.PaymentResultStatus.WaittingForSend,
//                                                                            string.Empty);

//                                                                        new ClsUtility().LogNew("9pas- Pasargad settle Success PaymentResultId : " + pasargadPayment.PaymentResultId);

//                                                                        continue;
//                                                                    }

//                                                                    ApplyPaymentChangesToDB(
//                                                                        pasargadPayment.PaymentResultId,
//                                                                        (int)Enums.PaymentResultStatus.Failed,
//                                                                        string.Empty);

//                                                                    new ClsUtility().LogNew("10pas- Pasargad settle failed PaymentResultId : " + pasargadPayment.PaymentResultId);

//                                                                    continue;
//                                                                }
//                                                            }

//                                                            new ClsUtility().LogNew("11pas- Pasargad settle Unknown PaymentResultId : " + pasargadPayment.PaymentResultId + " Message: " + outputSettle.Message);

//                                                            ApplyPaymentChangesToDB(
//                                                                pasargadPayment.PaymentResultId,
//                                                                (int)Enums.PaymentResultStatus.Unknown,
//                                                                string.Empty);

//                                                            continue;
//                                                        }
//                                                        catch (Exception ex)
//                                                        {
//                                                            new ClsUtility().LogNew("12pas- Pasargad settle Unknown PaymentResultId : " + pasargadPayment.PaymentResultId + " Exception: " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                                                            ApplyPaymentChangesToDB(
//                                                                pasargadPayment.PaymentResultId,
//                                                                (int)Enums.PaymentResultStatus.Unknown,
//                                                                "PasargadSSISSettleExcption",
//                                                                ex.Message);
//                                                        }

//                                                        #endregion
//                                                    }

//                                                    ApplyPaymentChangesToDB(
//                                                        pasargadPayment.PaymentResultId,
//                                                        (int)Enums.PaymentResultStatus.Failed,
//                                                        string.Empty);

//                                                    new ClsUtility().LogNew("13pas- Pasargad verify failed PaymentResultId : " + pasargadPayment.PaymentResultId);

//                                                    continue;
//                                                }
//                                            }

//                                            new ClsUtility().LogNew("14pas- Pasargad verify  Unknown PaymentResultId : " + pasargadPayment.PaymentResultId + " Message: " + outputVerify.Message);

//                                            ApplyPaymentChangesToDB(
//                                                pasargadPayment.PaymentResultId,
//                                                (int)Enums.PaymentResultStatus.Unknown,
//                                                string.Empty);
//                                        }
//                                        catch (Exception ex)
//                                        {
//                                            new ClsUtility().LogNew("15pas- Pasargad verify Unknown PaymentResultId : " + pasargadPayment.PaymentResultId + " Exception: " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                                            ApplyPaymentChangesToDB(
//                                                pasargadPayment.PaymentResultId,
//                                                (int)Enums.PaymentResultStatus.Unknown,
//                                                "PasargadSSISVerifyException",
//                                                ex.Message);
//                                        }

//                                        #endregion
//                                    }
//                                    else
//                                    {
//                                        //شرایطی که شماره مرجع، در استعلام از بانک دریافت نشود
//                                        ApplyPaymentChangesToDB(
//                                            pasargadPayment.PaymentResultId,
//                                            (int)Enums.PaymentResultStatus.Failed,
//                                            string.Empty);

//                                        new ClsUtility().LogNew("16pas- Pasargad inquiry failed tref null PaymentResultId : " + pasargadPayment.PaymentResultId);

//                                        continue;
//                                    }
//                                }
//                            }

//                            new ClsUtility().LogNew("17pas- Pasargad inquiry Unknown PaymentResultId : " + pasargadPayment.PaymentResultId + " Message: " + output.Message);

//                            ApplyPaymentChangesToDB(
//                                pasargadPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Unknown,
//                                string.Empty);

//                        }
//                        catch (Exception ex)
//                        {
//                            new ClsUtility().LogNew("18pas- Pasargad inquiry Unknown PaymentResultId : " + pasargadPayment.PaymentResultId + " Exception: " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                            ApplyPaymentChangesToDB(
//                                pasargadPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Unknown,
//                                "PasargadSSISInquiryException",
//                                ex.Message);
//                        }

//                        #endregion
//                    }
//                }
//                #endregion
//            }
//            public void Ikco()
//            {
//                #region Ikco

//                var _ikcoPayInProgress = GetIkcoPayInProgress();
//                new ClsUtility().LogNew("1i- ikco Candidate count : " + _ikcoPayInProgress.Count);

//                foreach (var ikcoPayment in _ikcoPayInProgress)
//                {
//                    //اگر کاربر مرورگر خود را ببندد در صفحه تکمیل خرید - یعنی پول را داده ولی تکمیل خرید نزده
//                    if (string.IsNullOrEmpty(ikcoPayment.Token) || string.IsNullOrEmpty(ikcoPayment.RefNum))
//                    {
//                        ApplyPaymentChangesToDB(
//                            ikcoPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "IkcoSSISNotVerified-TokenOrRefNumIsNullOrEmpty");

//                        new ClsUtility().LogNew("2i- ikco Candidate Token or RefNum is null or empty, paymentresultId: " + ikcoPayment.PaymentResultId);
//                    }

//                    //در صورتی که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشی نباشد و ظرفیت برنامه فروش تمام شده باشد
//                    //استعلام نمی گیریم تا پس از 30 دقیقه تراکنش توسط بانک اتو ریورس شود
//                    //بنابراین وضعیت تراکنش را ناموفق می کنیم
//                    //else if (ikcoPayment.OrderId != 0 && ikcoPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(ikcoPayment.SaleDetailId.Value))
//                    else if (ikcoPayment.OrderId != 0 && !HasCapacityBySold(ikcoPayment.SaleDetailId.Value))
//                    {
//                        ApplyPaymentChangesToDB(
//                            ikcoPayment.PaymentResultId,
//                            (int)Enums.PaymentResultStatus.Failed,
//                            "IKcoSSISNotVerified-SaleDetailHasNoCapacity");

//                        new ClsUtility().LogNew("3i- ikco Candidate Failed, SaleDetail has no capacity, paymentresultId: " + ikcoPayment.PaymentResultId);
//                    }
//                    else
//                    {
//                        #region "Settle"

//                        try
//                        {
//                            string[] paramArray =
//                            {
//                                "Ikco",
//                                ikcoPayment.UserName,
//                                ikcoPayment.Password,
//                                ikcoPayment.Token,
//                                ikcoPayment.RefNum
//                            };

//                            GWSrv.Input _input = new GWSrv.Input();
//                            _input.BankType = (GWSrv.BankType)(7);
//                            _input.Params = paramArray;
//                            new ClsUtility().LogNew("4i- ikco Candidate try for settle by paymentresultId: " + ikcoPayment.PaymentResultId + " params : " + string.Join(",", paramArray));

//                            ApplyPaymentChangesToDB(
//                                ikcoPayment.PaymentResultId,
//                                null,
//                                "IkcoSSISVerifyStart",
//                                string.Join(",", paramArray));

//                            var output = client.SettleForSSIS(paramArray);

//                            ApplyPaymentChangesToDB(
//                                ikcoPayment.PaymentResultId,
//                                null,
//                                "IkcoSSISVerifyResult",
//                                "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                            new ClsUtility().LogNew("5i- ikco settle for paymentresultId: " + ikcoPayment.PaymentResultId + " message : " + output.Message);

//                            if (output.Result == Result.ResponseSuccess)
//                            {
//                                //باید چك شود كه مبلغ دریافتی از بانك با مبلغ ركورد پرداخت یكی باشد
//                                var result = output.Message.Split(',');
//                                int amount = int.Parse(result[1]);

//                                if (ikcoPayment.PaidPrice == amount)
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        ikcoPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.WaittingForSend,
//                                        string.Empty,
//                                        additionalData: "<VerifyRefNum>" + result[0] + "</VerifyRefNum>" + "<VerifyAmount>" + result[1] + "</VerifyAmount>" + "<VerifyResult>" + result[2] + "</VerifyResult>");

//                                    new ClsUtility().LogNew("6i- ikco settle success  PaymentResultId : " + ikcoPayment.PaymentResultId);
//                                }
//                                else
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        ikcoPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.Failed,
//                                        string.Empty,
//                                        additionalData: "<VerifyRefNum>" + result[0] + "</VerifyRefNum>" + "<VerifyAmount>" + result[1] + "</VerifyAmount>" + "<VerifyResult>" + result[2] + "</VerifyResult>");

//                                    new ClsUtility().LogNew("7i- ikco settle Failed with RefNum: " + result[0] + " and Amount: " + result[1] + " and Result: " + result[2] + " PaymentResultId : " + ikcoPayment.PaymentResultId);
//                                }
//                            }
//                            else
//                            {
//                                ApplyPaymentChangesToDB(
//                                    ikcoPayment.PaymentResultId,
//                                    (int)Enums.PaymentResultStatus.Unknown,
//                                    string.Empty);

//                                new ClsUtility().LogNew("8i- ikco settle Unknown PaymentResultId : " + ikcoPayment.PaymentResultId);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            new ClsUtility().LogNew("9i- ikco settle " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                            ApplyPaymentChangesToDB(
//                                ikcoPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Unknown,
//                                "IkcoSSISVerifyException",
//                                ex.Message);
//                        }

//                        #endregion
//                    }
//                }

//                #endregion
//            }
//            public void IranKish()
//            {
//                #region IranKish

//                try
//                {
//                    var _iranKishPayInProgress = GetIranKishPayInProgress();
//                    if (_iranKishPayInProgress == null)
//                    {
//                        new ClsUtility().LogNew("1i- iranKish Candidate count : 0");

//                        return;
//                    }


//                    new ClsUtility().LogNew("1i- iranKish Candidate count : " + _iranKishPayInProgress.Count);

//                    foreach (var iranKishPayment in _iranKishPayInProgress)
//                    {
//                        //اگر کاربر مرورگر خود را ببندد در صفحه تکمیل خرید - یعنی پول را داده ولی تکمیل خرید نزده
//                        if (string.IsNullOrEmpty(iranKishPayment.Token) || string.IsNullOrEmpty(iranKishPayment.TransactionCode) || string.IsNullOrEmpty(iranKishPayment.systemTraceAuditNumber))
//                        {
//                            ApplyPaymentChangesToDB(
//                                iranKishPayment.PaymentResultId,
//                                (int)Enums.PaymentResultStatus.Failed,
//                                "IranKishSSISNotVerified-TokenOrReferenceIdOrAuditNumberIsNullOrEmpty");

//                            new ClsUtility().LogNew("2i- iranKish Candidate Token Or ReferenceId is null or empty, paymentresultId: " + iranKishPayment.PaymentResultId);
//                        }

//                        //در صورتی که تراکنش از نوع ثبت نام باشد و برنامه فروش از نوع قرعه کشی نباشد و ظرفیت برنامه فروش تمام شده باشد
//                        //استعلام نمی گیریم تا پس از 30 دقیقه تراکنش توسط بانک اتو ریورس شود
//                        //بنابراین وضعیت تراکنش را ناموفق می کنیم
//                        //else if (iranKishPayment.OrderId != 0 && iranKishPayment.PaymentFlowName != "PaymentForLottery" && !HasCapacityBySold(iranKishPayment.SaleDetailId.Value))
//                        else if (iranKishPayment.OrderId != 0 && !HasCapacityBySold(iranKishPayment.SaleDetailId.Value))
//                        {
//                            ApplyPaymentChangesToDB(
//                               iranKishPayment.PaymentResultId,
//                               (int)Enums.PaymentResultStatus.Failed,
//                               "IranKishSSISNotVerified-SaleDetailHasNoCapacity");

//                            new ClsUtility().LogNew("3i- iranKish Candidate Failed, SaleDetail has no capacity, paymentresultId: " + iranKishPayment.PaymentResultId);
//                        }
//                        else
//                        {
//                            #region Settle
//                            try
//                            {


//                                string[] paramArray = {"IranKish",
//                            iranKishPayment.Token,/*token*/
//                                                 iranKishPayment.Terminal,  /*terminal*/
//                                       iranKishPayment.TransactionCode, /*retrievalReferenceNumber*/
//                                       iranKishPayment.systemTraceAuditNumber /*systemTraceAuditNumber*/};

//                                GWSrv.Input _input = new GWSrv.Input();
//                                _input.BankType = (GWSrv.BankType)(8);
//                                _input.Params = paramArray;
//                                new ClsUtility().LogNew("4i- iranKish Candidate try for settle by paymentresultId: " + iranKishPayment.PaymentResultId + " params : " + string.Join(",", paramArray));


//                                ApplyPaymentChangesToDB(
//                                          iranKishPayment.PaymentResultId,
//                                          null,
//                                          "IranKishSSISVerifyStart",
//                                          string.Join(",", paramArray));

//                                var output = client.SettleForSSIS(paramArray);


//                                new ClsUtility().LogNew("5i- iranKish settle for paymentresultId: " + iranKishPayment.PaymentResultId + " message : " + output.Message);

//                                ApplyPaymentChangesToDB(
//                                    iranKishPayment.PaymentResultId,
//                                    null,
//                                    "IranKishSSISVerifyResult",
//                                    "Result:" + output.Result.ToString() + "-" + "Message:" + output.Message);

//                                if (output.Result == Result.ResponseSuccess)
//                                {
//                                    if (!string.IsNullOrEmpty(output.Message))
//                                    {
//                                        var result = output.Message.Split(",".ToCharArray());

//                                        //var additionalData =
//                                        //     "<token>" + result[0] + "</token>" +
//                                        //    "<acceptorId>" + result[1] + "</acceptorId>" +
//                                        //    "<responseCode>" + result[2] + "</responseCode>" +
//                                        //    "<paymentId>" + result[3] + "</paymentId>" +
//                                        //    "<RequestId>" + result[4] + "</RequestId>" +
//                                        //    "<sha256OfPan>" + result[5] + "</sha256OfPan>" +
//                                        //    "<systemTraceAuditNumber>" + result[6] + "</systemTraceAuditNumber>" +
//                                        //    "<amount>" + result[7] + "</amount>" +
//                                        //    "<maskedPan>" + result[8] + "</maskedPan>";
//                                        //"<VerifyCode>" + result[0] + "</VerifyCode>" +
//                                        //"<Description>" + result[1] + "</Description>";



//                                        if (result[2] == "00")
//                                        {
//                                            ApplyPaymentChangesToDB(
//                                                iranKishPayment.PaymentResultId,
//                                                (int)Enums.PaymentResultStatus.WaittingForSend,
//                                                string.Empty
//                                                );

//                                            new ClsUtility().LogNew("6i- iranKish settle success PaymentResultId : " + iranKishPayment.PaymentResultId);
//                                        }

//                                        else
//                                        {
//                                            ApplyPaymentChangesToDB(
//                                                iranKishPayment.PaymentResultId,
//                                                (int)Enums.PaymentResultStatus.Failed,
//                                                string.Empty);

//                                            new ClsUtility().LogNew("7i- iranKish iranKish Failed with ResultCode: " + output.Message + " PaymentResultId : " + iranKishPayment.PaymentResultId);
//                                        }
//                                    }

//                                }
//                                else
//                                {
//                                    ApplyPaymentChangesToDB(
//                                        iranKishPayment.PaymentResultId,
//                                        (int)Enums.PaymentResultStatus.Unknown,
//                                        string.Empty);

//                                    new ClsUtility().LogNew("8i- iranKish settle Unknown PaymentResultId : " + iranKishPayment.PaymentResultId);
//                                }
//                            }
//                            catch (Exception ex)
//                            {
//                                new ClsUtility().LogNew("9i- iranKish settle " + ex.Message + ", inner Exception :  " + ex.InnerException);

//                                ApplyPaymentChangesToDB(
//                                    iranKishPayment.PaymentResultId,
//                                    (int)Enums.PaymentResultStatus.Unknown,
//                                    "IranKishSSISVerifyException",
//                                    ex.Message);
//                            }
//                            //   }


//                            #endregion

//                        }
//                    }
//                }
//                catch (Exception ex)
//                {

//                    throw ex;
//                }


//                #endregion
//            }

//            public void Main()
//            {
//                try
//                {
//                    client = new GWSrv.BasicHttpBinding_IGatewayService();

//                    //string OnlineSale_connection = (string)Dts.Variables["$Package::OnlineSale"].Value;
//                    //new ClsUtility().LogNewFilePath = (string)Dts.Variables["$Package::LogPath"].Value;
//                    //ClsUtility.ConnectionString_FavaLog = (string)Dts.Variables["$Package::OnlineSaleLog"].Value;
//                    ClsUtility.ConnectionString_FavaLog = ((SqlConnection)Dts.Connections["OnlineSaleLog"].AcquireConnection(Dts.Transaction)).ConnectionString;
//                    ClsUtility.BatchId = DateTime.Now.Ticks;

//                    new ClsUtility().LogNew("-------------SSIS Bank and Capacity Version(6.9) IKCO Start at:");

//                    new ClsUtility().LogNew("client.Url : " + client.Url);

//                    Stopwatch stopWatch = new Stopwatch();
//                    stopWatch.Start();

//                    //connection_FAVA = new SqlConnection(OnlineSale_connection);
//                    connection_FAVA = (SqlConnection)Dts.Connections["OnlineSale"].AcquireConnection(Dts.Transaction);
//                    connection_FAVA.Close();
//                    connection_FAVA.ConnectionString = "MultipleActiveResultSets=true;" + connection_FAVA.ConnectionString;
//                    connection_FAVA.Open();
//                    Thread thMellat = new Thread(new ThreadStart(Mellat));
//                    Thread thParsian = new Thread(new ThreadStart(Parsian));
//                    Thread thSaman = new Thread(new ThreadStart(Saman));
//                    Thread thSadad = new Thread(new ThreadStart(Sadad));
//                    Thread thIrankish = new Thread(new ThreadStart(IranKish));
//                    Thread thIkco = new Thread(new ThreadStart(Ikco));
//                    thMellat.Start();
//                    thParsian.Start();
//                    thSadad.Start();
//                    thSaman.Start();
//                    thIrankish.Start();
//                    thIkco.Start();

//                    thMellat.Join();
//                    thParsian.Join();
//                    thSadad.Join();
//                    thSaman.Join();
//                    thIrankish.Join();
//                    thIkco.Join();
//                    connection_FAVA.Close();
//                }
//                catch (Exception ex)
//                {
//                    connection_FAVA.Close();
//                    new ClsUtility().LogNew("public exception : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                }

//                new ClsUtility().LogNew("-------------SSIS Bank and Capacity Version(6.9) IKCO End at:");

//                Dts.TaskResult = (int)ScriptResults.Success;
//            }

//            #region PayInProgress
//            private List<MellatPaymentResultDto> GetMellatPayInProgress()
//            {
//                List<MellatPaymentResultDto> _paymentResultDto = new List<MellatPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(1);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][2].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new MellatPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            TransactionCode = query.Rows[i][1].ToString(),
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][3].ToString()),
//                            TerminalId = query.Rows[i][4].ToString(),
//                            UserName = query.Rows[i][5].ToString(),
//                            Password = query.Rows[i][6].ToString(),
//                            //PaymentFlowName = query.Rows[i][7].ToString()
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetMellatPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            private List<ParsianPaymentResultDto> GetParsianPayInProgress()
//            {
//                List<ParsianPaymentResultDto> _paymentResultDto = new List<ParsianPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(3);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][3].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new ParsianPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            Pin = query.Rows[i][1].ToString(),
//                            Token = query.Rows[i][2].ToString(),
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][4].ToString()),
//                            //PaymentFlowName = query.Rows[i][5].ToString()
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetParsianPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            private List<PasargadPaymentResultDto> GetPasargadPayInProgress()
//            {
//                List<PasargadPaymentResultDto> _paymentResultDto = new List<PasargadPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(2);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][6].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new PasargadPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            InvoiceDate = query.Rows[i][1].ToString(),
//                            PaidPrice = decimal.Parse(query.Rows[i][2].ToString()),
//                            Status = int.Parse(query.Rows[i][3].ToString()),
//                            Merchant = query.Rows[i][4].ToString(),
//                            Terminal = query.Rows[i][5].ToString(),
//                            PrivateKey = query.Rows[i][8].ToString(),
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][7].ToString()),
//                            //PaymentFlowName = query.Rows[i][9].ToString()
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetPasargadPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            private List<SamanPaymentResultDto> GetSamanPayInProgress()
//            {
//                List<SamanPaymentResultDto> _paymentResultDto = new List<SamanPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(5);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][4].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new SamanPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            RefNum = query.Rows[i][1].ToString(),
//                            MID = query.Rows[i][2].ToString(),
//                            PaidPrice = decimal.Parse(query.Rows[i][3].ToString()),
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][5].ToString()),
//                            //PaymentFlowName = query.Rows[i][6].ToString()
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetSamanPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            private List<SadadPaymentResultDto> GetSadadPayInProgress()
//            {
//                List<SadadPaymentResultDto> _paymentResultDto = new List<SadadPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(6);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][3].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new SadadPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            MerchantKey = query.Rows[i][1].ToString(),
//                            Token = query.Rows[i][2].ToString(),
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][4].ToString()),
//                            //PaymentFlowName = query.Rows[i][5].ToString()
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetSadadPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            private List<IkcoPaymentResultDto> GetIkcoPayInProgress()
//            {
//                List<IkcoPaymentResultDto> _paymentResultDto = new List<IkcoPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(7);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][6].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new IkcoPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            UserName = query.Rows[i][1].ToString(),
//                            Password = query.Rows[i][2].ToString(),
//                            Token = query.Rows[i][3].ToString(),
//                            RefNum = query.Rows[i][4].ToString(),
//                            PaidPrice = decimal.Parse(query.Rows[i][5].ToString()),
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][7].ToString()),
//                            //PaymentFlowName = query.Rows[i][8].ToString()
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetIkcoPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            private List<IranKishPaymentResultDto> GetIranKishPayInProgress()
//            {
//                List<IranKishPaymentResultDto> _paymentResultDto = new List<IranKishPaymentResultDto>();

//                try
//                {
//                    var _paymentResult = new PaymentResult(connection_FAVA);
//                    var query = _paymentResult.GetPeyInProgress(8);
//                    for (int i = 0; i < query.Rows.Count; i++)
//                    {
//                        int _outSaleDetailId = 0;
//                        int.TryParse(query.Rows[i][5].ToString(), out _outSaleDetailId);

//                        _paymentResultDto.Add(new IranKishPaymentResultDto()
//                        {
//                            PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//                            Token = query.Rows[i][1].ToString(),
//                            Terminal = query.Rows[i][2].ToString(),
//                            TransactionCode = query.Rows[i][3].ToString(), /*referencenumber*/
//                            systemTraceAuditNumber = query.Rows[i][4].ToString(), /*referencenumber*/
//                            SaleDetailId = _outSaleDetailId,
//                            OrderId = int.Parse(query.Rows[i][6].ToString()),
//                            //PaymentFlowName = query.Rows[i][6].ToString()

//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew("GetIranKishPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//                    return null;
//                }
//                return _paymentResultDto;
//            }
//            #endregion


//            private string GetVerifyResultErrorMessage(int i)
//            {
//                switch (i)
//                {
//                    case -1:
//                        return "تراکنش مورد نظر با پارامترهای ارسالی RefNum و MID پیدا نشده است )مشکل در پارامترهای ارسالی در وب سرویس(.";

//                    case -6:
//                        return @"تراکنش مورد نظر قبلا برگشت خورده است. یا به دلیل ارسال برگشت تراکنش قبل از ارسال تاییدیه تراکنش از سوی مشتری و یا ارسال تایید تراکنش پس از سپری شدن 30 دقیقه از انجام تراکنش";

//                    case -18:
//                        return "آدرس IP فروشنده سمت درگاه پرداخت تعریف نشده است.";

//                    default:
//                        return "";
//                }
//            }
//            private string GetVerifyResultErrorMessageIkco(int i)
//            {
//                bool isError = false;
//                string errorMsg = "";
//                switch (i)
//                {

//                    case -1:        //TP_ERROR
//                        isError = true;
//                        errorMsg = "بروز خطا درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-1";
//                        break;
//                    case -2:        //ACCOUNTS_DONT_MATCH
//                        isError = true;
//                        errorMsg = "بروز خطا در هنگام تاييد رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-2";
//                        break;
//                    case -3:        //BAD_INPUT
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-3";
//                        break;
//                    case -4:        //INVALID_PASSWORD_OR_ACCOUNT
//                        isError = true;
//                        errorMsg = "خطاي دروني سيستم درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-4";
//                        break;
//                    case -5:        //DATABASE_EXCEPTION
//                        isError = true;
//                        errorMsg = "خطاي دروني سيستم درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-5";
//                        break;
//                    case -7:        //ERROR_STR_NULL
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-7";
//                        break;
//                    case -8:        //ERROR_STR_TOO_LONG
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-8";
//                        break;
//                    case -9:        //ERROR_STR_NOT_AL_NUM
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-9";
//                        break;
//                    case -10:   //ERROR_STR_NOT_BASE64
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-10";
//                        break;
//                    case -11:   //ERROR_STR_TOO_SHORT
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-11";
//                        break;
//                    case -12:       //ERROR_STR_NULL
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-12";
//                        break;
//                    case -13:       //ERROR IN AMOUNT OF REVERS TRANSACTION AMOUNT
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-13";
//                        break;
//                    case -14:   //INVALID TRANSACTION
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-14";
//                        break;
//                    case -15:   //RETERNED AMOUNT IS WRONG
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-15";
//                        break;
//                    case -16:   //INTERNAL ERROR
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-16";
//                        break;
//                    case -17:   // REVERS TRANSACTIN FROM OTHER BANK
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-17";
//                        break;
//                    case -18:   //INVALID IP
//                        isError = true;
//                        errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-18";
//                        break;

//                }
//                return errorMsg;
//            }

//            private bool ApplyPaymentChangesToDB(int PaymentResultId, int? PaymentResultStatus, string Message, string Parameter = null, string transactionCode = null, string additionalData = null)
//            {
//                SqlDataAdapter da = new SqlDataAdapter();
//                SqlCommand cmd = new SqlCommand();
//                DataTable dt = new DataTable();
//                DataSet ds = new DataSet();
//                try
//                {

//                    if (connection_FAVA.State != ConnectionState.Open) connection_FAVA.Open();

//                    cmd.Connection = connection_FAVA;
//                    cmd.CommandTimeout = 0;
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.CommandText = "dbo.spInsertAndUpdateDataForSetteledPayment";

//                    cmd.Parameters.AddWithValue("@PaymentResultId", PaymentResultId);
//                    cmd.Parameters.AddWithValue("@Message", Message);
//                    cmd.Parameters.AddWithValue("@Parameter", Parameter);
//                    cmd.Parameters.AddWithValue("@PaymentResultStatus", PaymentResultStatus);
//                    cmd.Parameters.AddWithValue("@TransactionCode", transactionCode);
//                    cmd.Parameters.AddWithValue("@AdditionalData", additionalData);

//                    cmd.ExecuteNonQuery();

//                    return true;
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew(ex.Message + " at :");
//                    return false;
//                }
//            }

//            private bool HasCapacityBySold(int saleDetailId)
//            {
//                SqlDataAdapter da = new SqlDataAdapter();
//                SqlCommand cmd = new SqlCommand();
//                DataTable dt = new DataTable();
//                DataSet ds = new DataSet();
//                try
//                {
//                    if (connection_FAVA.State != ConnectionState.Open) connection_FAVA.Open();

//                    cmd.Connection = connection_FAVA;
//                    cmd.CommandTimeout = 0;
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.CommandText = "dbo.sp_SaleDetailHasCapacity";
//                    cmd.Parameters.AddWithValue("@SaleDetailID", saleDetailId);

//                    SqlDataReader dr = cmd.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        return Convert.ToBoolean(dr[0]);
//                    }
//                    return false;
//                }
//                catch (Exception ex)
//                {
//                    new ClsUtility().LogNew(ex.Message + " at :");
//                    return false;
//                }
//                finally
//                {
//                    // cmd.Connection.Close();
//                }
//            }

//            #region ScriptResults declaration
//            /// <summary>
//            /// This enum provides a convenient shorthand within the scope of this class for setting the
//            /// result of the script.
//            /// 
//            /// This code was generated automatically.
//            /// </summary>
//            enum ScriptResults
//            {
//                Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success,
//                Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
//            };
//            #endregion

//            #region Inquiry

//            //private List<SadadPaymentResultDto> GetSadadPayInProgressForInquiry()
//            //{
//            //    List<SadadPaymentResultDto> _paymentResultDto = new List<SadadPaymentResultDto>();

//            //    try
//            //    {
//            //        var _paymentResult = new PaymentResult(connection_FAVA);
//            //        var query = _paymentResult.GetPeyInProgressForInquiry(6);
//            //        for (int i = 0; i < query.Rows.Count; i++)
//            //        {
//            //            int _outSaleDetailId = 0;
//            //            int.TryParse(query.Rows[i][3].ToString(), out _outSaleDetailId);

//            //            _paymentResultDto.Add(new SadadPaymentResultDto()
//            //            {
//            //                PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//            //                MerchantKey = query.Rows[i][1].ToString(),
//            //                Token = query.Rows[i][2].ToString(),
//            //                SaleDetailId = _outSaleDetailId,
//            //                OrderId = int.Parse(query.Rows[i][4].ToString()),
//            //                //PaymentFlowName = query.Rows[i][5].ToString()
//            //            });
//            //        }
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        ClsUtility.Log("GetSadadPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//            //        return null;
//            //    }
//            //    return _paymentResultDto;
//            //}


//            //private List<MellatPaymentResultDto> GetMellatPayInProgressForInquiry()
//            //{
//            //    List<MellatPaymentResultDto> _paymentResultDto = new List<MellatPaymentResultDto>();

//            //    try
//            //    {
//            //        var _paymentResult = new PaymentResult(connection_FAVA);
//            //        var query = _paymentResult.GetPeyInProgressForInquiry(1);
//            //        for (int i = 0; i < query.Rows.Count; i++)
//            //        {
//            //            int _outSaleDetailId = 0;
//            //            int.TryParse(query.Rows[i][2].ToString(), out _outSaleDetailId);

//            //            _paymentResultDto.Add(new MellatPaymentResultDto()
//            //            {
//            //                PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//            //                TransactionCode = query.Rows[i][1].ToString(),
//            //                SaleDetailId = _outSaleDetailId,
//            //                OrderId = int.Parse(query.Rows[i][3].ToString()),
//            //                TerminalId = query.Rows[i][4].ToString(),
//            //                UserName = query.Rows[i][5].ToString(),
//            //                Password = query.Rows[i][6].ToString(),
//            //                //PaymentFlowName = query.Rows[i][7].ToString()
//            //            });
//            //        }
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        new ClsUtility().LogNew("GetMellatPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//            //        return null;
//            //    }
//            //    return _paymentResultDto;
//            //}


//            //private List<ParsianPaymentResultDto> GetParsianPayInProgressForInquiry()
//            //{
//            //    List<ParsianPaymentResultDto> _paymentResultDto = new List<ParsianPaymentResultDto>();

//            //    try
//            //    {
//            //        var _paymentResult = new PaymentResult(connection_FAVA);
//            //        var query = _paymentResult.GetPeyInProgressForInquiry(3);
//            //        for (int i = 0; i < query.Rows.Count; i++)
//            //        {
//            //            int _outSaleDetailId = 0;
//            //            int.TryParse(query.Rows[i][3].ToString(), out _outSaleDetailId);

//            //            _paymentResultDto.Add(new ParsianPaymentResultDto()
//            //            {
//            //                PaymentResultId = int.Parse(query.Rows[i][0].ToString()),
//            //                Pin = query.Rows[i][1].ToString(),
//            //                Token = query.Rows[i][2].ToString(),
//            //                SaleDetailId = _outSaleDetailId,
//            //                OrderId = int.Parse(query.Rows[i][4].ToString()),
//            //                //PaymentFlowName = query.Rows[i][5].ToString()
//            //            });
//            //        }
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        new ClsUtility().LogNew("GetParsianPayInProgress : " + ex.Message + ", inner Exception :  " + ex.InnerException);
//            //        return null;
//            //    }
//            //    return _paymentResultDto;
//            //}

//            #endregion

//        }
//    }
//}
