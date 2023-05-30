using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain
{
    [Table("Province", Schema = "aucbase")]
    public class Province : Entity<int>
    {
        private ICollection<Agency> _agencies;

        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }
        public virtual ICollection<City> ProvinceCities { get; set; }
        public virtual ICollection<Agency> Agencies
        {
            get => _agencies ?? (_agencies = new List<Agency>());
            protected set => _agencies = value;
        }
    }
}
