using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectManagement.Models.JobViewModels
{
    public class PlannerEventViewModel
    {
        public string title { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }
}
