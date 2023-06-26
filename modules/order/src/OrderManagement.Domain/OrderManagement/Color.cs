using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class Color:FullAuditedEntity<int>
    {
        private ICollection<SaleDetailCarColor> _saleDetailCarColor;
        public string ColorName { get; set; }
        public int ColorCode { get; set; }


        public virtual ICollection<SaleDetailCarColor> SaleDetailCarColor
        {
            get => _saleDetailCarColor ?? (_saleDetailCarColor = new List<SaleDetailCarColor>());
            protected set => _saleDetailCarColor = value;
        }
    }
}
