using MongoDB.Bson;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application
{
    public class UserDataAccessService : ApplicationService, IUserDataAccessService
    {
        private readonly IEsaleGrpcClient _grpcClient;
        public UserDataAccessService(IEsaleGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<List<OldCarDto>> OldCarGetList(string Nationalcode)
        {
            var userDataAccess = await _grpcClient.GetUserDataAccessByNationalCode(Nationalcode, RoleTypeEnum.OldCar);
            var data = userDataAccess.Select(x => x.Data).ToList();
            var concatenatedData = string.Join("", data);
            var oldCarDto = JsonConvert.DeserializeObject<List<OldCarDto>>(concatenatedData);
        }

        public Task<List<UserDataAccessDto>> CheckOldCar(string nationalcode, string engineNo, string vin, string vehicle, string chassiNo)
        {
            throw new NotImplementedException();
        }

      
    }
}
