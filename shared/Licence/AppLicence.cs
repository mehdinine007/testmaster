namespace Licence
{
    public class AppLicence
    {
        public static string Version = "4.7.0-aplpha.1";
        public static string FixVersion = "4.7.0";
        public static string ReleaseDate = "1403/02/23";
        public static LicenceInfo GetLicence(string serialNumber)
        {
            LicenceInfo licenceInfo = new LicenceInfo();
            if (!(serialNumber == "19390"))
            {
                if (serialNumber == "18910")
                {
                    licenceInfo = new LicenceInfo
                    {
                        SerialNumber = serialNumber,
                        PublicKey = "",
                        Modules = new List<string> { "0001", "002", "003", "004" },
                        OrganizationTitle = "شرکت ایران خودرو دیزل",
                    };
                }
            }
            else
            {
                licenceInfo = new LicenceInfo
                {
                    SerialNumber = serialNumber,
                    PublicKey = "",
                    Modules = new List<string> { "001", "002", "003", "004" },
                    OrganizationTitle = "شرکت ایزان فاوا گسترش",
                };
            }

            licenceInfo.IsValid = CheckLicence(licenceInfo);
            return licenceInfo;
        }

        public static bool CheckLicence(LicenceInfo licenceInfo)
        {
            return !string.IsNullOrWhiteSpace(licenceInfo.SerialNumber);
        }

        public static VersionInfo GetVersion(string serialNumber)
        {
            var licenceInfo = GetLicence(serialNumber);
            return new VersionInfo()
            {
                OrganizationTitle = licenceInfo.OrganizationTitle,
                Version = Version,
                FixVersion = FixVersion,
                ReleaseDate = ReleaseDate
            }; 
        }
    }
}