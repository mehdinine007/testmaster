using Esale.Core.Utility.Results;
using Newtonsoft.Json;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Utility;

namespace UserManagement.Application.InquiryService;

public class InquiryBuilder
{
    private readonly ShahkarInquiryConfig _shahkarInquiryConfig;
    private readonly ShahkarInquiry _shahkarInquiry;
    private readonly FaraBoomInquiryConfig _FaraBoomInquiryConfig;
    private readonly FaraBoomInquiry _FaraBoomInquiry;
    private readonly NajaInquiryConfig _najaInquiryConfig;
    private readonly NajaInquiry _najaInquiry;
    public InquiryBuilder()
    {

    }
    public InquiryBuilder(ShahkarInquiryConfig shahkarInquiryConfig)
    {
        _shahkarInquiryConfig = shahkarInquiryConfig;
        _shahkarInquiry = new ShahkarInquiry(_shahkarInquiryConfig.BaseUrl, _shahkarInquiryConfig.UserName, _shahkarInquiryConfig.Password);
    }

    public InquiryBuilder(FaraBoomInquiryConfig FaraBoomInquiryConfig)
    {
        _FaraBoomInquiryConfig = FaraBoomInquiryConfig;
        _FaraBoomInquiry = new FaraBoomInquiry(_FaraBoomInquiryConfig.BaseUrl, _FaraBoomInquiryConfig.Headers);
    }

    public InquiryBuilder(NajaInquiryConfig najaInquiryConfig)
    {
        _najaInquiryConfig = najaInquiryConfig;
        _najaInquiry = new NajaInquiry(_najaInquiryConfig.Url, _najaInquiryConfig.UserName, _najaInquiryConfig.Password);
    }
    public async Task<IDataResult<string>> Shahkar(string mobile, string nationalCode)
    {
        var _random = new RandomGenerator();
        var _requestId = _shahkarInquiryConfig.PrefixRequestId + DateTime.Now.ToString("yyyyMMddHHmmss") + _random.RandomNumber(100000, 999999);
        var _result = await _shahkarInquiry.Inquiry(new ShahkarInquiryBodyData()
        {
            requestId = _requestId,
            serviceNumber = mobile,
            serviceType = 2,
            identificationType = 0,
            identificationNo = nationalCode
        });
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<ShahkarInquiryResponse>(_result.Data);
            if (_response.response == 200)
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            }
            else
            {
                if (_response.response == 600)
                {
                    return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
                }
                else
                {
                    return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "400");
                }
            }
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }
    public async Task<IDataResult<string>> SabteAhval(int BirthDate, long nationalCode)
    {
        var _random = new RandomGenerator();
        var _requestId = _shahkarInquiryConfig.PrefixRequestId + DateTime.Now.ToString("yyyyMMddHHmmss") + _random.RandomNumber(100000, 999999);
        var _result = await _shahkarInquiry.InquirySabteAhval(new SabteAhvalInput(nationalCode, BirthDate));
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<GSBSabteAhval.getEstelam3Response>(_result.Data);
            return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }

    public async Task<IDataResult<string>> FaraBoom(DateTime birthdate, string nationalCode)
    {
        var _result = await _FaraBoomInquiry.BirthDate(birthdate.ToString("yyyy-MM-dd"), nationalCode);
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<FaraBoomInquiryResponse>(_result.Data);
            if (_response.match)
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            }
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }

    public async Task<IDataResult<string>> FaraBoom(string mobile, string nationalCode)
    {
        var _result = await _FaraBoomInquiry.PhoneNumber(mobile, nationalCode);
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<FaraBoomInquiryResponse>(_result.Data);
            if (_response.match)
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            };
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }

    public async Task<IDataResult<string>> FaraBoom(string zipCode)
    {
        var _result = await _FaraBoomInquiry.PostalCode(zipCode);
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<FaraBoomZipCodeInquiryResponse>(_result.Data);
            if (!string.IsNullOrWhiteSpace(_response.Addresss))
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            }
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }

    public async Task<IDataResult<string>> NajaPlaque(string nationalCode)
    {
        var _result = await _najaInquiry.Plaque(nationalCode);
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<NajaInquiryResponse>(_result.Data);
            if (_response.resultField == "0")
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            };
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }
    public async Task<IDataResult<string>> NajaCetificate(string nationalCode)
    {
        var _result = await _najaInquiry.Certificate(nationalCode);
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<NajaInquiryResponse>(_result.Data);
            if (_response.resultField == "1")
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            };
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }
    }

    public async Task<IDataResult<string>> PlaqueFromGSB(string licenceNo, string nationalCode, string registerDate)
    {
        var _result = await _najaInquiry.PlaqueFromGSB(new PlaqueFromGSBInput
        {
            licenceNo = licenceNo,
            nationalCode = nationalCode,
            registerDate = registerDate

        });
        if (_result.Success)
        {
            var _response = JsonConvert.DeserializeObject<NajaInquiryResponse>(_result.Data);
            if (_response.resultField == "1")
            {
                return new SuccsessDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
            };
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: "200");
        }
        else
        {
            return new ErrorDataResult<string>(data: _result.Data, message: _result.Message, messageId: _result.MessageId);
        }

    }
}