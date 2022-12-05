using OnTwitter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTwitter.Application.Common.Interfaces
{
    public interface ITwitterService
    {
        Task<bool> InsertTwitter(TwitterDto twitterDto);
        Task<IEnumerable<TwitterDto>> GetTwitters();
        Task<int> CompletedAsync();
    }
}
