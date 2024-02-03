using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Domain.Shared
{
    public enum AttachmentEntityTypeEnum
    {
        [EnumProperty(Description = "نامشخص")]
        None = 0,
        [EnumProperty(Description = "گالری")]
        Gallery = 1,
        [EnumProperty(Description = "بنر")]
        Banner = 2,
        [EnumProperty(Description = "لوگو")]
        Logo = 3,
        [EnumProperty(Description = "لوگو در صفحه")]
        LogoInPage = 4,
        [EnumProperty(Description = "بخشنامه")]
        Circular = 5,
        [EnumProperty(Description = "آیکون")]
        Icon = 6,
        [EnumProperty(Description = "اطلاعیه")]
        Announcement = 7,
       [EnumProperty(Description = "تبلیغات")]
        AdvertisementDetail = 8
    }
}
