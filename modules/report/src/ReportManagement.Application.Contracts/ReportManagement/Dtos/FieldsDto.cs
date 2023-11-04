using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class FieldsDto
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
