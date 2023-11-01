using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report
{
    public class DashboardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
    }
}
