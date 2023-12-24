using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;

namespace CompanyManagement.Domain.CompanyManagement;

public class User : AbpUser<User>
{
    public const string DefaultPassword = "123qwe";
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
    [Column(TypeName = "NVARCHAR(10)")]
    public string PostalCode { get; set; }
    [Required]
    [Column(TypeName = "VARCHAR(11)")]
    public string Mobile { get; set; }
    [Column(TypeName = "VARCHAR(11)")]
    [Required]
    public string Tel { get; set; }
    [Column(TypeName = "NVARCHAR(255)")]
    [Required]
    public string Address { get; set; }
    public int? BankId { get; set; }
    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string AccountNumber { get; set; }
    [Required]
    [Column(TypeName = "VARCHAR(26)")]
    public string Shaba { get; set; }

    //public Province Province { get; set; }

    [Column(TypeName = "VARCHAR(6)")]
    public string PreTel { get; set; }
    [ForeignKey("City")]
    public int? BirthCityId { get; set; }
    [ForeignKey("City")]
    public int? IssuingCityId { get; set; }
    [ForeignKey("City")]
    public int? HabitationCityId { get; set; }
    [ForeignKey("Province")]
    public int? BirthProvinceId { get; set; }
    [ForeignKey("Province")]
    public int? IssuingProvinceId { get; set; }
    [ForeignKey("Province")]
    public int? HabitationProvinceId { get; set; }

    public DateTime? IssuingDate { get; set; }
    public Int16? RegionId { get; set; }
    [Column(TypeName = "NVARCHAR(100)")]
    public string Street { get; set; }
    [Column(TypeName = "VARCHAR(20)")]
    public string Pelaq { get; set; }
    [Column(TypeName = "NVARCHAR(100)")]
    public string Alley { get; set; }
    public int? Priority { get; set; }


    [ForeignKey("Company")]
    public int? CompanyId { get; set; }

    public Guid UID { get; set; }
    [NotMapped]
    public String TempUID { get; set; }


    public string ChassiNo { get; set; }
    public string Vin { get; set; }
    public string EngineNo { get; set; }
    public string Vehicle { get; set; }
    [NotMapped]
    public List<string> RolesM { get; set; }
    public static string CreateRandomPassword()
    {
        return Guid.NewGuid().ToString("N").Truncate(16);
    }

    public static User CreateTenantAdminUser(int tenantId, string emailAddress)
    {
        var user = new User
        {
            TenantId = tenantId,
            UserName = AdminUserName,
            Name = AdminUserName,
            Surname = AdminUserName,
            EmailAddress = emailAddress,
            Roles = new List<UserRole>()
        };

        user.SetNormalizedNames();

        return user;
    }
}
