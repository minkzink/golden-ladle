using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;

namespace GoldenLadle.Controllers
{

    public class EventsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment env;
        private string _path = "";

        public EventsController(IUnitOfWork unitOfWork, IHostingEnvironment env)
        {
            this.env = env;
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
            try
            {
                ViewBag.LoggedInUser = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            catch
            {
                ViewBag.LoggedInUser = "No logged in User";
            }

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
                    FilePath image = await UploadImage(@event, upload);

                    DeleteTempFile(_path);
                    @event.FilePaths.Add(image);
                }
                await _unitOfWork.Events.AddAsync(@event);
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
            return @event == null ? NotFound() : (IActionResult)View(@event);
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
                    FilePath image = await UploadImage(@event, upload);

                    DeleteTempFile(_path);
                    @event.FilePaths.Add(image);
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

        private void DeleteTempFile(string path)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
            }   
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

        private async Task<FilePath> UploadImage(Event @event, IFormFile upload)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                var image = new FilePath()
                {
                    FileName = Guid.NewGuid().ToString() + Path.GetFileNameWithoutExtension(upload.FileName),
                    FileType = FileType.Header,
                    FileExt = Path.GetExtension(upload.FileName)
                };
                @event.FilePaths = new List<FilePath>();

                _path = env.WebRootFileProvider.GetFileInfo(image.FileName).PhysicalPath;
                using (var stream = new FileStream(_path, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }
                Dictionary<FileType, string> imageUrls = UploadImageToCloudinary(image);
                image.FileName = imageUrls.FirstOrDefault(i => i.Key == FileType.Header).Value;
                image.ThumbName = imageUrls.FirstOrDefault(i => i.Key == FileType.Thumbnail).Value;
                return image;
            }
            else
            {
                return new FilePath();
            }
        }

        private Dictionary<FileType, string> UploadImageToCloudinary(FilePath image)
        {
            Dictionary<FileType, string> paths = new Dictionary<FileType, string>();
            Account account = new Account(
                "dc1tgasv1",
                "694124985656512",
                "RtY2gs7YjWDe51V86p80oloS9gc");

            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription($"wwwroot/{image.FileName}"),
                PublicId = image.FileName,
                Transformation = new Transformation().Width(2000).Crop("scale")
            };
            ImageUploadResult result = cloudinary.Upload(uploadParams);
            paths.Add(FileType.Header, cloudinary.Api.UrlImgUp.Secure(true)
                  .Transform(new Transformation().Width(1960).Crop("scale"))
                      .BuildUrl(image.FileName));
            paths.Add(FileType.Thumbnail, cloudinary.Api.UrlImgUp.Secure(true)
                  .Transform(new Transformation().Width(420).Crop("scale"))
                      .BuildUrl(image.FileName));

            return paths;
        }
    }
}
