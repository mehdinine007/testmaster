using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AgencyService : ApplicationService, IAgencyService
    {
        private readonly IRepository<Agency> _agencyRepository;
        private readonly IRepository<Province> _provinceRepository;

        public AgencyService(IRepository<Agency> agencyRepository, IRepository<Province> provinceRepository)
        {
            _agencyRepository = agencyRepository;
            _provinceRepository = provinceRepository;
        }



        public async Task Delete(int id)
        {
            await _agencyRepository.DeleteAsync(x => x.Id == id, autoSave: true);
            throw new UserFriendlyException("حذف با موفقیت انجام شد.");
        }

        public async Task<PagedResultDto<AgencyDto>> GetAgencies(int pageNo, int sizeNo)
        {
            var count = await _agencyRepository.CountAsync();
            var agencies = await _agencyRepository.WithDetailsAsync(x => x.Province);
            var queryResult = await agencies.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
            return new PagedResultDto<AgencyDto>
            {
                TotalCount = count,
                Items = ObjectMapper.Map<List<Agency>, List<AgencyDto>>(queryResult)
            };

        }
        [UnitOfWork]
        public async Task<int> Save(AgencyDto agencyDto)
        {
            var province = await _provinceRepository.FirstOrDefaultAsync(x => x.Id == agencyDto.ProvinceId);
            if (province == null || agencyDto.ProvinceId <= 0)
            {
                throw new UserFriendlyException("استان وجود ندارد.");
            }
            var agency = ObjectMapper.Map<AgencyDto, Agency>(agencyDto);
            await _agencyRepository.InsertAsync(agency, autoSave: true);
            return agency.Id;
        }

        public async Task<int> Update(AgencyDto agencyDto)
        {
            var province = await _provinceRepository.FirstOrDefaultAsync(x => x.Id == agencyDto.ProvinceId);
            if (province == null || agencyDto.ProvinceId <= 0)
            {
                throw new UserFriendlyException("استان وجود ندارد.");
            }
            var agency = ObjectMapper.Map<AgencyDto, Agency>(agencyDto);
            await _agencyRepository.UpdateAsync(agency, autoSave: true);
            return agency.Id;
        }
    }
}
