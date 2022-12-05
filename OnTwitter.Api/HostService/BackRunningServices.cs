using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using Tweetinvi.Models;
using Tweetinvi;

namespace OnTwitter.Api.HostService
{
    public class BackRunningServices : IHostedService, IDisposable, IBackRunningServices
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITwitterService _service;
        private readonly ILogger _log;
        private Timer _timer;
        public bool isRunning { get; set; }
        public BackRunningServices(ILogger<BackRunningServices> log, IServiceProvider serviceProvider)
        {
            _log = log;
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (isRunning)
            {
                GetInfoFromTwitter();
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            isRunning = false;
            _log.LogInformation($"{DateTime.Now} BackService is Stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void  GetInfoFromTwitter()
        {
            _log.LogInformation($"Timed Background Service is working.  {DateTime.Now}");
            try
            {
                

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    ITwitterService scopedProcessingService = scope.ServiceProvider.GetRequiredService<ITwitterService>();

                    

                    var appCredentials = new ConsumerOnlyCredentials("F6V1z0vy5o7i03ODKycwO2vM7", "T7Wu3zUgh5IlcxszdTzGfCR06L6rouC4btPdHSQZvCD8Nv86a9")
                    {
                        BearerToken = "AAAAAAAAAAAAAAAAAAAAAAevjwEAAAAAwNazaY0%2Bbx0uPzHY8%2ByU4iUbKjE%3DOMKenQfY697vfsrUkbIWNSwNpGWJquMvhVPzr9NIaaN3fGul7M"
                    };

                    var appClient = new TwitterClient(appCredentials);
                    var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();

                    sampleStreamV2.TweetReceived += async (sender, args) =>
                    {
                        var tweetResp = args.Tweet;

                        var twiiterEnty = new TwitterDto() { TwitterAuthor = tweetResp.AuthorId, TwitterId = tweetResp.Id};
                        await scopedProcessingService.InsertTwitter(twiiterEnty);
                        await scopedProcessingService.CompletedAsync();

                        if (tweetResp.Entities.Hashtags?.Length > 0)
                        {
                            foreach (var hashtag in tweetResp.Entities.Hashtags)
                            {
                                Console.WriteLine($"Id  => {tweetResp.Id} with HashTag {hashtag.Tag}");
                            }

                        }

                    };

                    await sampleStreamV2.StartAsync();



                }
            }
            catch (Exception ex)
            {
                isRunning = false;
                _log.LogInformation("Error {0}", ex.Message);
                throw ex;

            }
        }
    }
}
