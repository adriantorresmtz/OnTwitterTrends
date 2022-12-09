
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
        //// Arrange
        //var testObject = new Twitter() { Id = 1, TwitterId ="123456", TwitterAuthor ="4587898" };
        //var testList = new List<Twitter>() { testObject };

        //var dbSetMock = new Mock<DbSet<Twitter>>();



        //var contextDB = new Mock<TwitterDbContext>();
        //contextDB.Setup(x => x.Set<Twitter>()).Returns(dbSetMock.Object);

        //// Act
        //var TwitterRepo = new GenericRepository<Twitter>(contextDB.Object);
        //var result = TwitterRepo.GetAll();

        //// Assert
        ///

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

    private TwitterDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TwitterDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
        var dbContext = new TwitterDbContext(options);
        return dbContext;
    }
}
