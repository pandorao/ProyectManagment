using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectManagement.Data;
using ProyectManagement.Models;
using ProyectManagement.Models.ContributorViewModels;

namespace ProyectManagement.Controllers
{
    [Authorize]
    public class ContributorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContributorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contributor
        public IActionResult Index(int? proyectId)
        {
            if (proyectId == null)
            {
                return NotFound();
            } 

            var contributors = _context.Contributors
                .Include(c => c.Section)
                .Include(c => c.ApplicationUser)
                .Where(p => p.ProyectId == proyectId);

            ViewData["currentProyect"] = proyectId;

            return View(contributors);
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
        public IActionResult Create(int? proyectId)
        {
            if (proyectId == null)
            {
                return NotFound();
            }
            ViewData["currentProyect"] = proyectId;
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name");

            return View(new ContributorCreateViewModel()
            {
                ProyectId = (int) proyectId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int proyectId, ContributorCreateViewModel model)
        {
            ViewData["currentProyect"] = proyectId;
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null) 
                {
                    ModelState.AddModelError("UserName", "User not found");
                    return View(model);
                } 

                if (_context.Contributors.Any(c => c.ApplicationUserId == user.Id && c.ProyectId == proyectId))
                {
                    ModelState.AddModelError("UserName", "User is already contributor");
                    return View(model);
                }

                _context.Add(new Contributor
                {
                    ApplicationUserId = user.Id,
                    ProyectId = model.ProyectId,
                    SectionId = model.SectionId
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),  new { proyectId });
            }

            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", model.SectionId);
            return View(model);
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
