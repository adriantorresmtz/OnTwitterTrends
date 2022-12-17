
using Moq;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Infrastructure.Services;
using OnTwitter.Test.Helpers;
using AutoMapper;
using OnTwitter.Domain.Entities;
using OnTwitter.Application.Models;
using NuGet.Frameworks;
using FluentAssertions;

namespace OnTwitter.Test;

public class TwitterServiceTests
{
    #region Declarations

    private readonly TwitterService _sut;
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>();

    #endregion

    #region Constructor

    public TwitterServiceTests()
    {
        #region MapperConfiguration
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<AutoMapperProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
        #endregion
        _sut = new TwitterService(_uowMock.Object, _mapper);
    }

    #endregion


    [Fact]
    public async Task InsertTwitter_ShouldReturnTrue()
    {
        // Arrange
        var twitterDto = new TwitterDto() { TwitterId = "458789", TwitterAuthor = "23563214" };
        _uowMock.Setup(x => x.Twitters.Add(It.IsAny<Twitter>())).ReturnsAsync(true);

        //Act
        var result = await _sut.InsertTwitter(twitterDto);

        // Assert
        Assert.True(result);

    }

    [Fact]
    public async Task InsertTwitterHastTag_ShouldReturnTrue()
    {
        // Arrange
        var twitterHashTagDto = new TwitterHashTagDto() { TwitterId = "458789", HashTag = "Developer"};
        _uowMock.Setup(x => x.TwitterHashTags.Add(It.IsAny<TwitterHashTag>())).ReturnsAsync(true);

        //Act
        var result = await _sut.InsertTwitterHastTag(twitterHashTagDto);

        // Assert
        Assert.True(result);

    }


    [Fact]
    public async Task GetTwitters_ShouldReturnTwitters() 
    {
        // Arrange
        var twitters = TwitterDataHelper.GetTwitters_Fake();
        _uowMock.Setup(x => x.Twitters.GetAll()).ReturnsAsync(twitters);

        // Act
        var results = await _sut.GetTwitters();

        // Assert
        Assert.Equal(twitters.Count(), results.Count());
    }


    [Fact]
    public async Task GetTwittersTotal_ShouldReturnTwittersTotal() 
    {
        // Arrange
        var totalTwitters = 5;
        var twitters = TwitterDataHelper.GetTwitters_Fake();
        _uowMock.Setup(x => x.Twitters.GetAll()).ReturnsAsync(twitters);

        // Act
        var result = await _sut.GetTwittersTotal();

        // Assert
        Assert.Equal(totalTwitters, result);
    }

    [Fact]
    public async Task GetTwitters_ShouldReturnTwitterHasTags()
    {
        // Arrange
        var twitterHashTags = TwitterDataHelper.GetTwitterHashTags_Fake();
        _uowMock.Setup(x => x.TwitterHashTags.GetAll()).ReturnsAsync(twitterHashTags);

        // Act
        var results = await _sut.GetTwitterHashTagsTop(10);

        // Assert
        Assert.Equal(5, results.TotalTwitterHashTagsTotal);
        Assert.Equal(3, results.TwitterHashTagTops[0].HashTagTotal);
        Assert.Equal(2, results.TwitterHashTagTops[1].HashTagTotal);

    }


}
