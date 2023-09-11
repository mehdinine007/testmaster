using System;

namespace UserManagement.Application.Contracts.Models;

public class UserOrderDto
{
    public string NationalCode { get; set; }
    public DateTime BirthDate { get; set; }
    public int SaleDetailId { get; set; }
    public int SaleId { get; set; }
}
