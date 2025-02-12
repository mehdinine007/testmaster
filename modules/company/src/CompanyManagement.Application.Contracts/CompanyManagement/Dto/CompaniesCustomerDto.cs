﻿namespace CompanyManagement.Application.Contracts.CompanyManagement;

public class CompaniesCustomerDto
{
    public int Id { get; set; }
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
    public int Gender { get; set; }
    public int? ProvienceId { get; set; }
    public string ProvienceName { get; set; }
    public string Tel { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public DateTime? IssuingDate { get; set; }
    public string Shaba { get; set; }
    public string DeliveryDateDescription { get; set; }
    public int? OrderRejectionStatus { get; set; }
    public string CompanyName { get; set; }
    public int ESaleTypeId { get; set; }
    public string TrackingCode { get; set; }
    public string CompanySaleId { get; set; }
}
