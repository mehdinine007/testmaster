using OrderManagement.Domain.OrderManagement;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class Announcement : FullAuditedEntity<int>
{
    public DateTime Date { get; set; }

    public string Title { get; set; }

    public string Notice { get; set; }

    public string Description { get; set; }

    public string Content { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int? CompanyId { get; set; }

    public bool Active { get; set; }

    public virtual ProductAndCategory Company { get; set; }
}
