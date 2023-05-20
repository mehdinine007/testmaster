using MsDemo.Shared.ExtensionsInterfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MsDemo.Shared.ExtensionsImplementions
{
   
    public class ElkRepository<T, X> : IElkRepository<T, X> where T : class
        where X : class
    {
        private readonly ElasticClient _client;
        public ElkRepository(ElasticClient client)
        {
            _client = client;

        }

        public async Task<long> GetCount(Func<QueryContainerDescriptor<T>, QueryContainer> predicate = null, string IndexName = "")
        {
            var request = new CountRequest<T>
            {
                Query = new MatchAllQuery()
            };
             return (await _client.CountAsync<T>(x => x.Query(predicate))).Count;
        }

        public async Task<IndexResponse>  InsertAsync(T obj, string IndexName = "")
        {
            if(string.IsNullOrEmpty(IndexName))
            {
                return await _client.IndexDocumentAsync<T>(obj);
            }
            else
            {
                return await _client.IndexAsync<T>(obj, p => p.Index ( IndexName));

            }
        }
        public void  Insert(T obj, string IndexName = "")
        {
            if (string.IsNullOrEmpty(IndexName))
            {
                  _client.IndexDocument<T>(obj);
            }
            else
            {
                  _client.Index<T>(obj, p => p.Index(IndexName));

            }
        }
        Task<ISearchResponse<X>> IElkRepository<T, X>.Get( Func<SearchDescriptor<X>, Nest.ISearchRequest> predicate = null, string IndexName = "")
        {
            Task<ISearchResponse<X>> ls1;
            if (predicate == null)
            {
                 ls1 = _client.SearchAsync<X>(p =>  p.Query(q =>  q.MatchAll()));
                return ls1;
            }
            else
            {
                 ls1 = _client.SearchAsync<X>(predicate);
                return ls1;
            }
         
        }
    }
}
