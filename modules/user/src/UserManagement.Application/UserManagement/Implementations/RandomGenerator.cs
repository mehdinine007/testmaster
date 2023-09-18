using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.Services;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class RandomGenerator : IRandomGenerator
    {
        public int GetUniqueInt()
        {
            byte[] value = Guid.NewGuid().ToByteArray();
            return Math.Abs(BitConverter.ToInt32(value, 0));
        }
    }
}
