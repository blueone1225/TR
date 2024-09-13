using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;
using NuGet.Packaging.Signing;

namespace MyModel_CodeFirst.Controllers
{
    public class BooksController : Controller
    {
        private readonly GuestBookContext _context;

        public BooksController(GuestBookContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            // 依照 TimeStamp 由新到舊排序
            return View(await _context.Book.OrderByDescending(b => b.TimeStamp).ThenByDescending(b => b.GId).ToListAsync());
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.GId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("GId,Title,Description,Author,TimeStamp,Photo,ImageType")] Book book)
        {
            if (id != book.GId)
            {
                return NotFound();
            }

            book.TimeStamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.GId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Books/DeleteReBook/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReBook(long id)
        {
            var reBook = await _context.ReBook.FindAsync(id);

            if (reBook != null)
            {
                _context.ReBook.Remove(reBook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Delete", new { id = reBook.GId });
            }

            return NotFound();
        }

        // Check if a book exists
        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.GId == id);
        }

        // GET: Books/GetImage/5
        public async Task<FileContentResult> GetImage(long gid)
        {
            var book = await _context.Book.FindAsync(gid);

            if (book == null || book.Photo == null)
            {
                // Return a default image if the book or photo is not found
                return File(new byte[0], "image/jpeg"); // You might want to return a default image here
            }

            return File(book.Photo, book.ImageType);
        }
    }
}
