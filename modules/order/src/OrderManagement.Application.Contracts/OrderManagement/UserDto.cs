namespace OrderManagement.Application.Contracts
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
        public int GenderCode { get; set; }

        public int? CompanyId { get; set; }

        public string SurName { get; set; }

        public string Name { get; set; }

        public int? Priority { get; set; }

        public string BirthCertId { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCityTitle { get; set; }
        public string IssuingCityTitle { get; set; }
        public string Tel { get; set; }
        public string PostalCode { get; set; }
    }
}