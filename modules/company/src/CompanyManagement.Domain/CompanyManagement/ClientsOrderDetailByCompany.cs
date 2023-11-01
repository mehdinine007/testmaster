﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CompanyManagement.Domain.CompanyManagement
{
    public class ClientsOrderDetailByCompany : FullAuditedEntity<long>
    {
        public string NationalCode { get; set; }

        public int? SaleType { get; set; }

        public int? ModelType { get; set; }

        public DateTime? TurnDate { get; set; }

        public DateTime? InviteDate { get; set; }

        public DateTime? TranDate { get; set; }

        public long? PayedPrice { get; set; }

        public int ContRowId { get; set; }

        public DateTime? ContRowIdDate { get; set; }

        public string Vin { get; set; }

        public string BodyNumber { get; set; }

        public decimal? CooperateBenefit { get; set; }

        public decimal? CancelBenefit { get; set; }

        public decimal? DelayBenefit { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? FinalPrice { get; set; }

        public string CarDesc { get; set; }

        public long OrderId { get; set; }
    }
}
