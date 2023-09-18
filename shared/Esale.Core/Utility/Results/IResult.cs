using System;
using System.Collections.Generic;
using System.Text;

namespace Esale.Core.Utility.Results
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
        string MessageId { get; }
    }
}
