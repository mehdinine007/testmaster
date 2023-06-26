using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ICarTipService : IApplicationService
    {

        Task<PagedResultDto<CarTipDto>> GetCarTips(int pageNo, int sizeNo);
        Task<int> Save(CarTipDto carTipDto);
        Task<int> Update(CarTipDto carTipDto);
        Task<bool> Delete(int id);
        Task<List<CarTipDto>> GetAllCarTips();

    }
}
