using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Component
{
    public class SectionsSidebar : ViewComponent
    {
        private readonly ApplicationDbContext _context;  

        public SectionsSidebar(ApplicationDbContext context) 
        {
            _context = context;
        } 

        public async Task<IViewComponentResult> InvokeAsync(int proyectId)
        {
            var contributors = await _context.Sections
                .Where(p => p.ProyectId == proyectId).ToListAsync();
            return View(contributors);
        }
    }
}
