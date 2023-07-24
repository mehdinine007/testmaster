using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Grpc.Core.Metadata;

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
