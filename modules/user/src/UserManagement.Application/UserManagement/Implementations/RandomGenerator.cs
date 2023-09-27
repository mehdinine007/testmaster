using Volo.Abp.Application.Services;
using UserManagement.Application.Contracts.UserManagement.Services;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class RandomGenerator : ApplicationService,IRandomGenerator
    {
        public int GetUniqueInt()
        {
            byte[] value = Guid.NewGuid().ToByteArray();
            return Math.Abs(BitConverter.ToInt32(value, 0));
        }
    }
}
