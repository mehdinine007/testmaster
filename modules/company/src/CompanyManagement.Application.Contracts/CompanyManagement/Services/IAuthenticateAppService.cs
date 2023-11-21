using CompanyManagement.Domain.CompanyManagement;
using IFG.Core.Infrastructures.TokenAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services;


public interface IAuthenticateAppService : IApplicationService
{
    Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto model);
}
