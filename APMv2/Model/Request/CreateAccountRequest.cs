using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Model.Request
{
    public class CreateAccountRequest
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int Position { get; set; }
    }
}
