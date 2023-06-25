using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IColorService: IApplicationService
    {

        Task<PagedResultDto<ColorDto>> GetColors(int pageNo, int sizeNo);
        Task<int> Save(ColorDto colorDto);
        Task<int> Update(ColorDto colorDto);
        Task<bool> Delete(int id);

    }
}
