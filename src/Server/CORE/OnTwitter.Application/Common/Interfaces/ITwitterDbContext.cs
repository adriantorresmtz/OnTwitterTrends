using Microsoft.EntityFrameworkCore;
using OnTwitter.Domain.Entities;

namespace OnTwitter.Application.Common.Interfaces;

public interface ITwitterDbContext
{
    public DbSet<Twitter> Twitters { get; set; }
    public DbSet<TwitterHashTag> TwitterHashTags { get; set; }

    Task<int> SaveChangesAsync();
}
