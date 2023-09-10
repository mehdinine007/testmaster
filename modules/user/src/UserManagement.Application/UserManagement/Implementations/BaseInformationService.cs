using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations
{
    
    public class BaseInformationService : ApplicationService, IBaseInformationService
    {
        
        private IConfiguration _configuration { get; set; }
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<WhiteList, int> _whiteListRepository;

        public BaseInformationService(IConfiguration Configuration,
            IHttpContextAccessor httpContextAccessor,
            IRepository<WhiteList,int> whiteListRepository)
        {
            
            _configuration = Configuration;
            _httpContextAccessor = httpContextAccessor;
            _whiteListRepository = whiteListRepository;
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
}