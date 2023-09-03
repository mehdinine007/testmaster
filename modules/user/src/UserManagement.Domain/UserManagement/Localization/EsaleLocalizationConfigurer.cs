using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace UserManagement.Domain.UserManagement.Localization
{
    public static class EsaleLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(EsaleConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(EsaleLocalizationConfigurer).GetAssembly(),
                        "Esale.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
