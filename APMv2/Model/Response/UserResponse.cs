using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int? Role { get; set; }
        public int? Position { get; set; }
        public string PositionText { get; set; }
        public string Dob { get; set; }
        public int? Gender { get; set; }
        public List<string> ListProjectName { get; set; }
    }
}
