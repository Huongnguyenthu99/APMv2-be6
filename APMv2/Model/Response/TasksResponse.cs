using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Response
{
    public class TasksResponse
    {
        public int Id { get; set; }
        public int SprintBacklogId { get; set; }
        public int? UserId { get; set; }//FullName
        public string Name { get; set; }
        public string FullName { get; set; }
        public int Status { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int? EstimatedTime { get; set; }
    }
}
