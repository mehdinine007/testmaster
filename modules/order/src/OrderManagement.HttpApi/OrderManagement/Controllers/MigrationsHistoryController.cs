using Licence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.HttpApi.UserManagement.Controllers
{
    [RemoteService]
    [Route("api/services/app/[controller]/[action]")]
    public class MigrationsHistoryController : Controller
    {
        private readonly IMigrationsHistoryService _migrationsHistoryService;
        public MigrationsHistoryController(IMigrationsHistoryService migrationsHistoryService)
        {
            _migrationsHistoryService = migrationsHistoryService;
        }

        [HttpPost]
        public async Task<bool> UpdateDataBase()
        {
            return await _migrationsHistoryService.UpdateDatabase();
        }

    }
}
