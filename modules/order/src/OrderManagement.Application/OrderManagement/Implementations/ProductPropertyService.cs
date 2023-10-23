using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application
{
    public class ProductPropertyService : ApplicationService, IProductPropertyService
    {
        private readonly IRepository<ProductProperty, ObjectId> _productPropertyRepository;
        public ProductPropertyService(IRepository<ProductProperty, ObjectId> productPropertyRepository)
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
        public async Task<List<ProductPropertyDto>> GetList()
        {
            List<ProductProperty> productProperty = new();
            var productPropertyQuery = await _productPropertyRepository.GetQueryableAsync();
            productProperty = productPropertyQuery.ToList();
            var getProductProperty = ObjectMapper.Map<List<ProductProperty>, List<ProductPropertyDto>>(productProperty);
            return getProductProperty;
        }
        public async Task<ProductPropertyDto> GetById(string Id)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(Id, out objectId))
            {
                var productLevel = (await _productPropertyRepository.GetQueryableAsync())
               .FirstOrDefault(x => x.Id == objectId);
                return ObjectMapper.Map<ProductProperty, ProductPropertyDto>(productLevel);
            }
            else
            {
                throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);
            }
        }
        public async Task<ProductPropertyDto> Add(ProductPropertyDto productPropertyDto)
        {
            var productPropertyQuery = await _productPropertyRepository.GetQueryableAsync();
            var getProductProperty = productPropertyQuery.FirstOrDefault(x => x.ProductId == productPropertyDto.ProductId);
            if (getProductProperty != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }
            if (productPropertyDto.ProductId <= 0)
            {
                throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
            }
            var mapProductPropertyDto = ObjectMapper.Map<ProductPropertyDto, ProductProperty>(productPropertyDto);
            var entity = await _productPropertyRepository.InsertAsync(mapProductPropertyDto, autoSave: true);
            return ObjectMapper.Map<ProductProperty, ProductPropertyDto>(entity);
        }
        public async Task<ProductPropertyDto> Update(ProductPropertyDto productPropertyDto)
        {
            var existingEntity = await _productPropertyRepository.FindAsync(x => x.ProductId == productPropertyDto.ProductId);
            if (existingEntity == null)
            {
                throw new UserFriendlyException(OrderConstant.ProductLevelNotFound, OrderConstant.ProductLevelNotFoundId);
            }

            var duplicateProductProperty = await _productPropertyRepository.FirstOrDefaultAsync(x => x.ProductId != existingEntity.ProductId && x.ProductId == productPropertyDto.ProductId);
            if (duplicateProductProperty != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }

            if (productPropertyDto.ProductId <= 0)
            {
                throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
            }

            existingEntity.PropertyCategories = ObjectMapper.Map<List<ProductPropertyCategoryDto>, List<PropertyCategory>>(productPropertyDto.PropertyCategories);

            await _productPropertyRepository.UpdateAsync(existingEntity, autoSave: true);

            return ObjectMapper.Map<ProductProperty, ProductPropertyDto>(existingEntity);
        }
        public async Task<bool> Delete(string Id)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(Id, out objectId))
            {
                await _productPropertyRepository.DeleteAsync(x => x.Id == objectId, autoSave: true);
                return true;
            }
            else
            {
                throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);

            }
        }
    }
}
