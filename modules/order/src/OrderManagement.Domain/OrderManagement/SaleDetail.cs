using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class SaleDetail : FullAuditedEntity<int>
    {
        private  ICollection<CustomerOrder> _customerOrders;

        private ICollection<AgencySaleDetail> _agencySaleDetails;

        private ICollection<SaleDetailCarColor> _saleDetailCarColors;

        public Guid UID { get; set; }

        public int CircularSaleCode { get; set; } // شماره بخشنامه فروش
        public int SaleId { get; set; }
        public int SalePlanCode { get; set; } // شماره برنامه فروش 

        public string SalePlanDescription { get; set; }

        public int CarTipId { get; set; }

        public DateTime ManufactureDate { get; set; }

        public DateTime SalePlanStartDate { get; set; }

        public DateTime SalePlanEndDate { get; set; }

        public DateTime CarDeliverDate { get; set; }

        public int SaleTypeCapacity { get; set; }

        public double CoOperatingProfitPercentage { get; set; }

        public double RefuseProfitPercentage { get; set; }

        public int ESaleTypeId { get; set; }

        public decimal CarFee { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public int DeliverDaysCount { get; set; }

        public bool Visible { get; set; }

        public virtual CarTip CarTip { get; protected set; }
        public virtual ESaleType ESaleType { get; protected set; }

        public virtual SaleSchema SaleSchema { get; protected set; }

        public virtual ICollection<CustomerOrder> CustomerOrders
        {
            get => _customerOrders ?? (_customerOrders = new List<CustomerOrder>());
            protected set => _customerOrders = value;
        }

        public virtual ICollection<AgencySaleDetail> AgencySaleDetails
        {
            get => _agencySaleDetails ?? (_agencySaleDetails = new List<AgencySaleDetail>()) ;
            protected set => _agencySaleDetails = value;
        }


        public virtual ICollection<SaleDetailCarColor> SaleDetailCarColors
        {
            get => _saleDetailCarColors ?? (_saleDetailCarColors = new List<SaleDetailCarColor>());
            protected set => _saleDetailCarColors = value;
        }
    }
}
