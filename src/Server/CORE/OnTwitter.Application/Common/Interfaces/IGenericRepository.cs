
using System.Linq.Expressions;

namespace OnTwitter.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
    Task<bool> Add(T entity);
}
