using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Host.Infrastructures;

public class ApiResult
{
    public static ApiResult InitilizeFailureApiResult(Exception ex)
    {
        return new ApiResult()
        {
            Result = null,
            Error = ex,
            Success = false
        };
    }

    public static ApiResult InitilizeSuccessfullApiResult(object result)
    {
        return new ApiResult()
        {
            Success = true,
            Error = null,
            Result = result
        };
    }

    private ApiResult()
    {

    }

    public object Result { get; set; }

    public bool Success { get; set; }

    public Exception Error { get; set; }
}