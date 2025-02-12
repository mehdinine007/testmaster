﻿namespace UserManagement.Application.Contracts.Models;

public enum SMSType
{
    Register = 1,
    ForgetPassword = 2,
    UpdateProfile = 3,
    Login = 4,
    UserRejectionAdvocacy = 5,
    ChangePassword = 6,
    AnonymousQuestionnaireSubmitation = 7,
    UpdatePhoneNumber = 8
}