using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int jobId { get; set; }
        public Job job { get; set; }
        public int ContributorId { get; set; }
        public Contributor contributor { get; set; }
    }
}
