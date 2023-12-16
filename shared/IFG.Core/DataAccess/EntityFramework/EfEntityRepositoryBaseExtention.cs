using IFG.Core.Bases;
using IFG.Core.Utility.Tools;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace IFG.Core.DataAccess
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

        public static void AddEnumChangeTracker<TEntity, TEnum>(this EntityTypeBuilder<TEntity> entityTypeBuilder)
            where TEntity : BaseReadOnlyTable, new()
            where TEnum : struct, Enum
        {
            // Add control to ensure enum inherited from int32
            var enumDatas = Enum.GetValues<TEnum>()
                .Cast<TEnum>()
                .Select(x => new Tuple<int, string, string>(Convert.ToInt32(x), x.ToString(), x.GetDisplayName()))
                .OrderBy(x => x.Item1)
                .ToList();
            entityTypeBuilder.Property(x => x.Title)
                .HasMaxLength(250);
            entityTypeBuilder.Property(x => x.Title_En)
                .HasMaxLength(250);
            var recordsToInsert = new List<TEntity>(enumDatas.Capacity);
            for (var i = 0; i < enumDatas.Count; i++)
            {
                var record = enumDatas[i];
                recordsToInsert.Add(new TEntity
                {
                    Code = record.Item1,
                    Id = i + 1,
                    Title = record.Item3,
                    Title_En = record.Item2,
                });
            }
            entityTypeBuilder.HasData(recordsToInsert);
        }

        public static void AddFullEnumChangeTracker<TEntity, TEnum>(this EntityTypeBuilder<TEntity> entityTypeBuilder)
            where TEntity : ReadOnlyTableWithDescription, new()
            where TEnum : struct, Enum
        {
            // Add control to ensure enum inherited from int32
            var enumDatas = Enum.GetValues<TEnum>()
                .Cast<TEnum>()
                .Select(x => new Tuple<int, string, string, string>(Convert.ToInt32(x), x.ToString(), x.GetDisplayName(), x.GetDisplayDescription()))
                .OrderBy(x => x.Item1)
                .ToList();
            entityTypeBuilder.Property(x => x.Title)
                .HasMaxLength(250);
            entityTypeBuilder.Property(x => x.Title_En)
                .HasMaxLength(250);
            entityTypeBuilder.Property(x => x.Description)
                .HasMaxLength(500);
            var recordsToInsert = new List<TEntity>(enumDatas.Capacity);
            for (var i = 0; i < enumDatas.Count; i++)
            {
                var record = enumDatas[i];
                recordsToInsert.Add(new TEntity
                {
                    Code = record.Item1,
                    Id = i + 1,
                    Title = record.Item3,
                    Title_En = record.Item2,
                    Description = record.Item4
                });
            }
            entityTypeBuilder.HasData(recordsToInsert);
        }

        public static IQueryable<TEntity> SortByRule<TEntity>(this IQueryable<TEntity> query, IIfgSortedResultRequest input)
            where TEntity : IEntity
        {
            if (input is IIfgSortedResultRequest sortInput)
            {
                if (!string.IsNullOrEmpty(sortInput.Sorting))
                {
                    return input.SortingType == SortingType.Asc
                        ? query.OrderBy(x => EF.Property<object>(x, sortInput.Sorting))
                        : query.OrderByDescending(x => EF.Property<object>(x, sortInput.Sorting));
                }
            }
            return query;
        }
    }
}
