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
using StackExchange.Redis;

namespace OrderManagement.Application
{
    public class UserDataAccessService : ApplicationService, IUserDataAccessService
    {
        private readonly IEsaleGrpcClient _grpcClient;
        public UserDataAccessService(IEsaleGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        private async Task<List<UserDataAccessDto>> GetByNationalCode(string nationalCode, RoleTypeEnum roleType)
        {
            return await _grpcClient.GetUserDataAccessByNationalCode(nationalCode, roleType);
        }
        public async Task<List<OldCarDto>> OldCarGetList(string nationalcode)
        {
            var userDataAccess = await _grpcClient.GetUserDataAccessByNationalCode(nationalcode, RoleTypeEnum.OldCar);
            List<OldCarDto> oldCarDtoList = new List<OldCarDto>();
            userDataAccess.ForEach(x =>
            {
                OldCarDto oldCarDto = new OldCarDto();
                oldCarDto = JsonConvert.DeserializeObject<OldCarDto>(x.Data);
                oldCarDtoList.Add(oldCarDto);
            });
            return oldCarDtoList;

        }

        public async Task<IResult> CheckOldCar(string nationalcode, string engineNo, string vin, string chassiNo)
        {
            var oldCars = await OldCarGetList(nationalcode);
            var oldCar = oldCars.FirstOrDefault();
            if (oldCar != null)
            {
                if (oldCar.ChassiNo == chassiNo && oldCar.EngineNo == engineNo && oldCar.Vin == vin)
                {
                    return new SuccsessResult();

                }
            }
            return new ErrorResult(OrderConstant.UserDataAccessOldCarNotFound, OrderConstant.UserDataAccessOldCarNotFoundId);
        }

        public async Task<IResult> CheckProductAccess(string nationalCode, int productId)
        {
            var products = await ProductGetList(nationalCode);
            if (products.Count == 0 || !products.Any(x => x.ProductId == productId))
            {
                return new ErrorResult(OrderConstant.UserDataAccessProductNotFound, OrderConstant.UserDataAccessProductNotFoundId);
            }
            return new SuccsessResult();
        }

        public async Task<List<UserDataAccessProductDto>> ProductGetList(string nationalCode)
        {
            var getProducts = await GetByNationalCode(nationalCode, RoleTypeEnum.ProductAccess);
            if (getProducts == null || getProducts.Count == 0)
                return new List<UserDataAccessProductDto>();
            return JsonConvert.DeserializeObject<List<UserDataAccessProductDto>>(getProducts.FirstOrDefault().Data);
        }

        public async Task<bool> Exists(string nationalcode, RoleTypeEnum roleType)
        {
            var userDataAccess = await GetByNationalCode(nationalcode,roleType);
            return userDataAccess.Any();
        }
    }
}
