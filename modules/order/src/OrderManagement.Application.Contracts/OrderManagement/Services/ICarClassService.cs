using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ICarClassService: IApplicationService
    {
        Task<List<CarClassDto>> GetCarClass();
        Task<CarClassDto> Save(CarClassDto carClassDto);
        Task<CarClassDto> Update(CarClassDto carClassDto);
        Task<bool> Delete(int id);
    }
}
