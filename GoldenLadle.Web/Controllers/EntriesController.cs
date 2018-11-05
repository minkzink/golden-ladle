using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoldenLadle.Data;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;
using Microsoft.AspNetCore.Authorization;

namespace GoldenLadle.Controllers
{
    public class EntriesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public EntriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        // GET: Entries
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Entries.GetAllAsync());
        }

        [Authorize(Roles = "Admin, NormalUser")]
        // GET: Entries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _unitOfWork.Entries.GetAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        [Authorize(Roles = "Admin, NormalUser")]
        // GET: Entries/Create
        public IActionResult Create(int eventId)
        {
            var entry = new Entry()
            {
                EventId = eventId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            return View(entry);
        }

        // POST: Entries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,EventId,UserId")] Entry entry)
        {
            int? id = entry.EventId;
            if (ModelState.IsValid)
            {
                _unitOfWork.Entries.AddAsync(entry);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Details", "Events", new { id = entry.EventId });
            }
            return RedirectToAction("Details", "Events", new { id = entry.EventId });
        }

        [Authorize(Roles = "Admin, NormalUser")]
        // GET: Entries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _unitOfWork.Entries.GetAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return View(entry);
        }

        // POST: Entries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,EventId,UserId")] Entry entry)
        {
            if (id != entry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entryToUpdate = _unitOfWork.Entries.Get(entry.Id);
                entryToUpdate.Name = entry.Name;
                entryToUpdate.Description = entry.Description;
                entryToUpdate.ModifiedDate = DateTime.Now;
                try
                {
                    _unitOfWork.Entries.Update(entryToUpdate);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Events", new { id = entry.EventId });
            }
            return View(entry);
        }

        [Authorize(Roles = "Admin, NormalUser")]
        // GET: Entries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _unitOfWork.Entries.GetAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _unitOfWork.Entries.GetAsync(id);
            _unitOfWork.Entries.Remove(entry);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Details", "Events", new { id = entry.EventId });
        }

        private bool EntryExists(int id)
        {
            return _unitOfWork.Entries.CheckIfAnyExist(id);
        }
    }
}
