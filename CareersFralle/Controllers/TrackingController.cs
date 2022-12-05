using CareersFralle.Models;
using CareersFralle.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareersFralle.Controllers
{
    public class TrackingController : Controller
    {
        private readonly IClicksService _clicksService;

        public TrackingController(IClicksService clicksService)
        {
            _clicksService = clicksService;
        }

        public async Task<ActionResult> Click(int id, string url)
        {
            Uri address = new Uri(Request.Host.ToString());
            await _clicksService.RecordClick(new Click(id));

            return Redirect(url);
        }
    }
}
