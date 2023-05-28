using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Esale.Core.DataAccess
{
    public interface IEntityRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> 
        where TEntity : class, IEntity<TPrimaryKey>
    {
        Task<TEntity> AttachAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
