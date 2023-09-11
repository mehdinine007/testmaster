using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using Volo.Abp.Application.Services;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp;
using WorkingWithMongoDB.WebAPI.Services;
using MongoDB.Driver;

namespace UserManagement.Application.Implementations;

public class BaseInformationService : ApplicationService, IBaseInformationService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<AdvocacyUsers, int> _advocacyUsersRepository;
    private readonly UserMongoService _userMongoService;

    public BaseInformationService(IConfiguration configuration,
                                  IRepository<AdvocacyUsers , int> advocacyUsersRepository,
                                  UserMongoService userMongoService
        )
    {
        _configuration = configuration;
        _advocacyUsersRepository = advocacyUsersRepository;
        _userMongoService = userMongoService;
    }


    public async void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input)

    {

        if (_configuration.GetSection("IsCheckAdvocacy").Value == "1")
        {
            var advocacyuser = (await _advocacyUsersRepository.GetQueryableAsync())
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


        var user = (_userMongoService.GetUserCollectionSync())
            .Find(x => x.NormalizedUserName == input.Nationalcode
            && x.IsDeleted == false)
         .Project(x => x.NationalCode)
         .FirstOrDefault();
        if (user != null)
        {

            throw new UserFriendlyException("این کد ملی قبلا ثبت نام شده است");
        }

    }
}