
using UserManagement.Domain.UserManagement.Bases;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IBaseInformationService
    {
        Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode = "");
    }
}
