using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class CustomerPriority : FullAuditedEntity<long>
{
    public Guid Uid { get; set; }

    public int ApproximatePriority { get; set; }

    public int SaleId { get; set; }

    public int ChosenPriorityByCustomer { get; set; }
}