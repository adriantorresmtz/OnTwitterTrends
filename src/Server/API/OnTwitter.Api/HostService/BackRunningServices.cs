using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using Tweetinvi.Models;
using Tweetinvi;
using Tweetinvi.Streaming.V2;
using OnTwitter.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Tweetinvi.Streams;
using Tweetinvi.Parameters.V2;

namespace OnTwitter.Api.HostService;

public class BackRunningServices : IHostedService, IBackRunningServices
{
    #region Declarations

    private readonly IServiceProvider _serviceProvider;
    private readonly ITwitterService _service;
    private readonly ILogger _log;
    private ISampleStreamV2 sampleStreamV2;
    private readonly IHubContext<TwitterReportHub> _hubContext;
    private readonly TwitterClient _appClient;
    public bool isRunning { get; set; }
    public bool isStopRequest { get; set; }

    #endregion

    #region Constructor

    public BackRunningServices(ILogger<BackRunningServices> log, IServiceProvider serviceProvider, IHubContext<TwitterReportHub> hubContext, TwitterClient appClient)
    {
        _log = log;
        _serviceProvider = serviceProvider;
        _appClient = appClient;
        _hubContext = hubContext;

        sampleStreamV2 = _appClient.StreamsV2.CreateSampleStream();
    }

    #endregion

    #region Methods


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation($"{DateTime.Now} BackService is Starting");
        if (isRunning && !isStopRequest)
        {
            _ = GetInfoFromTwitter(cancellationToken);
        }
        else
        {
            isStopRequest = false;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        isRunning = false;
        isStopRequest = true;
        _log.LogInformation($"{DateTime.Now} BackService is Stopping");

        return Task.CompletedTask;
    }

    private async Task GetInfoFromTwitter(CancellationToken cancellationToken)
    {
        _log.LogInformation($"Getting twitters Background Service is working.  {DateTime.Now}");
        try
        {

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ITwitterService scopedProcessingService = scope.ServiceProvider.GetRequiredService<ITwitterService>();

                ProcessTwitters(scopedProcessingService);

                var twitterparams = new StartFilteredStreamV2Parameters();

                await sampleStreamV2.StartAsync();

            }
        }
        catch (Exception ex)
        {
            isRunning = false;
            _log.LogInformation("Error {0}", ex.Message);
            throw;

        }
    }

    private void ProcessTwitters(ITwitterService scopedProcessingService)
    {
        sampleStreamV2.TweetReceived += async (sender, args) =>
        {
            if (isRunning)
            {
                var tweetResp = args.Tweet;

                if (tweetResp.Lang == "en")
                {
                    var twiiterEnty = new TwitterDto() { TwitterAuthor = tweetResp.AuthorId, TwitterId = tweetResp.Id };
                    await scopedProcessingService.InsertTwitter(twiiterEnty);

                    await _hubContext.Clients.All.SendAsync("TwitterTotal", new { data = await scopedProcessingService.GetTwittersTotal() });


                    if (tweetResp.Entities.Hashtags?.Length > 0)
                    {
                        foreach (var hashtag in tweetResp.Entities.Hashtags)
                        {
                            var twiiterHashTagEnty = new TwitterHashTagDto() { HashTag = hashtag.Tag, TwitterId = tweetResp.Id };
                            await scopedProcessingService.InsertTwitterHastTag(twiiterHashTagEnty);

                            await _hubContext.Clients.All.SendAsync("TwitterTop", new { data = await scopedProcessingService.GetTwitterHashTagsTop(10) });
                        }
                    }

                }
            }

            await scopedProcessingService.CompletedAsync();

        };
    }


    public void Dispose()
    {
        isRunning = false;
    }

    #endregion


}