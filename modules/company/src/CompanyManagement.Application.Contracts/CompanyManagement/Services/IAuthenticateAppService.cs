using CompanyManagement.Application.Contracts.CompanyManagement.Dtos;
using CompanyManagement.Domain.CompanyManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services;


public interface IAuthenticateAppService : IApplicationService
{
    Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto model);
}
