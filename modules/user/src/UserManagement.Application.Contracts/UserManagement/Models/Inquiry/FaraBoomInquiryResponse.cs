namespace UserManagement.Application.Contracts.Models;

public class FaraBoomInquiryResponse
{
    public long operation_time { get; set; }

    public string ref_id { get; set; }

    public bool match { get; set; }
}