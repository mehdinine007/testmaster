namespace UserManagement.Application.Contracts.Models;

public record UpdateUserPhoneNumber(Guid UserId, string NewPhoneNumber, string SmsCode );