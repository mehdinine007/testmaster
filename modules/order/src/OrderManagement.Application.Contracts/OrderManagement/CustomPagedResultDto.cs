using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.Application.Contracts
{
    public class CustomPagedResultDto<T>
    {
        public CustomPagedResultDto(IEnumerable<T> data)
        {
            Data = data.ToArray();
        }

        public CustomPagedResultDto(IEnumerable<T> data, int totalCount) : this(data)
        {
            TotalCount = totalCount;
        }

        public CustomPagedResultDto()
        {

        }

        public T[] Data { get; set; }

        public int TotalCount { get; set; }
    }
}