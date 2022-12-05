using Tweetinvi;
using Tweetinvi.Models;
using Microsoft.Extensions.Hosting;
using OnTwitter.Application.Common.Interfaces;
using AutoMapper;
using OnTwitter.Domain.Entities;

namespace OnTwitter.Service;

public class TwitterReader : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // You need to use ConsumerOnly credentials to run a sample stream from labs
            var appCredentials = new ConsumerOnlyCredentials("F6V1z0vy5o7i03ODKycwO2vM7", "T7Wu3zUgh5IlcxszdTzGfCR06L6rouC4btPdHSQZvCD8Nv86a9")
            {
                BearerToken = "AAAAAAAAAAAAAAAAAAAAAAevjwEAAAAAwNazaY0%2Bbx0uPzHY8%2ByU4iUbKjE%3DOMKenQfY697vfsrUkbIWNSwNpGWJquMvhVPzr9NIaaN3fGul7M"
            };

            var appClient = new TwitterClient(appCredentials);
            var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();

            sampleStreamV2.TweetReceived += async (sender, args) =>
            {
                var tweetResp = args.Tweet;

               
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

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Reading from Twitter stop");
        return Task.CompletedTask;
    }
}