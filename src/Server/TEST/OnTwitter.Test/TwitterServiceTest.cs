
using AutoMapper;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Frameworks;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using OnTwitter.Domain.Entities;
using OnTwitter.Infrastructure.Persistence;
using OnTwitter.Infrastructure.Services;


namespace OnTwitter.Test;

public class TwitterServiceTest
{
    Mock<TwitterDbContext> mockContext;

    private IEnumerable<Twitter> twittersDBSet { get; set; }
    public TwitterServiceTest()
    {
        mockContext = new Mock<TwitterDbContext>();
        mockContext.Setup(a => a.Set<Twitter>()).Returns(Mock.Of<DbSet<Twitter>>);
        mockContext.Setup(a => a.Set<TwitterHashTag>()).Returns(Mock.Of<DbSet<TwitterHashTag>>);
    }


    [Fact]
    public async void InsertTwitter()
    {
        // Arrange
        var Twitter = new Twitter() { TwitterAuthor = "1234567", TwitterId = "9145245887" };
        var TwitterDTO = new TwitterDto() { TwitterAuthor = "1234567", TwitterId = "9145245887" };
        var TwitterResult = new Twitter() { Id = 1, TwitterAuthor = "1234567", TwitterId = "9145245887" };

        var mappingService = new Mock<IMapper>();
        var unitOfWorkMock = new UnitOfWork(mockContext.Object);
        mappingService.Setup(mock => mock.Map<Twitter>(It.IsAny<TwitterDto>())).Returns(TwitterResult);

        ITwitterService sut = new TwitterService(unitOfWorkMock, mappingService.Object);
        
        var TwitterRepo = new Mock<ITwitterRepository>();
        TwitterRepo.Setup(m => m.Add(Twitter)).Returns(Task.FromResult(true));

        //Act

        var actual = await sut.InsertTwitter(TwitterDTO);
        await sut.CompletedAsync();

        //Assert
        TwitterRepo.Verify();
        Assert.True(actual);

    }

    [Fact]
    public async void InsertTwitterHashTag()
    {
        // Arrange
        var TwitterHashTag = new TwitterHashTag() { HashTag = "Development", TwitterId = "1234567" };
        var TwitterHashTagDTO = new TwitterHashTagDto() { HashTag = "Development", TwitterId = "1234567" };
        var TwitterHashTagResult = new TwitterHashTag() { Id = 1, HashTag = "Development", TwitterId = "1234567" };

        var mappingService = new Mock<IMapper>();
        var unitOfWorkMock = new UnitOfWork(mockContext.Object);
        mappingService.Setup(mock => mock.Map<TwitterHashTag>(It.IsAny<TwitterHashTagDto>())).Returns(TwitterHashTagResult);


        ITwitterService sut = new TwitterService(unitOfWorkMock, mappingService.Object);


        var TwitterHashTagRepo = new Mock<ITwitterHashTagRepository>();
        TwitterHashTagRepo.Setup(m => m.Add(TwitterHashTag)).Returns(Task.FromResult(true));

        //Act

        var actual = await sut.InsertTwitterHastTag(TwitterHashTagDTO);
        await sut.CompletedAsync();

        //Assert
        TwitterHashTagRepo.Verify();
        Assert.True(actual);

    }

    [Fact]
    public async Task GetTwittersAsync()
    {
        // Arrange
        var mockContext = CreateDbContext();
        var sut = new GenericRepository<Twitter>(mockContext);
        var twitter = new Twitter { TwitterAuthor ="4598789", TwitterId ="7854111" };
        //Act
        var result = await sut.Add(twitter);
        await mockContext.SaveChangesAsync();
        Assert.True(result);

        //Assert
        Assert.True(mockContext.Twitters.Count() == 1);

    }

    [Fact]
    public async Task GetTwitterHashTagsAsync()
    {
        // Arrange
        var mockContext = CreateDbContext();
        var sut = new GenericRepository<TwitterHashTag>(mockContext);
        var twitterhashTag = new TwitterHashTag { HashTag = "Development", TwitterId = "7854111" };
        //Act
        var result = await sut.Add(twitterhashTag);
        await mockContext.SaveChangesAsync();
        Assert.True(result);

        //Assert
        Assert.True(mockContext.TwitterHashTags.Count() == 1);

    }

    [Fact]
    public async Task GetTotalTwitters()
    {
        // Arrange

        var mockContext = CreateDbContext();
        var repoTwitter = new GenericRepository<Twitter>(mockContext);
        var twitter = new Twitter {TwitterId = "7854111", TwitterAuthor = "1548878987"};
        //Act
        var result = await repoTwitter.Add(twitter);
        await mockContext.SaveChangesAsync();

        var TwitterResult = new Twitter() { Id = 1, TwitterId = "7854111", TwitterAuthor = "1548878987" };

        var mappingService = new Mock<IMapper>();
        var unitOfWorkMock = new UnitOfWork(mockContext);
        mappingService.Setup(mock => mock.Map<Twitter>(It.IsAny<TwitterDto>())).Returns(TwitterResult);

        ITwitterService sut = new TwitterService(unitOfWorkMock, mappingService.Object);

        //Act
        var response = await sut.GetTwittersTotal();

        //Assert
        Assert.True(response == 1);
    }

    [Fact]
    public async Task GetTotalTwitterHashTags() {
        // Arrange

        var mockContext = CreateDbContext();
        var repoTwitterHashTags = new GenericRepository<TwitterHashTag>(mockContext);
        var twitterhashTag = new TwitterHashTag { HashTag = "Development", TwitterId = "7854111" };
        //Act
        var result = await repoTwitterHashTags.Add(twitterhashTag);
        await mockContext.SaveChangesAsync();

        var TwitterHashTagResult = new TwitterHashTag() { Id = 1, HashTag = "Development", TwitterId = "7854111" };

        var mappingService = new Mock<IMapper>();
        var unitOfWorkMock = new UnitOfWork(mockContext);
        mappingService.Setup(mock => mock.Map<TwitterHashTag>(It.IsAny<TwitterHashTagDto>())).Returns(TwitterHashTagResult);

        ITwitterService sut = new TwitterService(unitOfWorkMock, mappingService.Object);


        //Act
        var response = await sut.GetTwitterHashTagsTop(1);

        //Assert
        Assert.True(response.TotalTwitterHashTagsTotal == 1);
    }

    private TwitterDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TwitterDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
        var dbContext = new TwitterDbContext(options);
        return dbContext;
    }
}
