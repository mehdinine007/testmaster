using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductAndCategoryService : IApplicationService
    {
        Task<ProductAndCategoryWithChildDto> GetById(int id, bool hasProperty, List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum>? attachmentlocation = null);

        Task<ProductAndCategoryDto> Insert(ProductAndCategoryCreateDto productAndCategoryCreateDto);

        Task<ProductAndCategoryAddDto> GetByAdd(int? productId);
        Task Delete(int id);

        Task<Guid> UploadFile(UploadFileDto uploadFileDto);

        Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input);

        Task<List<ProductAndCategoryWithChildDto>> GetList(ProductAndCategoryGetListQueryDto input);

        Task<ProductAndCategoryDto> Update(ProductAndCategoryUpdateDto productAndCategoryUpdateDto);

        Task<List<ProductAndCategoryWithChildDto>> GetProductAndSaleDetailList(ProductAndSaleDetailGetListQueryDto input);

        Task<List<FilterParamDto>> GetFilterParamList();

        Task<List<ProductAndCategoryDto>> GetAllParent();

        Task<ProductAndCategoryDto> GetProductAndCategoryByCode(string code, List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum>? attachmentlocation = null);
        Task<bool> Move(MoveDto move);
        Task<bool> Import(ImportExcelDto importExcelDto);
        Task<int> GetCompanyIdByCompanyCode(string code);
    }
}
