using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace WorkFlowManagement.Domain.Shared
{
    [Serializable]
    public class WorkFlowStockCountChangedEto : EtoBase
    {
        public Guid Id { get; }

        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private WorkFlowStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public WorkFlowStockCountChangedEto(Guid id, int oldCount, int currentCount)
        {
            Id = id;
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}