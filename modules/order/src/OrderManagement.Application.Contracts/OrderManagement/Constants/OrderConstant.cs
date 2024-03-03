using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts
{
    public static class OrderConstant
    {
        public static string NoCapacityCreateTicket = "در حال حاضر كل ظرفيت اين برنامه فروش، در حال خريد مي باشد، در صورت آزاد شدن ظرفيت و انصراف مشتريان در صف خريد،  مي توانيد مجددا براي سفارش خودرو تلاش فرماييد";
        public static string NoCapacityCreateTicketId = "1002";

        public static string AgancyNoCapacityCreateTicket = "در حال حاضر كل ظرفيت اين برنامه فروش در نمایندگی، در حال خريد مي باشد، در صورت آزاد شدن ظرفيت و انصراف مشتريان در صف خريد،  مي توانيد مجددا براي سفارش خودرو تلاش فرماييد";
        public static string AgancyNoCapacityCreateTicketId = "1003";

        public static string NoCapacitySaleDetail = "اتمام ظرفیت برنامه فروش!";
        public static string NoCapacitySaleDetailId = "1004";

        public static string NoValidFlowOrderStep = "عدم رعایت ترتیب مراحل سفارش";
        public static string NoValidFlowOrderStepId = "1005";

        public static List<OrderStepEnum> OrderStepWithoutPayment = new List<OrderStepEnum>
        {
            OrderStepEnum.Start,
            OrderStepEnum.PreviewOrder,
            OrderStepEnum.SaveOrder
        };

        public static List<OrderStepEnum> OrderStepWithPayment = new List<OrderStepEnum> 
        { 
            OrderStepEnum.Start, 
            OrderStepEnum.SubmitOrder, 
            OrderStepEnum.PreviewOrder,
            OrderStepEnum.SaveOrder 
        };

        public static string AttachmentNotFound = "ضمیمه وجود ندارد";
        public static string AttachmentNotFoundId = "2001";
        public static string FileUploadNotFound = "فایل نامعتبر";
        public static string FileUploadNotFoundId = "2002";
        public static string FileUploadNotExtention = "پسوند نامعتبر فایل";
        public static string FileUploadNotExtentionId = "2003";
        public static string FileUploadNotPathNotExists = "مسیر آپلود فایل وجود ندارد";
        public static string FileUploadNotPathNotExistsId = "2004";
        public static string FileUploadCopyError = "خطا در انتقال فایل به سرور";
        public static string FileUploadCopyErrorId = "2004";
        public static string AttachmentEntityIdNotFound = "شناسه فایل نامعتبر می باشد";
        public static string AttachmentEntityIdNotFoundId = "2005";
        public static string AttachmentPriorityIsEmpty = "ترتیب نمایش خالی است";
        public static string AttachmentPriorityIsEmptyId = "2006";
        public static string AttachmentPriorityDuplicate = "ترتیب نمایش تکراری است";
        public static string AttachmentPriorityDuplicateId = "2007";
        public static string AttachmentDuplicate = "ضمیمه وارد شده قبلا ایجاد شده";
        public static string AttachmentDuplicateId = "2008";

        public static string SiteStuctureNotFound = "ساختار سایت وجود ندارد";
        public static string SiteStuctureNotFoundId = "3001";

        public static string LastProductLevel = "سطح آخر دسنه بسندی میباشد";
        public static string LastProductLevelId = "4001";

        public static string DuplicatePriority = "شماره اولویت بندی تکراری میباشد";
        public static string DuplicatePriorityId = "4002";

        public static string InCorrectPriorityNumber = "شماره اولویت بندی باید بزرگترازصفرباشد";
        public static string InCorrectPriorityNumberId = "4003";

        public static string ProductLevelNotFound = "سطح بندی محصول وجود ندارد";
        public static string ProductLevelNotFoundId = "4004";

        public static string CarClassNotFound = "کلاس  خودرو وجود ندارد";
        public static string CarClassNotFoundId = "4005";
        public static string BankNotFound = "بانک وجود ندارد";
        public static string BankNotFoundId = "4006";

        public static string ChartStructureNotFound = "آمار وجود ندارد";
        public static string ChartStructureNotFoundId = "4007";

        public static string AnnouncementNotFound = "آگهی وجود ندارد";
        public static string AnnouncementNotFoundId = "4008";


        public static string ProductAndCategoryNotFound = "دسته بندی یا محصول وجود ندارد";
        public static string ProductAndCategoryFoundId = "4009";

        public static string SaleSchemaNotFound = "برنامه فروش وجود ندارد";
        public static string SaleSchemaFoundId = "5001";

        public static string NotValid = "ورودی نامعتبر می باشد";
        public static string NotValidId = "5002";
        public const string OrganizationUrlFormat = "{0}?enc={1}";
        public const string OrganizationEncryptedExpression = "nationalCode={0}|saleId={1}";

        public static string AgencyNotFound = "نمایندگی تعرف نشده است.";
        public static string AgencyId = "5003";

        public static string PspAccountNotFound = "درگاه بانکی انتخاب نشده است.";
        public static string PspAccountId = "5004";

        public static string ToDateLessThanFromDate = "تاریخ پایان کوچک تر از تاریخ شروع می باشد";
        public static string ToDateLessThanFromDateId = "5005";

        public static string AdvertisementNotFound = "جایگاه وجود ندارد";
        public static string AdvertisementNotFoundId = "5006";
        public static string FirstPriority = "اولین اولویت امکان تغییر ندارد";
        public static string FirstPriorityId = "5007";
        public static string LastPriority = "اخرین اولویت امکان تغییر ندارد";
        public static string LastPriorityId = "5008";
        public static string AdvertisementDelete = "برای حذف جایگاه ابتدا وابستگی های آن را حذف کنید";
        public static string AdvertisementDeleteId = "5009";
        public static string AdvertisementDetailNotFound = "جزییات جایگاه وجود ندارد";
        public static string AdvertisementDetailNotFoundId = "6001";
        public static string OrganizationNotFound = "سازمان وجود ندارد";
        public static string OrganizationNotFoundId = "6002";

        public static string UserDataAccessProductNotFound = "کاربر گرامی شما مجاز به ثبت سفارش برای این برنامه فروش نمی باشید";
        public static string UserDataAccessProductNotFoundId = "9005";
        public static string UserDataAccessOldCarNotFound = "خودروفرسوده موجود نمیباشد";
        public static string UserDataAccessOldCarNotFoundId = "9006";
        public const string ProductNotFound = "محصول وجود ندارد";
        public const string ProductNotFoundId = "9008";
        public const string OrderWinnerFound = "جهت ثبت سفارش جدید لطفا ابتدا از جزئیات سفارش، سفارش قبلی خود که احراز شده اید یا در حال بررسی می باشد را لغو نمایید";
        public const string OrderWinnerFoundId = "9009";
    }
}
