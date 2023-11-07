#nullable disable
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts.Dtos;

public class SubmitAnswerTreeDto
{
    public int QuestionnaireId { get; set; }

    public List<SubmitAnswerDto> SubmitAnswerDto { get; set; }

    public long? RelatedEntity { get; set; }

    public UnregisteredUserInformation UnregisteredUserInformation { get; set; }
}

public class UnregisteredUserInformation
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string NationalCode { get; set; }

    public string MobileNumber { get; set; }

    public string VehicleName { get; set; }

    public string ManufactureDate { get; set; }

    public string Vin { get; set; }

    public string SmsCode { get; set; }

    public int QuestionnaireId { get; set; }

    public DateTime Age { get; set; }

    public GenderType Gender { get; set; }

    public string EducationLevel { get; set; }

    public string Occupation { get; set; }
}


public class QuestionnaireAnalysisDto
{
    public int QuestionId { get; set; }

    public double Rate { get; set; }

    public string QuestionTitle { get; set; }
}

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }

    public long? QuestionAnswerId { get; set; }

    public string CustomAnswerValue { get; set; }
}
