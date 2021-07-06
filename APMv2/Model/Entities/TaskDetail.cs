using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class TaskDetail
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public DateTime? Date { get; set; }
        public int? RemainTime { get; set; }

        public Tasks Task { get; set; }
    }
}
