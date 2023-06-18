using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OrderManagement.Domain
{
    //TODO: Create tables for these tables dynamically
    public enum OrderRejectionType
    {

        [Display(Name = "عدم تطابق کدملی و شماره موبایل")]
        PhoneNumberAndNationalCodeConflict = 1,

        [Display(Name = "نداشتن گواهی نامه معتبر")]
        DoesntHadQualifiedDrivingLicense = 2,

        [Display(Name = "دارای پلاک فعال")]
        ActivePlaqueDetected = 3,

        [Display(Name = "ثبت سفارش در سامانه خودروهای وارداتی")]
        OrderRegisteredInInternalVehicleSite = 4,

        [Display(Name = "لیست خرید خودروساز (سایپا)")]
        SaipaVehicleManufactureList = 5,

        [Display(Name = "لیست خرید خودروساز (ایران خودرو)")]
        IkcoVehicleManufactureList = 6,

        [Display(Name = "لیست خرید خودروساز (کرمان موتور)")]
        KermanMotorVehicleManufactureList = 7,

        [Display(Name = "لیست خرید خودروساز (صنایع خودرو سازی ایلیا)")]
        IliaAutoVehicleManufactureList = 8,

        [Display(Name = "لیست خرید خودروساز (فردا موتورز)")]
        FardaMotorsVehicleManufactureList = 9,

        [Display(Name = "لیست خرید خودروساز (آرین پارس)")]
        ArianParsVehicleManufactureList = 10,

        [Display(Name = "لیست خرید خودروساز (مکث موتور)")]
        MaxMotorVehicleManufactureList = 11,

        [Display(Name = "لیست خرید خودروساز (بهمن موتور)")]
        BahmanMotorVehicleManufactureList = 12,

        [Display(Name = "لیست خرید خودروساز (مدیران خودرو)")]
        MvmVehicleManufactureList = 13,
        [Display(Name = "عدم احراز در طرح جوانی توسط ثبت احول")]
        YoungPlan = 14,
        [Display(Name = "عدم احراز خودرو فرسوده")]
        OldPlan = 15
    }
}
