using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models.ContributorViewModels
{
    public class ContributorEditViewModel
    {
        public Contributor Contributor { get; set; }

        public int Id { get; set; } 

        [Display(Name = "Section")]
        public int? SectionId { get; set; }
    }
}
