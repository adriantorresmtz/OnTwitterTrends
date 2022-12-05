using AutoMapper;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using OnTwitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTwitter.Infrastructure.Services
{
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
    }
}
