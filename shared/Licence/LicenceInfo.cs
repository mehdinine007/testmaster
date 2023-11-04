using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licence
{
    public class LicenceInfo
    {
        public bool IsValid { get; set; }
        public string SerialNumber { get; set; }

        public string PublicKey { get; set; }

        public List<string> Modules { get; set; }

        public string OrganizationTitle { get; set; }

    }
}
