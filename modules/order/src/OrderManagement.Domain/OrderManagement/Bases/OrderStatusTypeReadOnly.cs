using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain.Bases;

public class OrderStatusTypeReadOnly : Entity<int>
{
    public string OrderStatusTitle { get; set; }

    public int OrderStatusCode { get; set; }

    public string OrderStatusTitleEn { get; set; }
}