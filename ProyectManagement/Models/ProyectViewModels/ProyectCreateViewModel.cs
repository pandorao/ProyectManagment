using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models.ProyectViewModels
{
    public class ProyectCreateViewModel
    {
        [Required] 
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
