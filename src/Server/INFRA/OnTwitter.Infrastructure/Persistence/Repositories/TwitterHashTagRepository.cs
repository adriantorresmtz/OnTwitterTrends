using Microsoft.Extensions.Logging;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Domain.Entities;

namespace OnTwitter.Infrastructure.Persistence;

public class TwitterHashTagRepository : GenericRepository<TwitterHashTag>, ITwitterHashTagRepository
{
    public TwitterHashTagRepository(TwitterDbContext context) : base(context){}
}
