using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class Backlog
    {
        public Backlog()
        {
            SprintBacklog = new HashSet<SprintBacklog>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? Category { get; set; }
        public int? Module { get; set; }
        public int PercentageRemain { get; set; }

        public User User { get; set; }
        public ICollection<SprintBacklog> SprintBacklog { get; set; }
    }
}
