using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Domain.UserManagement.Enums;

namespace UserManagement.Application.Contracts;

public class UserDto
{
    [Required]

    public string UserName { get; set; }

    [Required]

    public string Name { get; set; }

    [Required]

    public string Surname { get; set; }

    public string FullName { get; set; }

    [Required]
    [Column(TypeName = "NCHAR(10)")]
    public string NationalCode { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(150)")]
    public string FatherName { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(11)")]
    public string BirthCertId { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    [Column(TypeName = "TINYINT")]
    public Gender Gender { get; set; }
    //public int ProvinceId { get; set; }
    //public int CityId { get; set; }
    [Required]
    [Column(TypeName = "NCHAR(10)")]
    public string PostalCode { get; set; }
    [Required]
    [Column(TypeName = "NCHAR(11)")]
    public string Mobile { get; set; }
    [Required]
    [Column(TypeName = "NCHAR(11)")]
    public string Tel { get; set; }
    [Column(TypeName = "NVARCHAR(255)")]
    public string Address { get; set; }
    public string PreTel { get; set; }
    public int? BirthCityId { get; set; }
    public int? IssuingCityId { get; set; }
    public int? HabitationCityId { get; set; }
    public Int16? RegionId { get; set; }
    [Column(TypeName = "NVARCHAR(100)")]
    public string Street { get; set; }
    [Column(TypeName = "VARCHAR(20)")]
    public string Pelaq { get; set; }
    public DateTime? IssuingDate { get; set; }
    [Column(TypeName = "NVARCHAR(100)")]
    public string Alley { get; set; }
    public int BankId { get; set; }
    public string AccountNumber { get; set; }
    public string Shaba { get; set; }
    public string Message { get; set; }
    public string smsCode { get; set; }
    public string ck { get; set; }
    public int? Priority { get; set; }
    public int? BirthProvinceId { get; set; }
    public int? IssuingProvinceId { get; set; }
    public int? HabitationProvinceId { get; set; }
    public string BirthCityName { get; set; }
    public string HabitationCityName { get; set; }
    public string IssuingCityName { get; set; }
    public string BirthProvinceName { get; set; }
    public string HabitationProvinceName { get; set; }
    public string IssuingProvinceName { get; set; }
    public Guid UID { get; set; }
    public string ChassiNo { get; set; }
    public string Vin { get; set; }
    public string EngineNo { get; set; }
    public string Vehicle { get; set; }

}
