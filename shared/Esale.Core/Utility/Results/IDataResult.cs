using System;
using System.Collections.Generic;
using System.Text;

namespace Esale.Core.Utility.Results
{
    public interface IDataResult<out T>:IResult
    {
        T Data { get; }
    }
}
