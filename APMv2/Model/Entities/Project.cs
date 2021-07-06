using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class Project
    {
        public Project()
        {
            ProjectUser = new HashSet<ProjectUser>();
            Sprint = new HashSet<Sprint>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ProjectUser> ProjectUser { get; set; }
        public ICollection<Sprint> Sprint { get; set; }
    }
}
