using AutoMapper;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using OnTwitter.Domain.Entities;


namespace OnTwitter.Infrastructure.Services;

public class TwitterService : ITwitterService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TwitterService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> InsertTwitter(TwitterDto twitterDto) { 
        var twitterEntity = _mapper.Map<Twitter>(twitterDto);
        return await _unitOfWork.Twitters.Add(twitterEntity);
    }
    

    public async Task<IEnumerable<TwitterDto>> GetTwitters()
    {
       var twitters = await _unitOfWork.Twitters.GetAll();
        return _mapper.Map<IEnumerable<TwitterDto>>(twitters);
    }

    public async Task<int> CompletedAsync()
    {
        return await _unitOfWork.CompletedAsync();
    }

    public async Task<double> GetTwittersTotal()
    {
        var twitters = await _unitOfWork.Twitters.GetAll();
        return twitters.Count();
    }

    public async Task<bool> InsertTwitterHastTag(TwitterHashTagDto twitterHashDto)
    {
        var twitterHashTagEntity = _mapper.Map<TwitterHashTag>(twitterHashDto);
        return await _unitOfWork.TwitterHashTags.Add(twitterHashTagEntity);
    }

    public async Task<TwitterHashTagTopData> GetTwitterHashTagsTop(int Top)
    {
        var twittersHashTags = await _unitOfWork.TwitterHashTags.GetAll();
        var twittersHashTagsList = twittersHashTags.GroupBy(g=> g.HashTag)
            .OrderByDescending(gp => gp.Count())
            .Take(Top)
            .Select(s => new TwitterHashTagTop { HashTagName = s.Key, HashTagTotal = s.Count()}).ToList();
        

        return new TwitterHashTagTopData() { TotalTwitterHashTagsTotal = twittersHashTags.Count(), TwitterHashTagTops = twittersHashTagsList };

    }
}
