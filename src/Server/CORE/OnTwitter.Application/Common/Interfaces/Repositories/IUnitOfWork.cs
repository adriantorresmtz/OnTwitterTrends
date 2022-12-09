
namespace OnTwitter.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITwitterRepository Twitters { get; }
    ITwitterHashTagRepository TwitterHashTags { get; }
    Task<int> CompletedAsync();
}
