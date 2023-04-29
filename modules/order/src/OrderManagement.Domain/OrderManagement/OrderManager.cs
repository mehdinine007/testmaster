using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OrderManagement
{
    public class OrderManager : DomainService
    {
        private readonly IRepository<Order, Guid> _OrderRepository;

        public OrderManager(IRepository<Order, Guid> OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<Order> CreateAsync(
            [NotNull] string code,
            [NotNull] string name,
            float price = 0.0f,
            int stockCount = 0,
            string imageName = null)
        {
            var existingOrder = await _OrderRepository.FirstOrDefaultAsync(p => p.Code == code);
            if (existingOrder != null)
            {
                throw new OrderCodeAlreadyExistsException(code);
            }

            return await _OrderRepository.InsertAsync(
                new Order(
                    GuidGenerator.Create(),
                    code,
                    name,
                    price,
                    stockCount,
                    imageName
                )
            );
        }
    }
}
