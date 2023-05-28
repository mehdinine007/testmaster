
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Esale.Core.DataAccess
{
    public class EfEntityRepositoryBase<TDbContext,TEntity, TKey> : EfCoreRepository<TDbContext, TEntity, TKey>, IEntityRepository<TEntity, TKey>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TKey>
    {
        public EfEntityRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TEntity> AttachAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var dbContext = await GetDbContextAsync();
            dbContext.Attach(entity);
            if (properties.Length > 0)
            {
                foreach (var propertyAccessor in properties)
                {
                    MemberExpression expression;
                    if (propertyAccessor.Body is UnaryExpression)
                        expression = ((UnaryExpression)propertyAccessor.Body).Operand as MemberExpression;
                    else
                        expression = (MemberExpression)propertyAccessor.Body;

                    string propertyName = expression.Member.Name;
                    dbContext.Entry(entity).Property(propertyName).IsModified = true;
                }
            }
            else
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }
    }
}
