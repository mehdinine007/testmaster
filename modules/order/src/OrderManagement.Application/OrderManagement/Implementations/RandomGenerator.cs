using OrderManagement.Application.Contracts.Services;
using System;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class RandomGenerator : ApplicationService, IRandomGenerator
{
    public int GetUniqueInt()
        => Math.Abs(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
}
