using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Domain.Entities;

namespace OnTwitter.Infrastructure.Persistence
{
    public class TwitterDbContext : DbContext, ITwitterDbContext
    {
        #region Constructor

        public TwitterDbContext(DbContextOptions<TwitterDbContext> options) : base(options) { }
        #endregion


        #region DbSets
        public DbSet<Twitter> Twitters { get; set; }
        public DbSet<TwitterHashTag> TwitterHashTags { get; set; }

        #endregion

        #region Methods
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        #endregion

    }
}
