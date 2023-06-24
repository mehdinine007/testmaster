
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Esale.Core.DataAccess
{
    public static class EfEntityRepositoryBaseExtention
    {
        public static async Task<TEntity> AttachAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        where TEntity : class, IEntity
        {
            var dbContext = repository.ToEfCoreRepository().DbContext;// await GetDbContextAsync();
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

            dbContext.SaveChanges();
            dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public static async Task GetTracker<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, TEntity entity)
         where TEntity : class, IEntity
        {
            var dbContext = repository.ToEfCoreRepository().DbContext;// await GetDbContextAsync();
            var x = dbContext.ChangeTracker.Entries<TEntity>().ToList();

        }
        public static async Task RemoveTracking<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, TEntity entity)
       where TEntity : class, IEntity
        {
            var dbContext = repository.ToEfCoreRepository().DbContext;// await GetDbContextAsync();
            dbContext.Entry(entity).State = EntityState.Detached;


        }
    }
}
