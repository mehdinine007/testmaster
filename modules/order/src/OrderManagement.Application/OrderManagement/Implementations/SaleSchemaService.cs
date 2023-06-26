using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class SaleSchemaService : ApplicationService, ISaleSchemaService
    {
        private readonly IRepository<SaleSchema> _saleSchemaRepository;
        public SaleSchemaService(IRepository<SaleSchema> saleSchemaRepository)
        {
            _saleSchemaRepository = saleSchemaRepository;
          
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SaleSchemaDto>> GetAllSaleSchema()
        {
            var saleSchema = await _saleSchemaRepository.GetListAsync();
            var saleSchemaDto = ObjectMapper.Map<List<SaleSchema>, List<SaleSchemaDto>>(saleSchema);
            return saleSchemaDto;
        }

        public Task<PagedResultDto<SaleSchemaDto>> GetSaleSchema(int pageNo, int sizeNo)
        {
            throw new NotImplementedException();
        }

        public Task<int> Save(SaleSchemaDto saleSchemaDto)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(SaleSchemaDto saleSchemaDto)
        {
            throw new NotImplementedException();
        }
    }
}
