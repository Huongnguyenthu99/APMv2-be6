using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class Sprint
    {
        public Sprint()
        {
            SprintBacklog = new HashSet<SprintBacklog>();
            SprintTarget = new HashSet<SprintTarget>();
            TimeWorkingOff = new HashSet<TimeWorkingOff>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int WorkingDay { get; set; }

        public Project Project { get; set; }
        public ICollection<SprintBacklog> SprintBacklog { get; set; }
        public ICollection<SprintTarget> SprintTarget { get; set; }
        public ICollection<TimeWorkingOff> TimeWorkingOff { get; set; }
    }
}
