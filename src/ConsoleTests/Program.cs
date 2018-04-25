using System;
using Lazy.Utilities.Extensions;
namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "   0gf1";
            Console.WriteLine(s.TryPrase<int>());
        }
    }
}
