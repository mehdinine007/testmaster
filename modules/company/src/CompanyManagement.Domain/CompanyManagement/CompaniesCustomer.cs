namespace CompanyManagement.Domain.CompanyManagement;

public class CompaniesCustomer
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProvienceName { get; set; }
    public string DeliveryDateDescription { get; set; }
    public int? OrderRejectionStatus { get; set; }
    public string CompanyName { get; set; }
    public int ESaleTypeId { get; set; }
    public string TrackingCode { get; set; }
    public string CompanySaleId { get; set; }
}
