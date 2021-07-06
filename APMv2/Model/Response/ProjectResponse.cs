using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Response
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserInfo> ListUser { get; set; }
        public int TotalMember { get; set; }
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
    }
}
