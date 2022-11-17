using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApp2.Data;
using MyWebApp2.Models;

namespace MyWebApp2.Controllers
{
    public class ScBoardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScBoardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScBoards
        public async Task<IActionResult> Index()
        {
              return View(await _context.ScBoard.ToListAsync());
        }

        // GET: ScBoards/Index2
        public async Task<IActionResult> Index2(int? StackId)
        {
            string usr = User.Identity.Name;
            ViewData["Vocstack"] = await _context.Vocstack.ToListAsync(); //list all vocstack name

            if (usr == null)
            {
                usr = "Guest";
            }
            if(StackId != null) //特定stack
            {
                return View(await _context.ScBoard.Where(i => i.Account == usr && i.StackId == StackId).ToListAsync());
            }

            return View(await _context.ScBoard.Where(i => i.Account == usr).ToListAsync());
        }

        // GET: ScBoards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScBoard == null)
            {
                return NotFound();
            }

            var scBoard = await _context.ScBoard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scBoard == null)
            {
                return NotFound();
            }

            return View(scBoard);
        }

        // GET: ScBoards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScBoards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,StackId,Account")] ScBoard scBoard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scBoard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scBoard);
        }

        // GET: ScBoards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScBoard == null)
            {
                return NotFound();
            }

            var scBoard = await _context.ScBoard.FindAsync(id);
            if (scBoard == null)
            {
                return NotFound();
            }
            return View(scBoard);
        }

        // POST: ScBoards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,StackId,Account")] ScBoard scBoard)
        {
            if (id != scBoard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scBoard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScBoardExists(scBoard.Id))
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
            return View(scBoard);
        }

        // GET: ScBoards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScBoard == null)
            {
                return NotFound();
            }

            var scBoard = await _context.ScBoard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scBoard == null)
            {
                return NotFound();
            }

            return View(scBoard);
        }

        // POST: ScBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScBoard == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ScBoard'  is null.");
            }
            var scBoard = await _context.ScBoard.FindAsync(id);
            if (scBoard != null)
            {
                _context.ScBoard.Remove(scBoard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScBoardExists(int id)
        {
          return _context.ScBoard.Any(e => e.Id == id);
        }
    }
}
