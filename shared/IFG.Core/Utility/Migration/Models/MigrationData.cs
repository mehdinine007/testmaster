using System;
using System.Collections.Generic;
using System.Text;

namespace IFG.Core.Utility.Migration.Models
{
    public class MigrationData
    {
        public List<MigrationItem> Schema { get; set; }
        public List<MigrationItem> Tables { get; set; }
        public List<MigrationItem> Programmability { get; set; }
        public List<MigrationItem> Patch { get; set; }
    }

    public class MigrationItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
    }
}
