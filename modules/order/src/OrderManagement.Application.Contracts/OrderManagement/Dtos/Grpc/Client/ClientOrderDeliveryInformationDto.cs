namespace OrderManagement.Application.Contracts;

public class ClientOrderDeliveryInformationDto
{
    public string NationalCode { get; set; }
    public DateTime? TranDate { get; set; }
    public long? PayedPrice { get; set; }
    public string ContRowId { get; set; }
    public string Vin { get; set; }
    public string BodyNumber { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public long? FinalPrice { get; set; }
    public string CarDesc { get; set; }
    public long OrderId { get; set; }
    public DateTime? ContRowIdDate { get; set; }
}


public class ClientOrderDeliveryInformationRequestDto
{
    public string NationalCode { get; set; }

    public long OrderId { get; set; }
}