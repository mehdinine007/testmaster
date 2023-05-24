using System;
using System.Collections.Generic;
using System.Text;

namespace Esale.Core.Utility.Results
{
    public interface IResult
    {
        bool Succsess { get; }
        string Message { get; }
        string MessageId { get; }
    }
}
