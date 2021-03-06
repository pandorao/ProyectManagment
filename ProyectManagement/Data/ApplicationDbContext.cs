﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectManagement.Models;

namespace ProyectManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        } 

        public DbSet<Proyect> Proyects { get; set; } 
        public DbSet<Section> Sections { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Models.Job> Jobs { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

    }
}
