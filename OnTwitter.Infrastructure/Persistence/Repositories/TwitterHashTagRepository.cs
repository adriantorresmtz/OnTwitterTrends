using Microsoft.Extensions.Logging;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTwitter.Infrastructure.Persistence
{
    public class TwitterHashTagRepository : GenericRepository<TwitterHashTag>, ITwitterHashTagRepository
    {
        public TwitterHashTagRepository(TwitterDbContext context) : base(context)
        {
        }
    }
}
