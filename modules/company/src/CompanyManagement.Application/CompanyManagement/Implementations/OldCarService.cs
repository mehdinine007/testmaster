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
using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Permissions;
using Esale.Share.Authorize;
using FluentValidation;
using CompanyManagement.Application.Contracts.CompanyManagement.FluentValidation;
using Volo.Abp;
using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation;
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
            var oldCarQuery = await _oldCarRepository.GetQueryableAsync();
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
        public async Task<bool> Delete(string nationalcode)
        {
            var oldCar = (await _oldCarRepository.GetQueryableAsync()).FirstOrDefault(x => x.Nationalcode == nationalcode);
            await _oldCarRepository.DeleteAsync(oldCar.Id);
            return true;
        }
        [SecuredOperation(OldCarServicePermissionConstants.Inquiry)]
        public async Task<List<OldCarDto>> Inquiry(string nationalcode)
        {
            var oldCars = (await _oldCarRepository.GetQueryableAsync()).Where(x => x.Nationalcode == nationalcode).ToList();
            var oldCarDto = ObjectMapper.Map<List<OldCar>, List<OldCarDto>>(oldCars);
            return oldCarDto;

        }
    }
}
