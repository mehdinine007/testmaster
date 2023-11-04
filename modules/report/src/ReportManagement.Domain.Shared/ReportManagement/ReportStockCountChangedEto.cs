using Volo.Abp.Domain.Entities.Events.Distributed;

namespace ReportManagement.Domain.Shared
{
    [Serializable]
    public class ReportStockCountChangedEto : EtoBase
    {
        public Guid Id { get; }

        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private ReportStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public ReportStockCountChangedEto(Guid id, int oldCount, int currentCount)
        {
            Id = id;
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}