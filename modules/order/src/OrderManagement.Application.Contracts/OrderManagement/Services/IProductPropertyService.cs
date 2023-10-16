using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductPropertyService : IApplicationService
    {
        Task<List<PropertyCategoryDto>> GetByProductId(int productId);
        Task<List<ProductPropertyDto>> GetList();
        Task<ProductPropertyDto> GetById(ObjectId id);
        Task<ProductPropertyDto> Add(ProductPropertyDto productPropertylDto);
        Task<ProductPropertyDto> Update(ProductPropertyDto productPropertylDto);
        Task<bool> Delete(ObjectId id);
    }
}
