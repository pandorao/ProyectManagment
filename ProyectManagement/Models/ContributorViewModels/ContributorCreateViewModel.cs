using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models.ContributorViewModels
{
    public class ContributorCreateViewModel
    {
        [Display(Name = "User Name")]
        [EmailAddress]
        public string UserName { get; set; } 

        public int ProyectId { get; set; } 

        public int? SectionId { get; set; }
    }
}
