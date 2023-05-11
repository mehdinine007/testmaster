using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain.Bases;

public class OrderRejectionTypeReadOnly : Entity<int>
{

    public OrderRejectionTypeReadOnly(int orderRejectionCode, string orderRejectionTitleEn, string orderRejectionTitle, int id)
    {
        Id = id;
        OrderRejectionCode = orderRejectionCode;
        OrderRejectionTitleEn = orderRejectionTitleEn;
        OrderRejectionTitle = orderRejectionTitle;
    }

    public int OrderRejectionCode { get; set; }

    public string OrderRejectionTitleEn { get; set; }

    public string OrderRejectionTitle { get; set; }
}
