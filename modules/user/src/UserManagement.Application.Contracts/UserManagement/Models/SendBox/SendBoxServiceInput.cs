using UserManagement.Domain.Shared.SendBox;

namespace UserManagement.Application.Contracts.Models.SendBox;

public class SendBoxServiceInput
{
    public string Recipient { get; set; }
    public string Text { get; set; }
    public ProviderSmsTypeEnum Provider { get; set; }
    public TypeMessageEnum Type { get; set; }
}