using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum IranSignStateEnum
    {
        DRAFT = 0,
        PENDING_FOR_APPROVAL = 1,
        IN_PROGRESS = 2,
        COMPLETED = 3,
        CANCELED = 4,
    }
}
