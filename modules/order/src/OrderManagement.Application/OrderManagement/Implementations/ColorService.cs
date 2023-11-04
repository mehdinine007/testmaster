using IFG.Core.DataAccess;
using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class ColorService : ApplicationService, IColorService
{

    private readonly IRepository<Domain.OrderManagement.Color> _colorRepository;
 

    public ColorService(IRepository<Domain.OrderManagement.Color> colorRepository)
    {
        _colorRepository = colorRepository;
    
    }

    [SecuredOperation(ColorServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        await _colorRepository.DeleteAsync(x => x.Id == id, autoSave: true);
        return true;
    }

    [SecuredOperation(ColorServicePermissionConstants.GetAllColors)]
    public async Task<List<ColorDto>> GetAllColors()
    {
        var Colors = await _colorRepository.GetListAsync();
        var ColorDto = ObjectMapper.Map<List<Domain.OrderManagement.Color>, List<ColorDto>>(Colors);
        return ColorDto;
    }

    [SecuredOperation(ColorServicePermissionConstants.GetColors)]
    public async Task<PagedResultDto<ColorDto>> GetColors(int pageNo, int sizeNo)
    {
        var count = await _colorRepository.CountAsync();
        var colors = await _colorRepository.WithDetailsAsync();
        var queryResult = await colors.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
        return new PagedResultDto<ColorDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<Domain.OrderManagement.Color>, List<ColorDto>>(queryResult)
        }; 
    }

    [SecuredOperation(ColorServicePermissionConstants.Save)]
    public async Task<int> Save(ColorDto colorDto)
    {
        var color = ObjectMapper.Map<ColorDto, Domain.OrderManagement.Color>(colorDto);
        await _colorRepository.InsertAsync(color, autoSave: true);
        return color.Id;
    }

    [SecuredOperation(ColorServicePermissionConstants.Update)]
    public async Task<int> Update(ColorDto colorDto)

    {
        var result = await _colorRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == colorDto.Id);
        if (result == null)
        {
            throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
        }
        var color = ObjectMapper.Map<ColorDto, Domain.OrderManagement.Color>(colorDto);

        await _colorRepository.AttachAsync(color, c => c.ColorCode, c=>c.ColorName);

        return color.Id;
    }
}
