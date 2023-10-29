using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Shared;

public enum GenderType
{
    [Display(Name ="مرد")]
    Male = 0,

    [Display(Name = "زن")]
    Female = 1
}