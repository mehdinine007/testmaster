using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ChartStructureDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChartTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public List<CategoryData> Categories { get; set; }
        public List<ChartSeriesData> Series { get; set; }
        public int Priority { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }

    }

    public class ChartSeriesData
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<int> Data { get; set; }
    }

    public class CategoryData
    {
        public string Title { get; set; }
        public string Color { get; set; }
    }


}
