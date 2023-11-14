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

        public override string ModuleIdentifier => "0005";

        public override string ServiceIdentifier => "0001";

        public override string ServiceDisplayName => "سرویس گزارشات";
    }
}
