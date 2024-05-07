using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace GatewayManagement.Application.GatewayManagement.Services.SendBox.Sign.Provider
{
    public class IranSign
    {
        private IranSignConfig _config;
        public IranSign(IranSignConfig config)
        {
            _config = config;
        }

        public async Task<ResponseCreateIranSign> Create(RequestCreateIranSign createIranSignDto)
        {

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(_config.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("api-key", _config.ApiKey);
                var content = new StringContent(JsonConvert.SerializeObject(createIranSignDto), Encoding.UTF8, "application/json");
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                     response = await client.PostAsync("/dms/api/v1/clientOrg/workflow/create", content);
                }
                catch (Exception)
                {
                    return new ResponseCreateIranSign
                    {
                        Success = false,
                        message = "خطا در برقراری ارتباط",
                        resultCode = 99
                    };

                }
               
                string readContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ResponseCreateIranSign result = null;
                    result = JsonConvert.DeserializeObject<ResponseCreateIranSign>(readContent);
                    result.Success = true;
                    return result;
                }
                else
                {
                    ErrorResponseIranSign _ret = null;
                    List<ErrorResponseIranSign> result = null;
                    result = JsonConvert.DeserializeObject<List<ErrorResponseIranSign>>(readContent);
                    _ret = result.FirstOrDefault();
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new ResponseCreateIranSign
                        {
                            Success = false,
                            message = _ret.message,
                            resultCode = (int)HttpStatusCode.Unauthorized
                        };
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return new ResponseCreateIranSign
                        {
                            Success = false,
                            resultCode = (int)HttpStatusCode.InternalServerError,
                            message = _ret.message
                        };
                    }
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return new ResponseCreateIranSign
                        {
                            Success = false,
                            resultCode = (int)HttpStatusCode.BadRequest,
                            message = _ret.message
                        };
                    }
                    return new ResponseCreateIranSign
                    {
                        Success = false,
                        resultCode = 99
                    };
                }
            }
        }

        public async Task<ResponseInquiryIranSign> Inquiry(Guid workflowTicket)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(_config.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("api-key", _config.ApiKey);
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await client.GetAsync($"/dms/api/v1/clientOrg/workflow/{workflowTicket}/ownerDetails?");
                }
                catch (Exception ex)
                {
                   
                    return new ResponseInquiryIranSign
                    {
                        message = "خطا در برقراری ارتباط",
                        Success = false,
                        resultCode = 99
                    };
                }

                string readContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ResponseInquiryIranSign result = null;
                    result = JsonConvert.DeserializeObject<ResponseInquiryIranSign>(readContent);
                    result.Success = true;
                    return result;
                }
                else
                {
                    ErrorResponseIranSign _ret = null;
                    List<ErrorResponseIranSign> result = null;
                    result = JsonConvert.DeserializeObject<List<ErrorResponseIranSign>>(readContent);
                    _ret = result.FirstOrDefault();
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new ResponseInquiryIranSign
                        {
                            message = _ret.message,
                            Success = false,
                            resultCode = (int)HttpStatusCode.Unauthorized
                        };
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return new ResponseInquiryIranSign
                        {
                            message = _ret.message,
                            Success = false,
                            resultCode = (int)HttpStatusCode.InternalServerError,
                        };
                    }
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return new ResponseInquiryIranSign
                        {
                            message = _ret.message,
                            Success = false,
                            resultCode = (int)HttpStatusCode.BadRequest,
                        };
                    }
                    return new ResponseInquiryIranSign
                    {
                        message = "",
                        Success = false,
                        resultCode = 99
                    };
                }
            }
        }
    }
}
