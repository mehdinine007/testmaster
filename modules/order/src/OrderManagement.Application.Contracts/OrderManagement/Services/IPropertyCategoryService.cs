using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IPropertyCategoryService : IApplicationService
    {
        Task<List<PropertyCategoryDto>> GetList();
        Task<PropertyCategoryDto> GetById(ObjectId id);
        Task<PropertyCategoryDto> Add(PropertyCategoryDto propertyCategoryDto);
        Task<PropertyCategoryDto> Update(PropertyCategoryDto propertyCategoryDto);
        Task<bool> Delete(ObjectId Id);
    }
}
