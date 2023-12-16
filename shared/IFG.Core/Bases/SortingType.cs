#nullable disable
using Volo.Abp.Application.Dtos;

namespace IFG.Core.Bases;

public enum SortingType
{
    Asc = 1,
    Desc = 2
}

public interface IIfgSortedResultRequest : ISortedResultRequest
{
    SortingType SortingType { get; set; }
}