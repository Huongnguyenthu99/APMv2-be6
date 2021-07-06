using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Request
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public int SprintBacklogId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int? EstimatedTime { get; set; }
    }
}
