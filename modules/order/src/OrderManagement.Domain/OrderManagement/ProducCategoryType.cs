using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.OrderManagement
{
    public enum ProductAndCategoryType
    {
        [Display(Name = "محصول")]
        Product = 1 ,

        [Display(Name = "دسته بندی")]
        Category = 2
    }
}
