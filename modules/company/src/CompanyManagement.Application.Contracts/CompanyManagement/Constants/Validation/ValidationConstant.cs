using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation
{
    public class ValidationConstant
    {
        public const string VehicleIsRequired = "وسیله نقلیه اجباری است";
        public const string VehicleIsRequiredId = "0001";
        public const string NationalCodeIsRequired = "کد ملی اجباری است";
        public const string NationalCodeIsRequiredId = "0002";
        public const string VinIsRequired = "vin اجباری است";
        public const string VinIsRequiredId = "0003";
        public const string ChassiIsRequired = "شماره شاسی اجباری است";
        public const string ChassiIsRequiredId = "0004";
        public const string EngineIsRequired = "شماره موتور اجباری است";
        public const string EngineIsRequiredId = "0005";
        public const string ItemNotFound = "مورد یافت نشد";
        public const string ItemNotFoundId = "0006";

    }
    public static class RuleSets
    {
        public const string Add = "AddList";
        public const string AddId = "0001";
    }
}
