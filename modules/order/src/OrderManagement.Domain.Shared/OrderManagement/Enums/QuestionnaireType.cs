﻿using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Shared;

public enum QuestionnaireType
{
    [Display(Name = "احراض اجباری")]
    AuthorizedOnly = 0,

    [Display(Name = "احراض اختیاری")]
    AnonymousAllowed = 1

}
