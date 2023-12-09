using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class UserDto
    {
        public string NationalCode { get; set; }

        public int? BankId { get; set; }

        public string AccountNumber { get; set; }

        public string Shaba { get; set; }

        public int? BirthCityId { get; set; }

        public int? IssuingCityId { get; set; }

        public int? HabitationCityId { get; set; }

        public int? BirthProvinceId { get; set; }

        public int? IssuingProvinceId { get; set; }

        public int? HabitationProvinceId { get; set; }

        public string MobileNumber { get; set; }

        public int? CompanyId { get; set; }

        public string SurName { get; set; }

        public string Name { get; set; }
        public string Uid { get; set; }
    }
}
