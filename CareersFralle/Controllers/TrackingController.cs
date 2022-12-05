using CareersFralle.Extensions;
using CareersFralle.Models;
using CareersFralle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CareersFralle.Controllers
{
    public class TrackingController : Controller
    {
        private readonly IClicksService _clicksService;
        private readonly IDistributedCache _cache;

        public TrackingController(IClicksService clicksService, IDistributedCache cache)
        {
            _clicksService = clicksService;
            _cache = cache;
        }

        public async Task<ActionResult> Click(int id, string url)
        {
            var click = new Click(id);
            var milliSeconds = (long)(click.Performed - new DateTime(1970, 1, 1)).TotalMilliseconds;
            await _cache.SetRecordAsync($"Click_{id}_{milliSeconds}", new Click(id));

            return Redirect(url);
        }

        public async Task<ActionResult> ClickSaveToDB(int id, string url)
        {
            await _clicksService.RecordClick(new Click(id));

            return Redirect(url);
        }
    }
}
