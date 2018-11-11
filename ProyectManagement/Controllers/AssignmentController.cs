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
using ProyectManagement.Models.AssignmentViewModels;

namespace ProyectManagement.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Assignment
        public async Task<IActionResult> AssignedJob(int proyectID) 
        {
            ViewData["CurrentProyect"] = proyectID;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return BadRequest();
            }

            var contributor = await _context.Contributors.FirstOrDefaultAsync(c => c.ProyectId == proyectID && c.ApplicationUserId == userId);
            if (contributor == null)
            {
                return View(new List<Job>());
            }

            var applicationDbContext = from assignment in _context.Assignments.Where(a => a.ContributorId == contributor.Id)
                                       join job in _context.Jobs.Include(p => p.Section).Where(p => p.Section.ProyectId == proyectID)
                                       on assignment.jobId equals job.Id
                                       select new Job()
                                       {
                                           endDate = job.endDate,
                                           Name = job.Name,
                                           Id = job.Id,
                                           sectionId = job.sectionId,
                                           startDate = job.startDate,
                                           State = job.State
                                       };

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Assignment/Create
        public IActionResult Create(int jobId, int proyectID)
        {
            ViewData["MsgError"] = TempData["MsgError"];
            var users = _context.Contributors
                .Include(p => p.ApplicationUser)
                .Where(c => c.ProyectId == proyectID)
                .Select(c => new AssignmentCreateViewModel()
                {
                    UserName = c.ApplicationUser.UserName,
                    ContributorId = c.Id
                });

            ViewData["ContributorId"] = new SelectList(users, "ContributorId", "UserName");
            ViewData["jobId"] = jobId;
            ViewData["CurrentProyect"] = proyectID;
            ViewBag.ListUserAssign = _context.Assignments
                .Include(c => c.contributor)
                    .ThenInclude(c => c.ApplicationUser)
                .Where(a => a.jobId == jobId).ToList();
            return View(new Assignment()
            {
                jobId = jobId
            });
        }

        // POST: Assignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int jobId, int proyectID,  Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                if (_context.Assignments.Any(a => a.ContributorId == assignment.ContributorId && a.jobId == assignment.jobId))
                {
                    TempData["MsgError"] = "Error! the job was already assigned";
                }
                else
                {
                    await _context.SaveChangesAsync();
                }
            } 
            return RedirectToAction(nameof(Create), new { proyectID, jobId });
        }

        // POST: Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignments.Include(a => a.contributor).FirstOrDefaultAsync(m => m.Id == id);
            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            TempData["MsgError"] = "Successful! the contributor was deleted";
            return RedirectToAction(nameof(Create), new { proyectId = assignment.contributor.ProyectId, assignment.jobId });
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.Id == id);
        }
    }
}
