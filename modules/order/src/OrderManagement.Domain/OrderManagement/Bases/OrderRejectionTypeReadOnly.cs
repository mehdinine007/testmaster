using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain.Bases;

public class OrderRejectionTypeReadOnly : Entity<int>
{
    public int OrderRejectionCode { get; set; }

    public string OrderRejectionTitleEn { get; set; }

    public string OrderRejectionTitle { get; set; }
}
