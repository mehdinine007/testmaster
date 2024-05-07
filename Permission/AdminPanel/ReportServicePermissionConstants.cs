using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.AdminPanel
{
    public class ReportServicePermissionConstants
    {
        public const string ReportQuestionnaire = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string ReportQuestionnaire_DisplayName = "گزارش پرسشنامه";
        public const string GetAllDashboard = "000500020002";
        public const string GetAllDashboard_DisplayName = "نمایش داشبوردها";
        public const string GetWidgetByDashboardId = "000500030003";
        public const string GetWidgetByDashboardId_DisplayName = "نمایش ویجت ها";
        public const string GetChart = "000500040004";
        public const string GetChart_DisplayName = "نمایش ویجت ها";
        public const string GetGrid = "000500050005";
        public const string GetGrid_DisplayName = "نمایش گرید";

        public const string ServiceIdentifier = "0001";
        public const string ServiceDisplayName = "سرویس گزارشات";
    }
}
