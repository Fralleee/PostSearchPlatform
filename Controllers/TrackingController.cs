using PostSearchPlatform.Extensions;
using PostSearchPlatform.Models;
using PostSearchPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace PostSearchPlatform.Controllers;

public class TrackingController : Controller
{
    private readonly IClicksService _clicksService;
    private readonly IDistributedCache _cache;

    public TrackingController(IClicksService clicksService, IDistributedCache cache)
    {
        _clicksService = clicksService;
        _cache = cache;
    }

    public async Task<IActionResult> Click(int id, string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            return BadRequest("Invalid URL");

        try
        {
            var click = new Click(id);
            var milliSeconds = (long)(click.Performed - new DateTime(1970, 1, 1)).TotalMilliseconds;
            await _cache.SetRecordAsync($"Click_{id}_{milliSeconds}", new Click(id));

            return Redirect(url);
        }
        catch (Exception)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while tracking the click");
        }
    }

    public async Task<IActionResult> ClickSaveToDB(int id, string url, CancellationToken cancellationToken)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            return BadRequest("Invalid URL");

        try
        {
            await _clicksService.RecordClick(new Click(id), cancellationToken);

            return Redirect(url);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the click to the database");
        }
    }
}
