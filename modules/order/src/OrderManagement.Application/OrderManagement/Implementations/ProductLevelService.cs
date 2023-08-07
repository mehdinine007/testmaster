using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
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

        public async Task<int> Save(ProductLevelDto productLevelDto)
        {
            var productLevel = ObjectMapper.Map<ProductLevelDto, ProductLevel>(productLevelDto);
            await _productLevelRepository.InsertAsync(productLevel, autoSave: true);
            return productLevel.Id;
        }

        public async Task<int> Update(ProductLevelDto productLevelDto)
        {
            var result = await _productLevelRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == productLevelDto.Id);
            if (result == null)
            {
                throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
            }
            var productLevel = ObjectMapper.Map<ProductLevelDto, ProductLevel>(productLevelDto);

            await _productLevelRepository.AttachAsync(productLevel, c => c.Title, c => c.Priority);

            return productLevel.Id;
        }
    }
}
