using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class TimeWorkingOff
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SprintId { get; set; }
        public string DayOff { get; set; }
        public double TotalDayOff { get; set; }

        public Sprint Sprint { get; set; }
        public User User { get; set; }
    }
}
