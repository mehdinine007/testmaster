using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using Volo.Abp.Application.Services;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp;
using WorkingWithMongoDB.WebAPI.Services;
using MongoDB.Driver;
using System.Security.Claims;
using UserManagement.Domain.UserManagement.Bases;
using Microsoft.AspNetCore.Http;
using UserManagement.Domain.Authorization.Users;
using MongoDB.Bson;

namespace UserManagement.Application.Implementations;

public class BaseInformationService : ApplicationService, IBaseInformationService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<AdvocacyUsers, int> _advocacyUsersRepository;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<WhiteList, int> _whiteListRepository;

    public BaseInformationService(IConfiguration configuration,
                                  IRepository<AdvocacyUsers, int> advocacyUsersRepository,
                                  UserMongoService userMongoService,
                                  IHttpContextAccessor httpContextAccessor,
                                  IRepository<WhiteList, int> whiteListRepository,
                                  IRepository<UserMongo, ObjectId> userMongoRepository
        )
    {
        _configuration = configuration;
        _advocacyUsersRepository = advocacyUsersRepository;
        _userMongoRepository = userMongoRepository;
        _httpContextAccessor = httpContextAccessor;
        _whiteListRepository = whiteListRepository;
    }

    public async void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input)
    {
        if (_configuration.GetSection("IsCheckAdvocacy").Value == "1")
        {
            var advocacyuser = _advocacyUsersRepository.WithDetails()
          .Select(x => new
          {
              x.shabaNumber,
              x.accountNumber,
              x.Id,
              x.nationalcode,
              x.BanksId
          })
          .OrderByDescending(x => x.Id).FirstOrDefault(x => x.nationalcode == input.Nationalcode);

            if (advocacyuser == null)
            {
                throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
            }
        }

        var user = (await _userMongoRepository
                    .GetQueryableAsync())
                    .FirstOrDefault(a => a.NormalizedUserName == input.Nationalcode && a.IsDeleted == false);
        if (user != null)
        {

            throw new UserFriendlyException("این کد ملی قبلا ثبت نام شده است");
        }
    }


    public async Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode)
    {
        if (_configuration.GetSection(whiteListEnumType.ToString()).Value == "1")
        {
            if (string.IsNullOrEmpty(Nationalcode))
            {
                Console.WriteLine("dakhel");
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                // Get the claims values
                string nc = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                if (Nationalcode == null)
                {
                    throw new UserFriendlyException("کد ملی صحیح نمی باشد");
                }
                Nationalcode = nc;
            }
            Console.WriteLine("biron");

            var WhiteList = (await _whiteListRepository.GetQueryableAsync())
                .Select(x => new { x.NationalCode, x.WhiteListType })
                .FirstOrDefault(x => x.NationalCode == Nationalcode && x.WhiteListType == whiteListEnumType);
            if (WhiteList == null)
            {
                //unitOfWork.Complete();
                throw new UserFriendlyException(_configuration.GetValue<string>("ErrorMessages:InsertUserRejectionAdvocacy"));
            }
            //unitOfWork.Complete();
        }

        return true;
    }
}