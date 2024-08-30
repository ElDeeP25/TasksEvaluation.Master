using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TasksEvaluation.Core.consts;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Infrastructure.Data;

namespace TasksEvaluation.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();


        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create(T model)
        {
            await DbSet.AddAsync(model);
            await SaveChangesAsync();
            return model;
        }

        public async Task Delete(T model)
        {
            DbSet.Remove(model);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll() => await DbSet.AsNoTracking().ToListAsync();

        public async Task<T> GetById<IdType>(IdType id)
        {
            var data = await DbSet.FindAsync(id);
            return data is null ? throw new InvalidOperationException("No data Found") : data;
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task Update(T model)
        {
            DbSet.Update(model);
            await SaveChangesAsync();
        }

        public async Task<Student> GetByEmail(string email)
        {
            var data = await _dbContext.Students.FirstOrDefaultAsync(t => t.Email == email);
            return data is null ? throw new InvalidOperationException("No data Found") : data;
        }

        public async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> match) => await DbSet.Where(match).AsNoTracking().ToListAsync();

        public async Task<T?> Find(Expression<Func<T, bool>> predicate) =>
    await _dbContext.Set<T>().SingleOrDefaultAsync(predicate);

        public async Task<T?> Find(Expression<Func<T, bool>> predicate, string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<T?> Find(Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

            if (include is not null)
                query = include(query);

            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(predicate);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(predicate);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

            if (include is not null)
                query = include(query);

            query = query.Where(predicate);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

       
    }
}

