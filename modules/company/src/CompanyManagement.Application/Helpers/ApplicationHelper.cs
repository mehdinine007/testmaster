using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;

namespace OrderManagement.Application.Helpers
{
    public static class ApplicationHelper
    {
        public static string ConcatErrorMessages(this List<Exception> exceptions)
        {
            var msg = new StringBuilder();
            exceptions.ForEach(x => msg.AppendLine(x.Message));
            return msg.ToString();
        }
    }
}
