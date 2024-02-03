using IFG.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AdvertisementDetailPaginationDto : PagedResultRequestDto, IIfgSortedResultRequest
    {
        public int AdvertisementId { get; set; }
        public string Sorting { get; set; }
        public SortingType SortingType { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentLocation { get; set; }
    }
}
