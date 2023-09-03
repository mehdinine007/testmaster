using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;
using Abp.Zero.Configuration;

namespace UserManagement.Domain.UserManagement.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()),
                  new SettingDefinition(
                           AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled,
                           "false",
                           new FixedLocalizableString("Is user lockout enabled."),
                           scopes: SettingScopes.Application | SettingScopes.Tenant,
                           isVisibleToClients: false
                           )

                    
            };
        }
    }
}
