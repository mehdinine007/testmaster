using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Contracts
{
    public class PreSaleDto
    {
        [Required]
        [Column(TypeName = "NVARCHAR(150)")]
        public string Brand { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(150)")]
        public string Name { get; set; }
        public int Count { get; set; }
    }
}