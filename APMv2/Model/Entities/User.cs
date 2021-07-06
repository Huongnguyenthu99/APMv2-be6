using System;
using System.Collections.Generic;

namespace APMv2.Model.Entities
{
    public partial class User
    {
        public User()
        {
            Backlog = new HashSet<Backlog>();
            ProjectUser = new HashSet<ProjectUser>();
            Tasks = new HashSet<Tasks>();
            TimeWorkingOff = new HashSet<TimeWorkingOff>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }
        public int? Position { get; set; }
        public string Dob { get; set; }
        public int? Gender { get; set; }

        public ICollection<Backlog> Backlog { get; set; }
        public ICollection<ProjectUser> ProjectUser { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
        public ICollection<TimeWorkingOff> TimeWorkingOff { get; set; }
    }
}
