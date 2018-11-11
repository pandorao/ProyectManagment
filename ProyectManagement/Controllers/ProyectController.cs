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
using ProyectManagement.Models.ProyectViewModels;

namespace ProyectManagement.Controllers
{
    [Authorize]
    public class ProyectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ProyectController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Proyect
        public async Task<IActionResult> Index()
        {
            ViewData["currentUser"] = _userManager.GetUserId(HttpContext.User);//se extrae el id del usuario
            var proyects_currentuser = from p in _context.Proyects select p;//se seleccionas primero todos los proyectos del usuario en sesion
            proyects_currentuser = proyects_currentuser.Where(p => p.ApplicationUserId.Equals(ViewData["currentUser"]));//se obtienen los proyectos que pertenezcan al usuario
            return View(await proyects_currentuser.ToListAsync());
        }

        // GET: Proyect/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyect = await _context.Proyects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proyect == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return BadRequest();
            }

            var contributor = await _context.Contributors.FirstOrDefaultAsync(c => c.ProyectId == proyect.Id && c.ApplicationUserId == userId);
            if (contributor == null)
            {
                ViewBag.DelayedJobs = new List<Job>();
            }
            else
            {
                ViewBag.DelayedJobs = (from assignment in _context.Assignments.Where(a => a.ContributorId == contributor.Id)
                                       join job in _context.Jobs.Include(p => p.Section).Where(p => p.Section.ProyectId == proyect.Id && ((DateTime.Now > p.endDate && p.State == enumState.Active) || p.State == enumState.Delayed))
                                         on assignment.jobId equals job.Id
                                       select new Job()
                                       {
                                           endDate = job.endDate,
                                           Name = job.Name,
                                           Id = job.Id,
                                           sectionId = job.sectionId,
                                           startDate = job.startDate,
                                           State = job.State
                                       }).ToList();
            }

            return View(proyect);
        }

        // GET: Proyect/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proyect/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProyectCreateViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var proyect = new Proyect()
                {
                    Description = model.Description,
                    Name = model.Name,
                    Status = EnumProyectManagment.InProcess,
                    ApplicationUserId = userId,
                };
                _context.Add(proyect);
                _context.Add(new Contributor()
                {
                    Proyect = proyect,
                    SectionId = null,
                    ApplicationUserId = userId
                });
                

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
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
            return View(new ProyectEditViewModel()
            {
                Id = proyect.Id,
                Name = proyect.Name,
                Description = proyect.Description,
                Status = proyect.Status
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProyectEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var proyect = await _context.Proyects.FindAsync(model.Id);
                if (proyect == null)
                {
                    return NotFound();
                }

                proyect.Name = model.Name;
                proyect.Description = model.Description;
                proyect.Status = model.Status;

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
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            return View(model);
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
