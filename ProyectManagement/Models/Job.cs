using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime startDate { get; set; }
        [Required]
        public DateTime endDate { get; set; }
        public enumState State { get; set; }
        public int sectionId { get; set; }
        public Section Section { get; set; } 
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
    public enum enumState
    {
        Active,
        Finalized,
        Closed,
        Delayed
    }
}
