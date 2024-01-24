using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface IOldCarService: IApplicationService
    {

      public Task<bool> AddList(OldCarCreateDtoList oldCarCreateDto);
      public Task<List<OldCarDto>> Inquiry(OldCarQueryDto oldCarQueryDto);
      public Task<bool> Delete(OldCarQueryDto oldCarQueryDto);

    }
}
