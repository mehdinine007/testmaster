using System.Collections.Specialized;

namespace PaymentManagement.Application.Utilities
{
    public static class StringUtil
    {
        public static string GenerateForm(string url, string methodName, NameValueCollection inputs)
        {
            var result = string.Empty;

            result += string.Format("<form name='form1' method='{0}' action='{1}' >", methodName, url);
            for (int i = 0; i < inputs.Keys.Count; i++)
            {
                result += string.Format("<input name='{0}' type='hidden' value='{1}'>", inputs.Keys[i], inputs[inputs.Keys[i]]);
            }
            result += "</form>";

            return result;
        }
    }
}
