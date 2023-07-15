using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace GatewayManagement.Domain.Shared
{
    [Serializable]
    public class GatewayStockCountChangedEto : EtoBase
    {
        public Guid Id { get; }

        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private GatewayStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public GatewayStockCountChangedEto(Guid id, int oldCount, int currentCount)
        {
            Id = id;
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}