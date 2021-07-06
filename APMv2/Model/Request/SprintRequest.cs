using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Request
{
    public class SprintRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int WorkingDay { get; set; }
    }
}
