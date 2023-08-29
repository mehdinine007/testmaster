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
    }
}
