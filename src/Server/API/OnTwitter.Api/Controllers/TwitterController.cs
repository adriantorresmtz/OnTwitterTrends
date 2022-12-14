using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OnTwitter.Api.HostService;
using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Application.Models;
using OnTwitter.Infrastructure.Services;

namespace OnTwitter.Api.Controllers;

[ApiController]
[Route("api/twitter/v1")]
public class TwitterController : ControllerBase
{
    #region declarations

    private readonly IBackRunningServices _backservice;
    private readonly ILogger<TwitterController> _logger;

    #endregion

    #region Constructor
    public TwitterController(IBackRunningServices backservice, ILogger<TwitterController> logger)
    {
        _logger = logger;
        _backservice = backservice;
    }

    #endregion

    #region Endpoints

    [HttpGet("start")]
    public async Task<IActionResult> Start()
    {
        await StartTwitterBackService();

        return Ok();
    }

    [HttpGet("stop")]
    public async Task<IActionResult> Stop()
    {
        await StopTwitterBackService();

        return Ok();
    }

    [HttpGet("servicerunning")]
    public IActionResult IsRunningService()
    {
        var result = _backservice.isRunning;
        return Ok(result);
    }

    #endregion


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
