using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectManagement.Data;
using ProyectManagement.Models;
using ProyectManagement.Models.SectionViewModels;

namespace ProyectManagement.Controllers
{
    [Authorize]
    public class SectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Section
        public async Task<IActionResult> Index(int? proyectID)
        {
            //var applicationDbContext = _context.Sections.Include(s => s.Proyect);
            var sections = from s in _context.Sections select s;
            if (proyectID == null)
            {
                return NotFound();
            }
            ViewData["currentProyect"] = proyectID;
            sections = sections.Where(s => s.ProyectId.Equals(proyectID));
            return View(await sections.ToListAsync());
        }

// GET: Section/Details/5
public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["currentSection"] = id;
            var section = await _context.Sections
                .Include(s => s.Proyect)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // GET: Section/Create
        public IActionResult Create(int? proyectID)
        {
            ViewData["currentProyect"] = proyectID;
            if (proyectID == null)
            {
                return BadRequest();
            }
            return View(new SectionCreateViewModel()
            {
                proyectId = (int)proyectID
            });
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SectionCreateViewModel section)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Section()
                {
                    ProyectId = section.proyectId,
                    Name = section.Name,
                    Descriptions = section.Description
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { ProyectId = section.proyectId });
            }
            //ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description", section.ProyectId);
            return View(section);
        }

        // GET: Section/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["currentSection"] = id;
            if (id == null)
            {
                return BadRequest();
            }
            var section = await _context.Sections.SingleOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }
            //ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description", section.ProyectId);
            return View(new SectionEditViewModel()
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Descriptions
            });
        }

        // POST: Section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SectionEditViewModel model)
        {
            if (id != model.Id) 
            {
                return NotFound();
            }
            var section = _context.Sections.FirstOrDefault(c => c.Id == model.Id);
            if(section == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                section.Name = model.Name;
                section.Descriptions = model.Description;
                try
                {
                    _context.Update(section);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(section.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new {section.Id});
            }
            //ViewData["ProyectId"] = new SelectList(_context.Proyects, "Id", "Description", section.ProyectId);
            return View(model);
        }

        // GET: Section/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var section = await _context.Sections
                .Include(s => s.Proyect)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var section = await _context.Sections.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {proyectID = section.ProyectId});
        }

        private bool SectionExists(int id)
        {
            return _context.Sections.Any(e => e.Id == id);
        }
    }
}
