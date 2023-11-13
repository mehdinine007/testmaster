using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db
{
    public class ReportQuestionnaireDb:Entity<long>
    {
        public string QuestionnaireTitle { get; set; }
        public int QuestionnaireId { get; set; }
        public string QuestionTitle { get; set; }
        public string CustomAnswerValue { get; set; }
        public string Description { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EducationLevel { get; set; }
        public GenderTypeEnum Gender { get; set; }
        public string ManufactureDate { get; set; }
        public string VehicleName { get; set; }
        public string Vin { get; set; }
        public DateTime? Age { get; set; }
     
        
            
            
            

    }
}
