using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using System.Threading;
using System.Transactions;
using UserManagement.Domain.Authorization.Roles;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Configuration;
using UserManagement.Domain.UserManagement.Localization;
using UserManagement.Domain.UserManagement.MultiTenancy;
using UserManagement.Domain.UserManagement.Timing;

namespace UserManagement.Domain.UserManagement
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class EsaleCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            EsaleLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = EsaleConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            Configuration.Localization.Languages.Clear();
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = EsaleConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = EsaleConsts.DefaultPassPhrase;
            Configuration.UnitOfWork.IsTransactional = false;
            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.ReadUncommitted;

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EsaleCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.ReadUncommitted;

        }
    }
}
