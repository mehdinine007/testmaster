using OrderManagement.Application.Contracts.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts
{
    public interface IUserDataAccessService : IApplicationService
    {
        Task<List<OldCarDto>> OldCarGetList(string Nationalcode);
        Task<List<UserDataAccessDto>> CheckOldCar(string nationalcode,string engineNo,string vin,string vehicle,string chassiNo);

    }
}
