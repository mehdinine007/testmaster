using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class SchemeService : ApplicationService, ISchemeService
    {

        private readonly IRepository<Scheme, int> _schemeRepository;
        public SchemeService(IRepository<Scheme, int> schemeRepository)
        {
            _schemeRepository = schemeRepository;
        }



        public async Task<SchemeDto> Add(SchemeCreateOrUpdateDto schemeCreateOrUpdateDto)
        {
            await Validation(null, schemeCreateOrUpdateDto);
            var scheme = ObjectMapper.Map<SchemeCreateOrUpdateDto, Scheme>(schemeCreateOrUpdateDto);
            var entity = await _schemeRepository.InsertAsync(scheme, autoSave: true);
            return ObjectMapper.Map<Scheme, SchemeDto>(entity);


        }

        public async Task<bool> Delete(int id)
        {
            var scheme = await Validation(id, null);
            await _schemeRepository.DeleteAsync(id);
            return true;
        }

        public async Task<SchemeDto> GetById(int id)
        {
            var scheme = await Validation(id, null);
            var schemeDto = ObjectMapper.Map<Scheme, SchemeDto>(scheme);
            return schemeDto;
        }

        public async Task<List<SchemeDto>> GetList()
        {
            var scheme = (await _schemeRepository.GetQueryableAsync()).ToList();
            var schemeDto = ObjectMapper.Map<List<Scheme>, List<SchemeDto>>(scheme);
            return schemeDto;
        }

        public async Task<SchemeDto> Update(SchemeCreateOrUpdateDto schemeCreateOrUpdateDto)
        {
            var scheme = await Validation(schemeCreateOrUpdateDto.Id, schemeCreateOrUpdateDto);
            scheme.Status = schemeCreateOrUpdateDto.Status;
            scheme.Title = schemeCreateOrUpdateDto.Title;
            scheme.Priority = schemeCreateOrUpdateDto.Priority;

            var entity = await _schemeRepository.UpdateAsync(scheme);
            return ObjectMapper.Map<Scheme, SchemeDto>(entity);
        }

        private async Task<Scheme> Validation(int? id, SchemeCreateOrUpdateDto schemeCreateOrUpdateDto)
        {
            var scheme = new Scheme();
            var schemeQuery = (await _schemeRepository.GetQueryableAsync());
            if (id != null)
            {
                scheme = schemeQuery.FirstOrDefault(x => x.Id == id);
                if (scheme is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.SchemeNotFound, WorkFlowConstant.SchemeNotFoundId);
                }
            }
            

            return scheme;
        }



    }
}
