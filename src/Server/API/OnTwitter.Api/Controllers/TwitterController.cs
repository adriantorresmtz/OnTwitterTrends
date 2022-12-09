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

        [HttpGet("/Total")]
        public async Task<ActionResult> GetTwittersTotal()
        {
            return Ok(await _service.GetTwittersTotal());
        }

        [HttpGet("/TopHashTags/{top}")]
        public async Task<ActionResult> GetTopTwittersHashTags(int top)
        {
            var twitters = await _service.GetTwitterHashTagsTop(top);
            return Ok(twitters);
        }

        [HttpGet("/StartService")]
        public async Task<ActionResult> StartService()
        {
            await StartTwitterBackService();

            return Ok(true);
        }

        [HttpGet("/StopService")]
        public async Task<ActionResult> StopService()
        {
            await StopTwitterBackService();

            return Ok(false);
        }

        [HttpGet("/IsRunningService")]
        public ActionResult IsRunningService()
        {
            var result = _backservice.isRunning;
            return Ok(result);
        }

        private async Task StartTwitterBackService() 
        {
            if (!_backservice.isRunning)
            {
                _logger.LogInformation($" Back Service requetest to START");
                _backservice.isRunning = true;
                await _backservice.StartAsync(default);
            }
            else
            {
                _logger.LogInformation($" Back Service is running ");
            }
        }

        private async Task StopTwitterBackService()
        {
            if (_backservice.isRunning)
            {
                _logger.LogInformation($" Back Service requetest to STOP");

                _backservice.isRunning = false;
                await _backservice.StopAsync(default);
            }
            else
            {
                _logger.LogInformation($" Back Service is not running");
            }
        }
    }
}
