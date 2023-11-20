using IFG.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Constants.Permissions
{
    public class ReportServicePermissionConstants : BasePermissionConstants
    {

        public const string ReportQuestionnaire = "000500010001";
        public const string ReportQuestionnaire_DisplayName = "گزارش پرسشنامه";
        public const string GetAllDashboard = "000500020002";
        public const string GetAllDashboard_DisplayName = "نمایش داشبوردها";
        public const string GetWidgetByDashboardId = "000500030003";
        public const string GetWidgetByDashboardId_DisplayName = "نمایش ویجت ها";
        public const string GetChart = "000500040004";
        public const string GetChart_DisplayName = "نمایش ویجت ها";
        public const string GetGrid = "000500050005";
        public const string GetGrid_DisplayName = "نمایش گرید";

        public override string ModuleIdentifier => "0005";

        public override string ServiceIdentifier => "0001";

        public override string ServiceDisplayName => "سرویس گزارشات";
    }
}
