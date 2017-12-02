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
            for (int i = 0; i < 20; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                RSA alice = new Cryptography_101.RSA();
                sw.Stop();
                Console.WriteLine("Time elapsed creating a new RSA instance on iteration {0} is: {1}.", i, sw.Elapsed);
            }

            Console.WriteLine("---------- Test session done ----------");
        }
    }
}
