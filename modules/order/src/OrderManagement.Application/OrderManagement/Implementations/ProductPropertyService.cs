using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application
{
    public class ProductPropertyService : ApplicationService, IProductPropertyService
    {
        private readonly IRepository<ProductProperty, Guid> _productPropertyRepository;
        public ProductPropertyService(IRepository<ProductProperty, Guid> productPropertyRepository)
        {
            _productPropertyRepository = productPropertyRepository;
        }
        public async Task<List<PropertyCategoryDto>> GetByProductId(int productId)
        {
            var productPropertyQuery = await _productPropertyRepository.GetQueryableAsync();
            var productProperty = productPropertyQuery
                .FirstOrDefault(x => x.ProductId == productId);
            if (productProperty == null)
                return null;
            return ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(productProperty.PropertyCategories); 
        }
    }
}
