using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace OrderManagement.Domain.Shared
{
    [Serializable]
    public class OrderStockCountChangedEto : EtoBase
    {
        public Guid Id { get; }

        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private OrderStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public OrderStockCountChangedEto(Guid id, int oldCount, int currentCount)
        {
            Id = id;
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}