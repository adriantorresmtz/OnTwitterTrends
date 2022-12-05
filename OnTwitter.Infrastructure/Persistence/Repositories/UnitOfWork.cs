using Microsoft.Extensions.Logging;
using OnTwitter.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTwitter.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TwitterDbContext _context;

    public UnitOfWork(TwitterDbContext context)
    {
        _context = context;
        Twitters = new TwitterRepository(_context);
        TwitterHashTags = new TwitterHashTagRepository(_context);
    }

    public ITwitterRepository Twitters { get; private set; }

    public ITwitterHashTagRepository TwitterHashTags { get; private set; }

    public async Task<int> CompletedAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
