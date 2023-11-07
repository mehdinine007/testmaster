﻿using ReportManagement.Domain.Shared.ReportManagement.Dtos;
using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class GridDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CategoryData> Categories { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
    }
}
