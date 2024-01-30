using IFG.Core.Utility.Results;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
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
        private readonly IEsaleGrpcClient _esaleGrpcClient;
        public UserDataAccessService(IEsaleGrpcClient esaleGrpcClient)
        {
            _esaleGrpcClient = esaleGrpcClient;
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
            var getProducts = await _esaleGrpcClient.GetUserDataAccessByNationalCode(nationalCode, RoleTypeEnum.ProductAccess);
            if (getProducts == null || getProducts.Count == 0)
                return new List<UserDataAccessProductDto>();
            return JsonConvert.DeserializeObject<List<UserDataAccessProductDto>>(getProducts.FirstOrDefault().Data); 
        }
    }
}
