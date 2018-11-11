using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Component
{
    public class ContributorsSidebar : ViewComponent
    {
        private readonly ApplicationDbContext _context;  

        public ContributorsSidebar(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int proyectId)
        {
            var contributors = await _context.Contributors
                .Include(c => c.Section)
                .Include(c => c.ApplicationUser)
                .Where(p => p.ProyectId == proyectId).ToListAsync();
            return View(contributors);
        }
    }
}
