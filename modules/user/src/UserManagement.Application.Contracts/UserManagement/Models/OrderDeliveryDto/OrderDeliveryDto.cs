using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagement.Application.Contracts.Models;
    public class OrderDeliveryDto
    {
        public string NationalCode { get; set; }
        public DateTime? TranDate { get; set; }
        public long? PayedPrice { get; set; }
        public string ContRowId { get; set; }
        public string Vin { get; set; }
        public string BodyNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? FinalPrice { get; set; }
        public string CarDesc { get; set; }
        public long OrderId { get; set; }
        public long Id { get; set; }
    }

