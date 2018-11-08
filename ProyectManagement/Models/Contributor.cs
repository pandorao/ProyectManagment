using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models
{
    public class Contributor
    { 
        public int Id { get; set; } 

        public int? SectionId { get; set; } 
        public Section Section { get; set; }

        public int ProyectId { get; set; }
        public Proyect Proyect { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    } 
}
