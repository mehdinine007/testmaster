using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class ProductLevelService : ApplicationService, IProductLevelService
    {
        private readonly IRepository<ProductLevel> _productLevelRepository;
        public ProductLevelService(IRepository<ProductLevel> productLevelRepository)
        {
            _productLevelRepository = productLevelRepository;

        }


        public async Task<bool> Delete(int id)
        {
            await _productLevelRepository.DeleteAsync(x => x.Id == id, autoSave: true);
            return true;
        }

        public async Task<List<ProductLevelDto>> GetList()
        {
            var productLevels = await _productLevelRepository.GetListAsync();
            var productLevelsDto = ObjectMapper.Map<List<ProductLevel>, List<ProductLevelDto>>(productLevels);
            return productLevelsDto;
        }

        public async Task<ProductLevelDto> Add(ProductLevelDto productLevelDto)
        {
            var productLevelQuery = await _productLevelRepository.GetQueryableAsync();
            var getProductLevel = productLevelQuery.FirstOrDefault(x => x.Priority == productLevelDto.Priority);
            if (getProductLevel != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }
            if (productLevelDto.Priority <= 0)
            {
                throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
            }
            var productLevel = ObjectMapper.Map<ProductLevelDto, ProductLevel>(productLevelDto);
          var entity= await _productLevelRepository.InsertAsync(productLevel, autoSave: true);
            return ObjectMapper.Map<ProductLevel, ProductLevelDto>(entity);
        }

        public async Task<ProductLevelDto> Update(ProductLevelDto productLevelDto)
        {
            var productLevelQuery = await _productLevelRepository.GetQueryableAsync();

            var result =  productLevelQuery.AsNoTracking().FirstOrDefault(x => x.Id == productLevelDto.Id);
            if (result == null)
            {
                throw new UserFriendlyException(OrderConstant.ProductLevelNotFound, OrderConstant.ProductLevelNotFoundId);
            }

          
            var getProductLevel = productLevelQuery.FirstOrDefault(x => x.Id != productLevelDto.Id &&  x.Priority == productLevelDto.Priority);
            if (getProductLevel != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }
            if (productLevelDto.Priority <= 0)
            {
                throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
            }


            var productLevel = ObjectMapper.Map<ProductLevelDto, ProductLevel>(productLevelDto);

            var entity= await _productLevelRepository.AttachAsync(productLevel, c => c.Title, c => c.Priority);

            return ObjectMapper.Map<ProductLevel, ProductLevelDto>(entity);
        }
        public async Task<ProductLevelDto> GetById(int id)
        {
            var productLevel = (await _productLevelRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            return ObjectMapper.Map<ProductLevel, ProductLevelDto>(productLevel);
        }

    }
}
