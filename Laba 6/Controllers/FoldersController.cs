using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba_6.Data;
using Laba_6.Models;
using Laba_6.ViewModels;

namespace Laba_6.Controllers
{
    public class FoldersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoldersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Folders/Details/5
        public async Task<IActionResult> Details(int id = 1)
        {
            var folder = await _context.Folders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (folder == null)
            {
                folder = await _context.Folders
                .FirstOrDefaultAsync(m => m.Id == 1);
            }

            var model = new FolderViewModel(folder, _context);

            return View(model);
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentId")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(folder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FoldersController.Details), new { id = folder.Id});
            }

            return RedirectToAction(nameof(FoldersController.Details), new { id = folder.ParentId });
        }

        // GET: Folders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 1)
            {
                return NotFound();
            }

            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentId")] Folder folder)
        {
            if (id != folder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderExists(folder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = folder.Id});
            }
            return View(folder);
        }

        // GET: Folders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 1)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .Include(f => f.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == 1)
            {
                return NotFound();
            }

            var folder = await _context.Folders.FindAsync(id);
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = folder.ParentId });
        }

        private bool FolderExists(int id)
        {
            return _context.Folders.Any(e => e.Id == id);
        }
    }
}
