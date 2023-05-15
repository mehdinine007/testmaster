using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MsDemo.Shared.ExtensionsInterfaces
{
    public interface IElkRepository<T, X> where T : class
        where X : class
    {
        Task<IndexResponse> InsertAsync(T obj, string IndexName = "");
        void Insert(T obj, string IndexName = "");

        Task<ISearchResponse<X>> Get(Func<SearchDescriptor<X>, Nest.ISearchRequest> predicate = null, string IndexName = "");
        Task<long> GetCount(Func<QueryContainerDescriptor<T>, QueryContainer> predicate = null, string IndexName = "");


    }
}
