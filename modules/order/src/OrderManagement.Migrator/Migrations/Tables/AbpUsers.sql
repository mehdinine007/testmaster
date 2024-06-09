if not exists(select 1 from sysObjects where Upper(Name)= 'AbpUsers' ) begin
exec(N'CREATE TABLE [dbo].[AbpUsers](
[Id] [bigint] IDENTITY(1,1) NOT NULL,
[AccessFailedCount] [int] NOT NULL,
[AuthenticationSource] [nvarchar](64) NULL,
[ConcurrencyStamp] [nvarchar](128) NULL,
[CreationTime] [datetime2](7) NOT NULL,
[CreatorUserId] [bigint] NULL,
[DeleterUserId] [bigint] NULL,
[DeletionTime] [datetime2](7) NULL,
[EmailAddress] [nvarchar](256) NOT NULL,
[EmailConfirmationCode] [nvarchar](328) NULL,
[IsActive] [bit] NOT NULL,
[IsDeleted] [bit] NOT NULL,
[IsEmailConfirmed] [bit] NOT NULL,
[IsLockoutEnabled] [bit] NOT NULL,
[IsPhoneNumberConfirmed] [bit] NOT NULL,
[IsTwoFactorEnabled] [bit] NOT NULL,
[LastModificationTime] [datetime2](7) NULL,
[LastModifierUserId] [bigint] NULL,
[LockoutEndDateUtc] [datetime2](7) NULL,
[Name] [nvarchar](64) NOT NULL,
[NormalizedEmailAddress] [nvarchar](256) NULL,
[NormalizedUserName] [nvarchar](256) NOT NULL,
[Password] [nvarchar](128) NOT NULL,
[PasswordResetCode] [nvarchar](328) NULL,
[PhoneNumber] [nvarchar](32) NULL,
[SecurityStamp] [nvarchar](128) NULL,
[Surname] [nvarchar](64) NOT NULL,
[TenantId] [int] NULL,
[UserName] [nvarchar](20) NOT NULL,
[Address] [nvarchar](255) NOT NULL,
[BirthCertId] [nvarchar](11) NOT NULL,
[BirthDate] [datetime2](7) NULL,
[FatherName] [nvarchar](150) NOT NULL,
[Gender] [tinyint] NOT NULL,
[Mobile] [varchar](11) NOT NULL,
[NationalCode] [nchar](20) NOT NULL,
[PostalCode] [nvarchar](10) NOT NULL,
[Tel] [varchar](12) NOT NULL,
[AccountNumber] [nvarchar](50) NOT NULL,
[BankId] [int] NULL,
[Shaba] [varchar](28) NOT NULL,
[BirthCityId] [int] NULL,
[HabitationCityId] [int] NULL,
[IssuingCityId] [int] NULL,
[Pelaq] [varchar](10) NULL,
[PreTel] [varchar](6) NULL,
[RegionId] [smallint] NULL,
[Street] [nvarchar](100) NULL,
[Alley] [nvarchar](100) NULL,
[IssuingDate] [datetime2](7) NULL,
[Priority] [int] NULL,
[BirthProvinceId] [int] NULL,
[HabitationProvinceId] [int] NULL,
[IssuingProvinceId] [int] NULL,
[CompanyId] [int] NULL,
[UID] [uniqueidentifier] NOT NULL,
[ChassiNo] [nvarchar](max) NULL,
[EngineNo] [nvarchar](max) NULL,
[Vehicle] [nvarchar](max) NULL,
[Vin] [nvarchar](max) NULL,
[AllRoles] [nvarchar](500) NULL,
[MongoId] [nvarchar](36) NULL,
[CreatorId] [uniqueidentifier] NULL,
[DeleterId] [uniqueidentifier] NULL,
[LastModifierId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AbpUsers] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF_AbpUsers_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__Addres__61BB7BD9]  DEFAULT (N'''') FOR [Address]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__BirthC__60C757A0]  DEFAULT (N'''') FOR [BirthCertId]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__BirthD__10566F31]  DEFAULT (''0001-01-01T00:00:00.0000000'') FOR [BirthDate]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__Father__5FD33367]  DEFAULT (N'''') FOR [FatherName]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__Gender__114A936A]  DEFAULT (CONVERT([tinyint],(0))) FOR [Gender]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__Mobile__5EDF0F2E]  DEFAULT ('''') FOR [Mobile]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__Nation__123EB7A3]  DEFAULT (N'''') FOR [NationalCode]')
exec(N'ALTER TABLE [dbo].[AbpUsers] ADD  CONSTRAINT [DF__AbpUsers__Accoun__62AFA012]  DEFAULT (N'''') FOR [AccountNumber]')
end
