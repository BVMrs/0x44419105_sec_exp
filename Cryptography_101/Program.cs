using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Cryptography_101
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------- Commencing test session ----------");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            RSA alice = new Cryptography_101.RSA();
            sw.Stop();
            Console.WriteLine("Time elapsed creating a new RSA instance is: {0}.", sw.Elapsed);

            Console.WriteLine("---------- Test session done ----------");
        }
    }
}
