﻿using System;
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
    public class ContributorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContributorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contributor
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contributors.Include(c => c.ApplicationUser).Include(c => c.Proyect).Include(c => c.Section);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contributor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributors
                .Include(c => c.ApplicationUser)
                .Include(c => c.Proyect)
                .Include(c => c.Section)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (contributor == null)
            {
                return NotFound();
            }

            return View(contributor);
        }

        // GET: Contributor/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description");
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name");
            return View();
        }

        // POST: Contributor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SectionId,ProyectId,ApplicationUserId")] Contributor contributor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", contributor.ApplicationUserId);
            ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description", contributor.ProyectId);
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", contributor.SectionId);
            return View(contributor);
        }

        // GET: Contributor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributors.SingleOrDefaultAsync(m => m.Id == id);
            if (contributor == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", contributor.ApplicationUserId);
            ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description", contributor.ProyectId);
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", contributor.SectionId);
            return View(contributor);
        }

        // POST: Contributor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectionId,ProyectId,ApplicationUserId")] Contributor contributor)
        {
            if (id != contributor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contributor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributorExists(contributor.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", contributor.ApplicationUserId);
            ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description", contributor.ProyectId);
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", contributor.SectionId);
            return View(contributor);
        }

        // GET: Contributor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributors
                .Include(c => c.ApplicationUser)
                .Include(c => c.Proyect)
                .Include(c => c.Section)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (contributor == null)
            {
                return NotFound();
            }

            return View(contributor);
        }

        // POST: Contributor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contributor = await _context.Contributors.SingleOrDefaultAsync(m => m.Id == id);
            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributorExists(int id)
        {
            return _context.Contributors.Any(e => e.Id == id);
        }
    }
}