using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductAndCategoryService : IApplicationService
    {
        Task<ProductAndCategoryDto> GetById(int id);

        Task<ProductAndCategoryDto> Insert(ProductAndCategoryDto productAndCategoryDto);

        Task Delete(int id);

        Task<bool> UploadFile(UploadFileDto uploadFileDto);

        Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input);
    }
}
