﻿using System;
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

    }
}
