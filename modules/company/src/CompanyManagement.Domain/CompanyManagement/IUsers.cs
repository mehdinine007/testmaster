﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;

namespace CompanyManagement.Domain.CompanyManagement;

public interface IUsers
{
    public const int MaxUserNameLength = 256;
    //public DateTime CreationTime { get;set; }
    //public Guid? CreatorId ;
    //public Guid? LastModifierId ;
    //public Guid? DeleterId { get;set; }
    //public DateTime? LastModificationTime ;
    /// <summary>
    /// Maximum length of the <see cref="EmailAddress"/> property.
    /// </summary>
    public const int MaxEmailAddressLength = 256;

    /// <summary>
    /// Maximum length of the <see cref="Name"/> property.
    /// </summary>
    public const int MaxNameLength = 64;

    /// <summary>
    /// Maximum length of the <see cref="Surname"/> property.
    /// </summary>
    public const int MaxSurnameLength = 64;

    /// <summary>
    /// Maximum length of the <see cref="AuthenticationSource"/> property.
    /// </summary>
    public const int MaxAuthenticationSourceLength = 64;

    /// <summary>
    /// UserName of the admin.
    /// admin can not be deleted and UserName of the admin can not be changed.
    /// </summary>
    public const string AdminUserName = "admin";

    /// <summary>
    /// Maximum length of the <see cref="Password"/> property.
    /// </summary>
    public const int MaxPasswordLength = 128;

    /// <summary>
    /// Maximum length of the <see cref="Password"/> without hashed.
    /// </summary>
    public const int MaxPlainPasswordLength = 32;

    /// <summary>
    /// Maximum length of the <see cref="EmailConfirmationCode"/> property.
    /// </summary>
    public const int MaxEmailConfirmationCodeLength = 328;

    /// <summary>
    /// Maximum length of the <see cref="PasswordResetCode"/> property.
    /// </summary>
    public const int MaxPasswordResetCodeLength = 328;

    /// <summary>
    /// Maximum length of the <see cref="PhoneNumber"/> property.
    /// </summary>
    public const int MaxPhoneNumberLength = 32;

    /// <summary>
    /// Maximum length of the <see cref="SecurityStamp"/> property.
    /// </summary>
    public const int MaxSecurityStampLength = 128;
    public const string DefaultPassword = "123qwe";
    [Required]
    [Column(TypeName = "NCHAR(10)")]
    //public long Id ;
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
    //public int ProvinceId ;
    //public int CityId ;
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
    //public Province Province ;

    // boilerplate legacy
    //public  bank Bank ;
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
    public string NormalizedUserName { get; set; }
    public string ConcurrencyStamp { get; set; }
    public int? CompanyId { get; set; }
    // public string UID  { get; set; }
    // public bool IsDeleted { get { get; set; }set { get; set; } }
    public string ChassiNo { get; set; }
    public string Vin { get; set; }
    public string EngineNo { get; set; }
    public string Vehicle { get; set; }
    public List<string> Roles { get; set; }


    /// <summary>
    /// Authorization source name.
    /// It's set to external authentication source name if created by an external source.
    /// Default: null.
    /// </summary>
    [StringLength(MaxAuthenticationSourceLength)]
    public string AuthenticationSource { get; set; }

    /// <summary>
    /// User name.
    /// User name must be unique for it's tenant.
    /// </summary>
    [Required]
    [StringLength(MaxUserNameLength)]
    public string UserName { get; set; }

    /// <summary>
    /// Tenant Id of this user.
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// Email address of the user.
    /// Email address must be unique for it's tenant.
    /// </summary>
    [Required]
    [StringLength(MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Name of the user.
    /// </summary>
    [Required]
    [StringLength(MaxNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Surname of the user.
    /// </summary>
    [Required]
    [StringLength(MaxSurnameLength)]
    public string Surname { get; set; }

    /// <summary>
    /// Return full name (Name Surname )
    /// </summary>
    [NotMapped]
    public string FullName { get { return this.Name + " " + this.Surname; } }

    /// <summary>
    /// Password of the user.
    /// </summary>
    [Required]
    [StringLength(MaxPasswordLength)]
    public string Password { get; set; }

    /// <summary>
    /// Confirmation code for email.
    /// </summary>
    [StringLength(MaxEmailConfirmationCodeLength)]
    public string EmailConfirmationCode { get; set; }

    /// <summary>
    /// Reset code for password.
    /// It's not valid if it's null.
    /// It's for one usage and must be set to null after reset.
    /// </summary>
    [StringLength(MaxPasswordResetCodeLength)]
    public string PasswordResetCode { get; set; }

    /// <summary>
    /// Lockout end date.
    /// </summary>
    public DateTime? LockoutEndDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the access failed count.
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Gets or sets the lockout enabled.
    /// </summary>
    public bool IsLockoutEnabled { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    [StringLength(MaxPhoneNumberLength)]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Is the <see cref="PhoneNumber"/> confirmed.
    /// </summary>
    public bool IsPhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Gets or sets the security stamp.
    /// </summary>
    [StringLength(MaxSecurityStampLength)]
    public string SecurityStamp { get; set; }

    /// <summary>
    /// Is two factor auth enabled.
    /// </summary>
    public bool IsTwoFactorEnabled { get; set; }

    /// <summary>
    /// Login definitions for this user.
    /// </summary>


    /// <summary>
    /// Is the <see cref="AbpUserBase.EmailAddress"/> confirmed.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// Is this user active?
    /// If as user is not active, he/she can not use the application.
    /// </summary>
    public bool IsActive { get; set; }
}
