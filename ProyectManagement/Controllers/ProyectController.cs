using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectManagement.Data;
using ProyectManagement.Models;

namespace ProyectManagement.Controllers
{
    public class ProyectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProyectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Proyect
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proyects.ToListAsync());
        }

        // GET: Proyect/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyect = await _context.Proyects
                .SingleOrDefaultAsync(m => m.Id == id);
            if (proyect == null)
            {
                return NotFound();
            }

            return View(proyect);
        }

        // GET: Proyect/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proyect/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Status")] Proyect proyect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proyect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proyect);
        }

        // GET: Proyect/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyect = await _context.Proyects.SingleOrDefaultAsync(m => m.Id == id);
            if (proyect == null)
            {
                return NotFound();
            }
            return View(proyect);
        }

        // POST: Proyect/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Status")] Proyect proyect)
        {
            if (id != proyect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proyect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProyectExists(proyect.Id))
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
            return View(proyect);
        }

        // GET: Proyect/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyect = await _context.Proyects
                .SingleOrDefaultAsync(m => m.Id == id);
            if (proyect == null)
            {
                return NotFound();
            }

            return View(proyect);
        }

        // POST: Proyect/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proyect = await _context.Proyects.SingleOrDefaultAsync(m => m.Id == id);
            _context.Proyects.Remove(proyect);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProyectExists(int id)
        {
            return _context.Proyects.Any(e => e.Id == id);
        }
    }
}
