using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain.Bases;

public class OrderStatusTypeReadOnly : Entity<int>
{
    public OrderStatusTypeReadOnly(string orderStatusTitle, int orderStatusCode, string orderStatusTitleEn,int id)
    {
        Id = id;
        OrderStatusTitle = orderStatusTitle;
        OrderStatusCode = orderStatusCode;
        OrderStatusTitleEn = orderStatusTitleEn;
    }

    public string OrderStatusTitle { get; set; }

    public int OrderStatusCode { get; set; }

    public string OrderStatusTitleEn { get; set; }
}