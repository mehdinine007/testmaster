using System;
using Volo.Abp.Domain.Entities;

public class CompaniesCustomer : Entity<int>
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string NationalCode { get; set; }
    public string Mobile { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FatherName { get; set; }
    public string BirthCertId { get; set; }
    public DateTime BirthDate { get; set; }
    public byte Gender { get; set; }
    public int ProvienceId { get; set; }
    public string ProvienceName { get; set; }
    public string Tel { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public DateTime IssuingDate { get; set; }
    public string Shaba { get; set; }
    public string DeliveryDateDescription { get; set; }
    public int OrderRejectionStatus { get; set; }
    public string CompanyName { get; set; }
    public int ESaleTypeId { get; set; }
}
