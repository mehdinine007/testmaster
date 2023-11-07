using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.Models;
using MongoDB.Bson;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/MenuController/[action]")]
    //[UserAuthorization]
    public class MenuController : Controller, IMenuAppService
    {
        private readonly IMenuAppService _menuAppService;

        public MenuController(IMenuAppService menuAppService)
            => _menuAppService = menuAppService;


        [HttpGet]
        public async Task<List<MenuDto>> GetList()
            => await _menuAppService.GetList();

        [HttpGet]
        public async Task<MenuDto> GetById(ObjectId Id)
         => await _menuAppService.GetById(Id);

        [HttpPost]
        public async Task<MenuDto> Add(MenuDto menu)
      => await _menuAppService.Add(menu);


        [HttpPut]
        public async Task<MenuDto> Update(MenuDto input)
            => await _menuAppService.Update(input);

        [HttpDelete]
        public async Task<bool> Delete(ObjectId Id)
                => await _menuAppService.Delete(Id);

        [HttpPost]
        public async Task InsertList()
            => await _menuAppService.InsertList();
   
    }
}
