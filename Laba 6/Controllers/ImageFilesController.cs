using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba_6.Data;
using Laba_6.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Laba_6.ViewModels;

namespace Laba_6.Controllers
{
    public class ImageFilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageFilesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: ImageFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageFile = await _context.ImageFiles
                .Include(i => i.Folder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageFile == null)
            {
                return NotFound();
            }

            var model = new ImageFileViewModel(imageFile, _context);

            return View(model);
        }

        // POST: ImageFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile image, int folderId)
        {
            if (image != null && image.ContentType.StartsWith("image"))
            {
                // путь к папке Files
                string path = "/Files/" + image.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_hostEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                ImageFile file = new ImageFile { Name = image.FileName, Path = path, FolderId = folderId };
                await _context.ImageFiles.AddAsync(file);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(FoldersController.Details), new { id = folderId, Controller = "Folders" });
        }

        // GET: ImageFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageFile = await _context.ImageFiles
                .Include(i => i.Folder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageFile == null)
            {
                return NotFound();
            }

            return View(imageFile);
        }

        // POST: ImageFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageFile = await _context.ImageFiles.FindAsync(id);
            _context.ImageFiles.Remove(imageFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FoldersController.Details), new { id = imageFile.FolderId, Controller = "Folders"});
        }
    }
}
