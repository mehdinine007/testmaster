using IFG.Core.Utility.Results;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
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

        public async Task<List<OldCarDto>> OldCarGetList(string nationalcode)
        {
            var userDataAccess = await _grpcClient.GetUserDataAccessByNationalCode(nationalcode, RoleTypeEnum.OldCar);
            var data = userDataAccess.Select(x => x.Data).ToList();
            var concatenatedData = string.Join("", data);
            var oldCarDto = JsonConvert.DeserializeObject<List<OldCarDto>>(concatenatedData);
            return oldCarDto;
        }

        public Task<List<UserDataAccessDto>> CheckOldCar(string nationalcode, string engineNo, string vin, string vehicle, string chassiNo)
        {
            throw new NotImplementedException();
        }

      
   
        public async Task<IResult> CheckProductAccess(string nationalCode, int productId)
        {
            var products = await ProductGetList(nationalCode);
            if (products.Count == 0 || !products.Any(x=> x.ProductId == productId))
            {
                return new ErrorResult(OrderConstant.UserDataAccessProductNotFound, OrderConstant.UserDataAccessProductNotFoundId);
            }
            return new SuccsessResult();
        }

        public async Task<List<UserDataAccessProductDto>> ProductGetList(string nationalCode)
        {
            var getProducts = await _grpcClient.GetUserDataAccessByNationalCode(nationalCode, RoleTypeEnum.ProductAccess);
            if (getProducts == null || getProducts.Count == 0)
                return new List<UserDataAccessProductDto>();
            return JsonConvert.DeserializeObject<List<UserDataAccessProductDto>>(getProducts.FirstOrDefault().Data); 
        }
    }
}
