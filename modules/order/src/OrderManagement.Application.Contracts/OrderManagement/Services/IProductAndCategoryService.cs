using OrderManagement.Domain.Shared;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductAndCategoryService : IApplicationService
    {
        Task<ProductAndCategoryWithChildDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType, bool hasProperty);

        Task<ProductAndCategoryDto> Insert(ProductAndCategoryCreateDto productAndCategoryCreateDto);

        Task Delete(int id);

        Task<Guid> UploadFile(UploadFileDto uploadFileDto);

        Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input);

        Task<List<ProductAndCategoryWithChildDto>> GetList(ProductAndCategoryGetListQueryDto input);

        Task<ProductAndCategoryDto> Update(ProductAndCategoryUpdateDto productAndCategoryUpdateDto);

        Task<List<ProductAndCategoryWithChildDto>> GetProductAndSaleDetailList(ProductAndSaleDetailGetListQueryDto input);

        Task<List<FilterParamDto>> GetFilterParamList();
    }
}
