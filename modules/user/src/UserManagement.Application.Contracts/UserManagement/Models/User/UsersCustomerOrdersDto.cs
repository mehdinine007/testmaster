using Abp.Authorization.Users;
using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Contracts.Models;

public class UsersCustomerOrdersDto
{

    [Required]
    [Column(TypeName = "NCHAR(10)")]
    public string NationalCode { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(150)")]
    public string FatherName { get; set; }
    [Required]

    public DateTime BirthDate { get; set; }
    [Required]
    public string Gender { get; set; }
    [Required]
    [Column(TypeName = "NCHAR(10)")]
    public string PostalCode { get; set; }
    [Required]
    [Column(TypeName = "NCHAR(11)")]
    public string Mobile { get; set; }

    [Required]
    [Column(TypeName = "NCHAR(11)")]
    public string Tel { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(255)")]
    public string Address { get; set; }
    [Column(TypeName = "NVARCHAR(50)")]
    [Required]
    public string AccountNumber { get; set; }
    [Required]
    [Column(TypeName = "VARCHAR(26)")]
    public string Shaba { get; set; }
    [Column(TypeName = "VARCHAR(6)")]
    public string PreTel { get; set; }

    [Column(TypeName = "NVARCHAR(100)")]
    public string Street { get; set; }

    [Column(TypeName = "VARCHAR(20)")]
    public string Pelaq { get; set; }
    [Column(TypeName = "NVARCHAR(100)")]
    public string Alley { get; set; }

    public int? Priority { get; set; }
    [Required]
    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string UserName { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }

    public string FullName { get; set; }
    [JsonProperty(PropertyName = "شهرمحل تولد")]
    public string BirthCityName { get; set; }
    [JsonProperty(PropertyName = "شهرمحل سکونت")]
    public string HabitationCityName { get; set; }
    [JsonProperty(PropertyName = "شهرصدور شناسنامه")]
    public string IssuingCityName { get; set; }
    [JsonProperty(PropertyName = "استان محل تولد")]
    public string BirthProvinceName { get; set; }
    [JsonProperty(PropertyName = "استان محل سکونت")]
    public string HabitationProvinceName { get; set; }
    [JsonProperty(PropertyName = "استان صدور شناسنامه")]
    public string IssuingProvinceName { get; set; }


}
