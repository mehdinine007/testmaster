using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace UserManagement.Domain
{
    public class Logs : Entity<long>
    {

        [Column(TypeName = "VARCHAR(100)")]
        public string Method { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "VARCHAR(max)")]
        public string Message { get; set; }
        public Int16 Type { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string Servername { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string Ip { get; set; }
        public int LocationId { get; set; }

    }
}
