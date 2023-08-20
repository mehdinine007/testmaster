using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Shared
{
    public enum QuestionType
    {
        [Display(Name = "تشریحی")]
        Descriptional = 1,
        [Display(Name = "گزینه ای")]
        Optional = 2,
        [Display(Name = "بازه")]
        Range = 3,
        [Display(Name = "چند گزینه ای")]
        MultiSelectOptional = 4
    }
}


