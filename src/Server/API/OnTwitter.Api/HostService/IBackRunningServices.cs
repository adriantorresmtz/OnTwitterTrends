namespace OnTwitter.Api.HostService
{
    public interface IBackRunningServices
    {
        bool isRunning { get; set; }

        void Dispose();
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}