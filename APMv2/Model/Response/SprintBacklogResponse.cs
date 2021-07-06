using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Response
{
    public class SprintBacklogResponse
    {
        public int Id { get; set; }
        public int SprintId { get; set; }
        public int BacklogId { get; set; }
        public string BacklogName { get; set; }
        public string Name { get; set; }
        public int? PercentageRemain { get; set; }
        public int Status { get; set; }
        public string Priority { get; set; }
        public List<TasksResponse> ListTasks { get; set; }
        public int TotalTask { get; set; }
        public int DoneTask { get; set; }
    }
}
