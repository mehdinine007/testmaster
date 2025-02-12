﻿using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class SaleDetail : FullAuditedEntity<int>
{
    private ICollection<CustomerOrder> _customerOrders;

    private ICollection<AgencySaleDetail> _agencySaleDetails;

    private ICollection<SaleDetailCarColor> _saleDetailCarColors;

    private ICollection<SaleDetailAllocation> _seasonCompanyProducts;

    public Guid UID { get; set; }
    public string Title { get; set; }
    public int CircularSaleCode { get; set; } // شماره بخشنامه فروش
    public int SaleId { get; set; }
    public int SalePlanCode { get; set; } // شماره برنامه فروش 

    public string SalePlanDescription { get; set; }

    public int? CarTipId { get; set; }

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

    public SaleProcessType SaleProcess { get; set; }
    public string CompanySaleId { get; set; }

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
        get => _agencySaleDetails ?? (_agencySaleDetails = new List<AgencySaleDetail>());
        protected set => _agencySaleDetails = value;
    }


    public virtual ICollection<SaleDetailCarColor> SaleDetailCarColors
    {
        get => _saleDetailCarColors ?? (_saleDetailCarColors = new List<SaleDetailCarColor>());
        protected set => _saleDetailCarColors = value;
    }

    public ICollection<SaleDetailAllocation> SeasonCompanyProducts
    {
        get => _seasonCompanyProducts ??= new List<SaleDetailAllocation>();
        protected set => _seasonCompanyProducts = value;
    }

    [ForeignKey("ProductAndCategory")]
    public int ProductId { get; set; }
    public virtual ProductAndCategory Product { get; protected set; }


}
