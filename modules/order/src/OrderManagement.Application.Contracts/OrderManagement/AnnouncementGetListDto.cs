using IFG.Core.Bases;
using Volo.Abp.Application.Dtos;

namespace OrderManagement.Application.Contracts.OrderManagement;

public class AnnouncementGetListDto: PagedResultRequestDto , IIfgSortedResultRequest
{
    public string? AttachmentType { get; set; }
    public string? AttachmentLocation { get; set; }
    public int? CompanyId { get; set; }
    public bool? Active { get; set; }
    public string Sorting { get; set; }
    public SortingType SortingType { get ; set ; }
}
