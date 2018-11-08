using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models
{
    public class Section
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ProyectId { get; set; } 
        public Proyect Proyect { get; set; } 

        public string Descriptions { get; set; }
    }
}
