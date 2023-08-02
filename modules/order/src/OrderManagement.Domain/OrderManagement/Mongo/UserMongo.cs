
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class UserMongo : FullAuditedEntity<ObjectId>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
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
        public int BankId { get; set; }
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
        public short? RegionId { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string Street { get; set; }
        [Column(TypeName = "VARCHAR(10)")]
        public string Pelaq { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string Alley { get; set; }
        public int? Priority { get; set; }
        public string NormalizedUserName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int? CompanyId { get; set; }
        public Guid UID { get; set; }
        public string ChassiNo { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string Vehicle { get; set; }
        public List<string> Roles { get; set; }
        public const int MaxUserNameLength = 256;

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

        /// <summary>
        /// Authorization source name.
        /// It's set to external authentication source name if created by an external source.
        /// Default: null.
        /// </summary>
        [StringLength(MaxAuthenticationSourceLength)]
        public virtual string AuthenticationSource { get; set; }

        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// Tenant Id of this user.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>
        [Required]
        [StringLength(MaxSurnameLength)]
        public virtual string Surname { get; set; }

        /// <summary>
        /// Return full name (Name Surname )
        /// </summary>
        [NotMapped]
        public virtual string FullName { get { return Name + " " + Surname; } }

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required]
        [StringLength(MaxPasswordLength)]
        public virtual string Password { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// </summary>
        [StringLength(MaxEmailConfirmationCodeLength)]
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Reset code for password.
        /// It's not valid if it's null.
        /// It's for one usage and must be set to null after reset.
        /// </summary>
        [StringLength(MaxPasswordResetCodeLength)]
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// Lockout end date.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the access failed count.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the lockout enabled.
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [StringLength(MaxPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Is the <see cref="PhoneNumber"/> confirmed.
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// </summary>
        [StringLength(MaxSecurityStampLength)]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Is two factor auth enabled.
        /// </summary>
        public virtual bool IsTwoFactorEnabled { get; set; }

        /// <summary>
        /// Login definitions for this user.
        /// </summary>


        /// <summary>
        /// Is the <see cref="AbpUserBase.EmailAddress"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Is this user active?
        /// If as user is not active, he/she can not use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }


        public virtual void SetNewPasswordResetCode()
        {
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(MaxPasswordResetCodeLength);
        }

        public virtual void SetNewEmailConfirmationCode()
        {
            EmailConfirmationCode = Guid.NewGuid().ToString("N").Truncate(MaxEmailConfirmationCodeLength);
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }


    }
}
