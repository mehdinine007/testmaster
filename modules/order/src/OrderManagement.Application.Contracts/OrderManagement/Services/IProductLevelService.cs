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
        Task<ProductLevelDto> GetById(int id);
        Task<ProductLevelDto> Add(ProductLevelDto productLevelDto);
        Task<ProductLevelDto> Update(ProductLevelDto productLevelDto);
        Task<bool> Delete(int id);
    }
}
