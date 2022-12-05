using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnTwitter.Api.HostService;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using OnTwitter.Infrastructure.Services;

namespace OnTwitter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private readonly ITwitterService _service;
        private readonly IBackRunningServices _backservice;

        private readonly ILogger<TwitterController> _logger;


        public TwitterController(ITwitterService service, IBackRunningServices backservice, ILogger<TwitterController> logger)
        {
            _logger = logger;
            _service = service;  
            _backservice = backservice;
        }

        [HttpPost]
        public async Task<bool> CreateTwitter(TwitterDto twitterDto) {

            try
            {
                var result = await _service.InsertTwitter(twitterDto);
                await _service.CompletedAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetTwitters() {
            var twitters = await _service.GetTwitters();
            return Ok(twitters);
        }

        [HttpGet("/StartService")]
        public async Task<ActionResult> StartService()
        {
            _logger.LogInformation($" Back Services requetest to START");
            var result = true;
            _backservice.isRunning = result;
            await _backservice.StartAsync(default);
            return Ok(result);
        }

        [HttpGet("/StopService")]
        public async Task<ActionResult> StopService()
        {
            _logger.LogInformation($" Back Services requetest to STOP");
            var result = false;
            _backservice.isRunning = result;
            await _backservice.StopAsync(default);
            return Ok(result);
        }


    }
}
