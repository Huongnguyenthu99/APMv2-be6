using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class SprintBacklog
    {
        public SprintBacklog()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public int SprintId { get; set; }
        public int BacklogId { get; set; }
        public string Name { get; set; }
        public int? PercentageRemain { get; set; }
        public int Status { get; set; }
        public string Priority { get; set; }

        public Backlog Backlog { get; set; }
        public Sprint Sprint { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
    }
}
