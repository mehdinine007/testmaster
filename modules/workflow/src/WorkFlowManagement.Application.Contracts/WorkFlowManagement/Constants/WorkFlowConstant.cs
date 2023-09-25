using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants
{
    public class WorkFlowConstant
    {

        public static string OrganizationChartNotFound = "پست سازمان وجود ندارد";
        public static string OrganizationChartNotFoundId = "1002";
        public static string OrganizationChartDuplicate = "پست  سازمان تکراری میباشد";
        public static string OrganizationChartDuplicateId = "1003";
        public static string OrganizationChartParentNotFound = "چارت سازمان سطح بالایی وجود ندارد";
        public static string OrganizationChartParentNotFoundId= "1004";

        public static string OrganizationPositionNotFound = "پست سازمانی وجود ندارد";
        public static string OrganizationPositionNotFoundId = "1005";

        public static string OrganizationPositionDuplicateNotFound = "پست سازمانی برای کاربردیگری تعریف شده است";
        public static string OrganizationPositionDuplicateNotFoundId = "1006";

        public static string InvalidEndDate = "تاریخ پایان نباید از تاریخ شروع کوچکتر باشد";
        public static string InvalidEndDateId = "1007";


        public static string RoleNotFound = "نقش وجود ندارد";
        public static string RoleNotFoundId = "1008";

        public static string RoleOrganizationChartNotFound = "نقش چارت وجود ندارد";
        public static string RoleOrganizationChartNotFoundId = "1009";

        public static string SchemeNotFound = "طرح وجود ندارد";
        public static string SchemeNotFoundId = "2001";


        public static string ActivityNotFound = "فعالیت وجود ندارد";
        public static string ActivityNotFoundId = "2002";


        public static string TransitionNotFound = "انتفال وجود ندارد";
        public static string TransitionNotFoundId = "2003";


        public static string ActivitySourceNotFound = "مبدا فعالیت وجود ندارد";
        public static string ActivitySourceNotFoundId = "2004";

        public static string ActivityTargetNotFound = " مقصد فعالیت وجود ندارد";
        public static string ActivityTargetNotFoundId = "2005";


        public static string ActivityRoleNotFound = " نقش فعالیت وجود ندارد";
        public static string ActivityRoleNotFoundId = "2006";

        public static string ProcessNotFound = " فرآیند وجود ندارد";
        public static string ProcessNotFoundId = "2007";

        public static string PreviousActivityNotFound = "فعالیت قبلی وجود ندارد";
        public static string PreviousActivityNotFoundId = "2008";

        public static string CreatedOrganizationChartNotFound = "سمت درخواست کننده وجود ندارد";
        public static string CreatedOrganizationChartNotFoundId = "2009";

        public static string PreviousOrganizationChartNotFound = "سازمان قبلی وجود ندارد";
        public static string PreviousOrganizationChartNotFoundId = "3001";


        public static string InboxNotFound = "صندوق ورودی وجود ندارد";
        public static string InboxNotFoundId = "3002";

        public static string InboxParentNotFound = "صندوق ورودی سطح بالایی وجود ندارد";
        public static string InboxParentNotFoundId = "3003";

        public static string DependentOrganizationChartNotFound = "این سازمان درحال استفاده میباشد";
        public static string DependentOrganizationChartNotFoundId = "3004";
        public static string UserNotFound = "کاربروجود ندارد";
        public static string UserNotFoundFoundId = "3005";

        public static string PersonNotFound = "پرسنل وجود ندارد";
        public static string PersonNotFoundId = "3006";

    }
}
