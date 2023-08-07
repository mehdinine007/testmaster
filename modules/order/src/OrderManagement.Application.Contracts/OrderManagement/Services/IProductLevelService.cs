using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductLevelService: IApplicationService
    {
        Task<List<ProductLevelDto>> GetList();
        Task<int> Save(ProductLevelDto productLevelDto);
        Task<int> Update(ProductLevelDto productLevelDto);
        Task<bool> Delete(int id);
    }
}
