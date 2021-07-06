using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class Tasks
    {
        public Tasks()
        {
            TaskDetail = new HashSet<TaskDetail>();
        }

        public int Id { get; set; }
        public int SprintBacklogId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int? EstimatedTime { get; set; }

        public SprintBacklog SprintBacklog { get; set; }
        public User User { get; set; }
        public ICollection<TaskDetail> TaskDetail { get; set; }
    }
}
