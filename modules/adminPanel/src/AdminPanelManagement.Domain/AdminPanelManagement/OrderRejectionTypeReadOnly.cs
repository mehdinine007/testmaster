using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace AdminPanelManagement.Domain.AdminPanelManagement
{
    public class OrderRejectionTypeReadOnly: Entity<int>
    {
        public int Code { get; set; }

        public string Title { get; set; }

        public string Title_En { get; set; }
    }
}
