using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.CompanyManagement;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using System.Linq;
using Esale.Share.Authorize;
using FluentValidation;
using CompanyManagement.Application.Contracts.CompanyManagement.FluentValidation;
using Volo.Abp;
using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation;
using Microsoft.EntityFrameworkCore;
using Permission.Company;
namespace CompanyManagement.Application.CompanyManagement.Implementations
{
    public class OldCarService : ApplicationService, IOldCarService
    {
        private readonly IRepository<OldCar, int> _oldCarRepository;
        private readonly IValidator<OldCarCreateDtoList> _oldCarValidator;

        public OldCarService(IRepository<OldCar, int> oldCarRepository, IValidator<OldCarCreateDtoList> oldCarValidator)
        {
            _oldCarRepository = oldCarRepository;
            _oldCarValidator = oldCarValidator;

        }
        [SecuredOperation(OldCarServicePermissionConstants.AddList)]
        public async Task<bool> AddList(OldCarCreateDtoList oldCarCreateDto)
        {


            var oldCars = ObjectMapper.Map<List<OldCarCreateDto>, List<OldCar>>(oldCarCreateDto.OldCars, new List<OldCar>());
            var oldCarQuery = (await _oldCarRepository.GetQueryableAsync()).AsNoTracking();
            var lastBatchNo = 0;
            if (oldCarQuery.Any())
            {
                lastBatchNo = oldCarQuery.Max(x => x.BatchNo);
                lastBatchNo++;
            }
            else
            {
                lastBatchNo = 1;
            }

            oldCars.ForEach(x =>
            {
                x.BatchNo = lastBatchNo;
            });

            await _oldCarRepository.InsertManyAsync(oldCars);
            return true;
        }
        [SecuredOperation(OldCarServicePermissionConstants.Delete)]
        public async Task<bool> Delete(OldCarQueryDto oldCarQueryDto)
        {
            var ids = (await _oldCarRepository.GetQueryableAsync()).AsNoTracking()
                .Where(x => x.Nationalcode == oldCarQueryDto.NationalCode)
                .Select(x => x.Id);
            await _oldCarRepository.DeleteManyAsync(ids);
            return true;
        }
        [SecuredOperation(OldCarServicePermissionConstants.Inquiry)]
        public async Task<OldCarDto> Inquiry(OldCarQueryDto oldCarQueryDto)
        {
            var oldCars = (await _oldCarRepository.GetQueryableAsync()).AsNoTracking().
                OrderByDescending(x => x.Id).FirstOrDefault(x => x.Nationalcode == oldCarQueryDto.NationalCode);
            var oldCarDto = ObjectMapper.Map<OldCar, OldCarDto>(oldCars);
            return oldCarDto;

        }
    }
}
