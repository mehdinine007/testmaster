using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos
{
    public  class ReportQueryDto
    {
        public ReportQuestionnaireTypeEnum Type { get; set; }
        public string? NationalCode { get; set; }
        public int? SkipCount { get; set; }
        public int? MaxResultCount { get; set; }
        
    }
}
