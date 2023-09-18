namespace UserManagement.Application.Contracts.Models;

public class RegistrationValidationDto
{
    public string Nationalcode { get; set; }
    public string CT { get; set; }
    public string CIT { get; set; }
    public string CK { get; set; }
    public RegistrationValidationDto(string nc)
    {
        this.Nationalcode = nc;
    }

}