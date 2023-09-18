using Esale.Core.Utility.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.SendBox.Dtos;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface ISendBoxAppService
    {
        Task<IResult> SendSms(SendSMSDto input);
    }
}
