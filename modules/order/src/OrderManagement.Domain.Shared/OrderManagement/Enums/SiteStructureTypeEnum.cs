using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared
{
    public enum SiteStructureTypeEnum
    {
        [EnumProperty(Description = "اسلایدر")]
        Slider = 1,
        [EnumProperty(Description = "محصولات")]
        ProductCarousel = 2,
        [EnumProperty(Description = "کلاس خودرو")]
        CarClassCarousel = 3,
        [EnumProperty(Description = "نوع طرح")]
        EsaleType = 4,
        [EnumProperty(Description = "بانک")]
        Bank = 5,
    }
}
