using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Utility
{
    public static class utilities
    {
        public static string GetServerIPAddress()
        {
            string name = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
            else
            {
                try
                {
                    return System.Environment.MachineName;

                }
                catch (Exception ex)
                {

                }
                return "192";
            }

        }
        public static string GetIpAddress(IHttpContextAccessor _httpContextAccessor)
        {
            string result = "";
            if (_httpContextAccessor.HttpContext.Request.Headers != null)
            {
                //the X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a client
                //connecting to a web server through an HTTP proxy or load balancer

                var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                if (!StringValues.IsNullOrEmpty(forwardedHeader))
                    result = forwardedHeader.FirstOrDefault();
            }

            //if this header not exists try get connection remote IP address
            if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return result;
        }


    }
}
