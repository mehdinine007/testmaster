using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace PaymentManagement.Domain.Shared
{
    [Serializable]
    public class PaymentStockCountChangedEto : EtoBase
    {
        public Guid Id { get; }

        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private PaymentStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public PaymentStockCountChangedEto(Guid id, int oldCount, int currentCount)
        {
            Id = id;
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}