﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Application.Contracts
{
    public class UserRejectionAdvocacyDto
    {
        public string NationalCode { get; set; }
        public DateTime datetime { get; set; }
        [Column(TypeName = "VARCHAR(26)")]
        public string ShabaNumber { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        public string accountNumber { get; set; }
    }
}