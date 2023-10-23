
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Enums;
using Volo.Abp.Domain.Entities.Auditing;

using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.EventBus;

namespace UserManagement.Domain.UserManagement.Authorization.Users
{
    [EventName("UserSQL")]
    public class UserSQL : FullAuditedEntity<long>, IUsers
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
        public string MongoId { get;set; }
        public const int MaxSecurityStampLength = 128;
        public string NationalCode { get; set; }
        public string FatherName { get; set; }
        public string BirthCertId { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; }
        public string Shaba { get; set; }
        public string PreTel { get; set; }
        public int? BirthCityId { get; set; }
        public int? IssuingCityId { get; set; }
        public int? HabitationCityId { get; set; }
        public int? BirthProvinceId { get; set; }
        public int? IssuingProvinceId { get; set; }
        public int? HabitationProvinceId { get; set; }
        public DateTime? IssuingDate { get; set; }
        public short? RegionId { get; set; }
        public string Street { get; set; }
        public string Pelaq { get; set; }
        public string Alley { get; set; }
        public int? Priority { get; set; }
        public string NormalizedUserName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int? CompanyId { get; set; }
        public string UID { get; set; }
        public string ChassiNo { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string Vehicle { get; set; }
        public string AuthenticationSource { get; set; }
        public string UserName { get; set; }
        public int? TenantId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string EmailConfirmationCode { get; set; }
        public string PasswordResetCode { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsLockoutEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
        public string AllRoles { get;set; }
    }
}
