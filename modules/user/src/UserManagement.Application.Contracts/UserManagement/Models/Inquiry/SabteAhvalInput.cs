namespace UserManagement.Application.Contracts.Models;

public class SabteAhvalInput
{
    public long Nationalcode { get; set; }
    public int BirthDate { get; set; }
    public SabteAhvalInput(long nc, int bd)
    {
        Nationalcode = nc;
        BirthDate = bd;
    }
}