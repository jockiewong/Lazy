using System;
using Lazy.Utilities.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "   0gf1";
            Console.WriteLine(s.TryPrase<int>());

            IQueryable<int> i = null;
            IEnumerable<int> i1 = null;
            i.WhereIf(true, null);
            i1.WhereIf(true, null);
        }
    }
}
