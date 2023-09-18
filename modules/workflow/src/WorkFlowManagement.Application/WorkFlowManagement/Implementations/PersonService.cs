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
    public class PersonService : ApplicationService, IPersonService
    {
        private readonly IRepository<Person, Guid> _PersonRepository;
        public PersonService(IRepository<Person, Guid> PersonRepository)
        {
            _PersonRepository = PersonRepository;
        }

        public async Task<PersonDto> Add(PersonCreateOrUpdateDto personCreateOrUpdateDto)
        {
            var person = ObjectMapper.Map<PersonCreateOrUpdateDto,Person>(personCreateOrUpdateDto);
            var entity = await _PersonRepository.InsertAsync(person, autoSave: true);
            return ObjectMapper.Map<Person, PersonDto>(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            var person = await Validation(id, null);
            await _PersonRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PersonDto> GetById(Guid id)
        {
            var personQuery = (await _PersonRepository.GetQueryableAsync());
            var person = personQuery.FirstOrDefault(x => x.Id == id);
            var personDto = ObjectMapper.Map<Person, PersonDto>(person);
            return personDto;
        }

       

        public async  Task<List<PersonDto>> GetList(int activityId)
        {
            var person = (await _PersonRepository.GetQueryableAsync()).ToList();
            var personDto = ObjectMapper.Map<List<Person>, List<PersonDto>>(person);
            return personDto;
        }

        public async Task<PersonDto> Update(PersonCreateOrUpdateDto personCreateOrUpdateDto)
        {
            var person = await Validation(personCreateOrUpdateDto.Id, personCreateOrUpdateDto);
            person.Title = personCreateOrUpdateDto.Title;
            person.NationalCode = personCreateOrUpdateDto.NationalCode;
            var entity = await _PersonRepository.UpdateAsync(person);
            return ObjectMapper.Map<Person, PersonDto>(entity);
        }

        private async Task<Person> Validation(Guid? id, PersonCreateOrUpdateDto personCreateOrUpdateDto)
        {
            var person = new Person();
            var personQuery = (await _PersonRepository.GetQueryableAsync());
            if (id != null)
            {
                person = personQuery.FirstOrDefault(x => x.Id == id);
                if (person is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ProcessNotFound, WorkFlowConstant.ProcessNotFoundId);
                }
            }
            return person;
        }



    }
}
