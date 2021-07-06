using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class SprintTarget
    {
        public int Id { get; set; }
        public int SprintId { get; set; }
        public string Name { get; set; }

        public Sprint Sprint { get; set; }
    }
}
