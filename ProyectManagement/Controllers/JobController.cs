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
using ProyectManagement.Models.JobViewModels;

namespace ProyectManagement.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }
		
        // GET: Job
        public async Task<IActionResult> Index(int? proyectID)
        {
			if(proyectID == null)
			{
				return NotFound();
			}
			ViewData["CurrentProyect"] = proyectID;
            var applicationDbContext = _context.Jobs.Include(j => j.Section).Where(c => c.Section.ProyectId == proyectID);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Job/Create
        public IActionResult Create(int proyectID)
        {
            ViewData["CurrentProyect"] = proyectID;
            ViewData["sectionId"] = new SelectList(_context.Sections.Where(s => s.ProyectId == proyectID), "Id", "Name");
            return View();
        }

        // POST: Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job, int proyectID)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { proyectID });
            }

            ViewData["CurrentProyect"] = proyectID;
            ViewData["sectionId"] = new SelectList(_context.Sections.Where(s => s.ProyectId == proyectID), "Id", "Name", job.sectionId);
            return View(job);
        }

        // GET: Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.Include(j => j.Section).FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["CurrentProyect"] = job.Section.ProyectId;
            ViewData["sectionId"] = new SelectList(_context.Sections.Where(s => s.ProyectId == job.Section.ProyectId), "Id", "Name", job.sectionId);
            return View(job);
        }

        // POST: Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job, int proyectID)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { proyectID });
            }
            ViewData["CurrentProyect"] = proyectID;
            ViewData["sectionId"] = new SelectList(_context.Sections.Where(s => s.ProyectId == proyectID), "Id", "Name", job.sectionId);
            return View(job);
        }

        // GET: Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Section)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["CurrentProyect"] = job.Section.ProyectId;
            return View(job);
        }

        // GET: Job/Planner/5
        public IActionResult Planner(int? proyectId)
        {
            ViewData["CurrentProyect"] = proyectId;
            return View();
        }

        // GET: Job/Planner/5
        public IActionResult PlannerEvent()
        {
            var list = new List<PlannerEventViewModel>()
            {
                new PlannerEventViewModel(){
                    title = "job 1",
                    start = new DateTime(2018,11,17),
                    end = new DateTime(2018,11,16)
                },
                new PlannerEventViewModel(){
                    title = "job 2",
                    start = new DateTime(2018,11,12),
                    end = new DateTime(2018,11,13)
                },
                new PlannerEventViewModel(){
                    title = "job 3",
                    start = new DateTime(2018,11,20),
                    end = new DateTime(2018,11,23)
                }
            };
            return Json(list);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Section)
                .Include(j => j.Assignments)
                .FirstOrDefaultAsync(m => m.Id == id);
            _context.Jobs.Remove(job);
            _context.Assignments.RemoveRange(job.Assignments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { job.Section.ProyectId });
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
