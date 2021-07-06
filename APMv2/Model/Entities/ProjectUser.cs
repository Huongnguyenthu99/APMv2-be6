using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class ProjectUser
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }
    }
}
