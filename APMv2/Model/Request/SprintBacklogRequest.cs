using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Request
{
    public class SprintBacklogRequest
    {
        public int Id { get; set; }
        public int SprintId { get; set; }
        public int BacklogId { get; set; }
        public string Name { get; set; }
        public int? PercentageRemain { get; set; }
        public int Status { get; set; }
        public string Priority { get; set; }
    }
}
