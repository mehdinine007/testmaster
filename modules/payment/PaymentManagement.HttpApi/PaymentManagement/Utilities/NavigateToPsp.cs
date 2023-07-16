using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.Text;

namespace PaymentManagement.HttpApi.Utilities
{
    public class NavigateToPsp
    {
        private NameValueCollection Inputs = new NameValueCollection();
        private string m_Url = "";
        private string m_Method = "post"; //or Get
        private string m_FormName = "form1";

        public void Post1(HttpContext context)
        {

            context.Response.Clear();
            try
            {
                context.Response.ContentType = "text/html";

            }
            catch (Exception ex)
            {

            }
            //     StringBuilder sb = new StringBuilder();
            //     sb.Append("<html><head>");
            context.Response.WriteAsync("<html><head>");

            context.Response.WriteAsync(string.Format("</head><body onload=\"document.{0}.submit()\">", m_FormName));
            //    sb.Append(string.Format("</head><body onload=\"document.{0}.submit()\">", m_FormName));

            context.Response.WriteAsync(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", m_FormName, m_Method, Url));
            //      sb.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", m_FormName, m_Method, Url));

            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                context.Response.WriteAsync(string.Format("<input name=\"{0}\" type=\"hidden\" value=\'{1}\'>", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                //         sb.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\'{1}\'>", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            //     sb.Append("</form>");

            context.Response.WriteAsync("</form>");
            //    sb.Append("</body></html>");

            context.Response.WriteAsync("</body></html>");


            context.Response.CompleteAsync();

            // return sb;
        }

        public string Post(HttpContext context)
        {
            try
            {
                context.Response.Clear();
                context.Response.ContentType = "text/html";
                context.Response.Headers.Add("Connection", "keep-alive");

                StringBuilder sb = new();
                sb.Append("<html><head>");
                sb.Append(string.Format("</head><body onload=\"document.{0}.submit()\">", m_FormName));
                sb.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", m_FormName, m_Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    sb.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\'{1}\'>", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                sb.Append("</form>");
                sb.Append("</body></html>");

                context.Response.WriteAsync(sb.ToString());
                context.Response.CompleteAsync();
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message + (ex.InnerException != null ? (Environment.NewLine + "InnerException: " + ex.InnerException.Message) : string.Empty);
            }
        }

        public void AddKey(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public string Method
        {
            get
            {
                return m_Method;
            }
            set
            {
                m_Method = value;
            }
        }

        public string FormName
        {
            get
            {
                return m_FormName;
            }
            set
            {
                m_FormName = value;
            }
        }

        public string Url
        {
            get
            {
                return m_Url;
            }
            set
            {
                m_Url = value;
            }
        }
    }
}
