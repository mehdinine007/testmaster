using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CarMakerBlackList : FullAuditedEntity<long>
    {
        public string Nationalcode { get; set; }
        public string Title { get; set; }
        public int EsaleTypeId { get; set; }
        public int? CarMaker { get; set; }
        public int SaleId { get; set; }
    }

    [Table("Province", Schema = "aucbase")]
    public class Province : Entity<int>
    {
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }
        public virtual ICollection<City> ProvinceCities { get; set; }
    }

    [Table("City", Schema = "aucbase")]
    public class City : Entity<int>
    {
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
    }
}
