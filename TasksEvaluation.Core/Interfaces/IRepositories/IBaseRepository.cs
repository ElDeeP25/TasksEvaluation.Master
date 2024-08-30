using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.consts;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> match);
        Task<T> GetById<IdType>(IdType id);
        Task<T> Create(T model);
        Task Update(T model);
        Task Delete(T model);
        Task SaveChangesAsync();
        Task<Student> GetByEmail(string email);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<T?> Find(Expression<Func<T, bool>> predicate, string[]? includes = null);
        Task<T?> Find(Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending);
    }
}
