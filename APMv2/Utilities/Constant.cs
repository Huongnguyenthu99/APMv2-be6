using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APMv2.Utilities
{
    public class Constant
    {
    }
    public enum Status
    {
        ToDo = 0,
        Doing = 1,
        Done = 2,
    }

    public enum Role
    {
        Admin = 1,
        User = 0,
    }

    public enum Position
    {
        Dev = 1,
        Tester = 2,
        BA = 3,
        Designer = 4,
    }
}
