using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class UnAuthorizedUser : FullAuditedEntity<long>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string NationalCode { get; set; }

    public string MobileNumber { get; set; }

    public string VehicleName { get; set; }

    public string ManufactureDate { get; set; }

    public string Vin { get; set; }

    public string SmsCode { get; set; }

    public Questionnaire Questionnaire { get; set; }

    public int QuestionnaireId { get; set; }
}