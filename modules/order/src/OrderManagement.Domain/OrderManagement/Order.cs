using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement
{
    public class Order : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// A unique value for this Order.
        /// OrderManager ensures the uniqueness of it.
        /// It can not be changed after creation of the Order.
        /// </summary>
        [NotNull]
        public string Code { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        public float Price { get; private set; }

        public int StockCount { get; private set; }

        public string ImageName { get; private set; }

        private Order()
        {
            //Default constructor is needed for ORMs.
        }

        internal Order(
            Guid id,
            [NotNull] string code, 
            [NotNull] string name, 
            float price = 0.0f, 
            int stockCount = 0,
            string imageName = null)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));

            if (code.Length >= OrderConsts.MaxCodeLength)
            {
                throw new ArgumentException($"Order code can not be longer than {OrderConsts.MaxCodeLength}");
            }

            Id = id;
            Code = code;
            SetName(Check.NotNullOrWhiteSpace(name, nameof(name)));
            SetPrice(price);
            SetImageName(imageName);
            SetStockCountInternal(stockCount, triggerEvent: false);
        }

        public Order SetName([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            if (name.Length >= OrderConsts.MaxNameLength)
            {
                throw new ArgumentException($"Order name can not be longer than {OrderConsts.MaxNameLength}");
            }

            Name = name;
            return this;
        }

        public Order SetImageName([CanBeNull] string imageName)
        {
            if (imageName == null)
            {
                return this;
            }

            if (imageName.Length >= OrderConsts.MaxImageNameLength)
            {
                throw new ArgumentException($"Order image name can not be longer than {OrderConsts.MaxImageNameLength}");
            }

            ImageName = imageName;
            return this;
        }

        public Order SetPrice(float price)
        {
            if (price < 0.0f)
            {
                throw new ArgumentException($"{nameof(price)} can not be less than 0.0!");
            }

            Price = price;
            return this;
        }

        public Order SetStockCount(int stockCount)
        {
            return SetStockCountInternal(stockCount);
        }

        private Order SetStockCountInternal(int stockCount, bool triggerEvent = true)
        {
            if (StockCount < 0)
            {
                throw new ArgumentException($"{nameof(stockCount)} can not be less than 0!");
            }

            if (StockCount == stockCount)
            {
                return this;
            }

            if (triggerEvent)
            {
                AddDistributedEvent(
                    new OrderStockCountChangedEto(
                        Id,
                        StockCount,
                        stockCount
                    )
                );
            }

            StockCount = stockCount;
            return this;
        }
    }
}
