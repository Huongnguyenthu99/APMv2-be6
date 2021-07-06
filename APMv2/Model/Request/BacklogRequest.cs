using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Request
{
    public class BacklogRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Category { get; set; }
    }
}
