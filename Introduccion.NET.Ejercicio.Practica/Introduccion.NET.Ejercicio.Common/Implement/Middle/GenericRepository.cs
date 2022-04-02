using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly IServiceScopeFactory _serviceScope;
        private readonly IHttpContextAccessor _http;

        public GenericRepository(IServiceScopeFactory serviceScope, IHttpContextAccessor http)
        {
            _serviceScope = serviceScope;
            _http = http;
        }

        #region Create

        public async Task<TEntity> Create(TEntity entity)
        {
            using var scope = _serviceScope.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            _ = await context.Set<TEntity>().AddAsync(entity.SetDefaultValues(_http));
            var affectedRecords = await context.SaveChangesAsync();
            return affectedRecords > 0 ? entity : null;
        }


        #endregion

        #region Read


        public async Task<List<TEntity>> ReadAll()
        {
            using var scope = _serviceScope.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<PagedResult<TEntity>> Read(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page = 0, int size = 0)
        {
            using var scope = _serviceScope.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            if (!orderBy.IsEmpty())
                query = orderBy(query);
            return await query.Paginate(page, size, orderBy);
        }

        public async Task<PagedResult<TEntity>> Read(Expression<Func<TEntity, bool>> expression,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                                                  int page = 0,
                                                  int size = 0)
        {
            using var scope = _serviceScope.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var query = context.Set<TEntity>().AsNoTracking();
            if (!orderBy.IsEmpty())
                query = orderBy(query);
            if (!include.IsEmpty())
                query = include(query);
            query = query.Where(expression).AsQueryable();
            return await query.Paginate(page, size, orderBy);
        }

        public async Task<List<TEntity>> Read(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                using var scope = _serviceScope.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                var query = context.Set<TEntity>().AsNoTracking();
                return await query.Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        #endregion

        #region Update

        public async Task<bool> Update(TEntity entity)
        {
            using var scope = _serviceScope.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.Update(entity.SetDefaultValues(_http));
            var affectedRecords = await context.SaveChangesAsync();
            return affectedRecords > 0;
        }

        #endregion

        #region Delete

        public async Task<bool> Delete(IEnumerable<TEntity> data)
        {
            using var scope = _serviceScope.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.RemoveRange(data);
            int affectedRecords = await context.SaveChangesAsync();
            return affectedRecords > 0;
        }

        #endregion

    }
}
