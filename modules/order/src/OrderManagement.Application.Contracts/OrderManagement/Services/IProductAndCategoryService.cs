using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductAndCategoryService : IApplicationService
    {
        Task<ProductAndCategoryDto> Get(int id);

        Task<ProductAndCategoryDto> Insert(ProductAndCategoryDto productAndCategoryDto);

        Task Delete(int id);
    }
}
