using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoldenLadle.Data;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;

namespace GoldenLadle.Controllers
{
    
    public class EventsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Events.GetAllAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _unitOfWork.Events.GetAsync(id);
            ViewBag.LoggedInUser = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        [Authorize(Roles = "Admin")]
        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDT,EndDT")] Event @event, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    var image = new FilePath()
                    {
                        FileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(upload.FileName),
                        FileType = FileType.Header,
                    };
                    @event.FilePaths = new List<FilePath>();
                    @event.FilePaths.Add(image);
                    //Save the file to the images folder
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", image.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                    }
                }
                _unitOfWork.Events.AddAsync(@event);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _unitOfWork.Events.GetAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDT,EndDT")] Event @event, IFormFile upload)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    var image = new FilePath()
                    {
                        FileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(upload.FileName),
                        FileType = FileType.Header,
                    };
                    @event.FilePaths = new List<FilePath>();
                    @event.FilePaths.Add(image);
                    //Save the file to the images folder
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", image.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                    }
                }
                try
                {
                    _unitOfWork.Events.Update(@event);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _unitOfWork.Events.GetAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _unitOfWork.Events.GetAsync(id);
            _unitOfWork.Events.Remove(@event);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _unitOfWork.Events.CheckIfAnyExist(id);
        }
    }
}
