

using UserManagement.Domain.UserManagement.Debugging;

namespace UserManagement.Domain.UserManagement
{
    public class EsaleConsts
    {
        public const string LocalizationSourceName = "Esale";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "178375f2302a4ecd8e1e44e5e7c69b70";
    }
}
