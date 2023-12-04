using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain;

public class Priority : Entity<int>
{
    public int Radif { get; set; }
    public string OrderID { get; set; }
    public string SortID { get; set; }
    public string NationalCode { get; set; }
    public string EkanNumber { get; set; }
    public string SumNumberOfNationalCode { get; set; }
    public string PriorityNum { get; set; }
    public int PriorityLevel { get; set; }
}
