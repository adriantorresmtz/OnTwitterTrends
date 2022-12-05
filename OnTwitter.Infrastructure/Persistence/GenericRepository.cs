using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnTwitter.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace OnTwitter.Infrastructure.Persistence;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TwitterDbContext _context;
    internal DbSet<T> dbSet;
    public GenericRepository(TwitterDbContext context)
    {
        _context = context;
        dbSet = _context.Set<T>();
    }

    public async Task<bool> Add(T entity)
    {
        await dbSet.AddAsync(entity);

        return true;
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
    {
        return await dbSet.Where(expression).ToListAsync();

    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();

    }
}
