using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AgencyService : ApplicationService, IAgencyService
    {
        private readonly IRepository<Agency> _agencyRepository;

        public AgencyService(IRepository<Agency> agencyRepository)
        {
            _agencyRepository = agencyRepository;
        }



        public async Task<int> Delete(int id)
        {
            await _agencyRepository.DeleteAsync(x => x.Id == id);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<List<AgencyDto>> GetAgencies()
        {
            var agencies = await _agencyRepository.WithDetailsAsync();
            var queryResult = agencies.ToList();
            var agencyDto = ObjectMapper.Map<List<Agency>, List<AgencyDto>>(queryResult);
            return agencyDto;
        }
        [UnitOfWork]
        public async  Task<int> Save(AgencyDto agencyDto)
        {
            var agency = ObjectMapper.Map<AgencyDto, Agency>(agencyDto);
            await _agencyRepository.InsertAsync(agency);
            await CurrentUnitOfWork.SaveChangesAsync();
            return agency.Id;
        }

        public async Task<int> Update(AgencyDto agencyDto)
        {
            var agency = ObjectMapper.Map<AgencyDto, Agency>(agencyDto);
            await _agencyRepository.UpdateAsync(agency);
            await CurrentUnitOfWork.SaveChangesAsync();
            return agency.Id;
        }
    }
}
