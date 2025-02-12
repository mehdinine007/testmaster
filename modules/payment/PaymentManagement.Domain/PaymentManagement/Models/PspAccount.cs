﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;

namespace PaymentManagement.Domain.Models
{
    [Table("PspAccount", Schema = "dbo")]
    [Index(nameof(IsActive))]
    public class PspAccount : FullAuditedEntity<int>
    {
        public int PspId { get; set; }
        public int AccountId { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "VARCHAR(500)")]
        public string JsonProps { get; set; }
        [Column(TypeName = "VARCHAR(200)")]
        public string? Logo { get; set; }
        public virtual Psp Psp { get; set; }
        public virtual Account Account { get; set; }
        private ICollection<Payment> _payments;
        public virtual ICollection<Payment> Payments
        {
            get => _payments ??= new List<Payment>();
            protected set => _payments = value;
        }

        #region PspProps
        //[Column(TypeName = "VARCHAR(50)")]
        //public string ParsianPin { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string MellatTerminalId { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string MellatUserName { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string MellatUserPassword { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string SadadMerchantId { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string SadadTerminalId { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string SadadTerminalKey { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string SamanMID { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string IkcoUserName { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string IkcoPassword { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string IranKishTerminalId { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string IranKishAcceptorId { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]
        //public string IranKishPassPhrase { get; set; }
        //[Column(TypeName = "VARCHAR(50)")]

        #endregion
    }
}
