using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Request
{
    public class TimeWorkingOffRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SprintId { get; set; }
        public string DayOff { get; set; }
        public double TotalDayOff { get; set; }
    }
}
