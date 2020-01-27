using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AVI.Helpers;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoldenLadle.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var events = (List<Event>)await _unitOfWork.Events.GetAllCurrent();
            for (int i = 0; i < events.Count; i++)
            {
                events[i].StartDT = TimeZoneHelper.ConvertTimeToLocal(events[i].StartDT, "America/New_York");
                events[i].EndDT = TimeZoneHelper.ConvertTimeToLocal(events[i].EndDT, "America/New_York");
            }
            return View(events);
        }

        [HttpGet("/PastEvents")]
        public async Task<IActionResult> PastEvents()
        {
            var events = await _unitOfWork.Events.GetAllPast();
            return View(events);
        }

        public IActionResult About()
        {
            return Forbid();
        }

        public IActionResult Contact()
        {
            return Forbid();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}