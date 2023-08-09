using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAgencySaleDetailService: IApplicationService
    {
        Task<PagedResultDto<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId, int pageNo, int sizeNo);

        Task<AgencySaleDetailListDto> GetBySaleDetailId(int saleDetailId, int? agancyId);
        Task<List<AgencySaleDetailForCapacityControlDto>> GetAgeneciesBySaleDetail(int saleDetailId);

        long GetReservCount(int saleDetailId);
        Task<int> Save(AgencySaleDetailDto agencySaleDetailDto);
        Task<bool> Delete(int id);


    }
}
