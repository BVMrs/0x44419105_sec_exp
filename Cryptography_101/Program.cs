using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;

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

            BigInteger msg = BigInteger.Parse("073811323030852810356417874183354216169584614857118668375147489469322004843753734209928490797218876714840256327583099651405111360830110407144251284000440454981221826378677397481795725070541305544143080730980746486247130845531203593818661954723401747925869225128853779664478075510760374717399594736545286400401438846788361630982143501430088351118798669627464369854565436463895609031040153599370588932714210618301835725694963578335784950701607560478773768552356843444860281849431500598470327221412697586010556336496776518572414366837510728849157764353245763234");
            BigInteger cry = 0;

            cry = alice.encrypt(msg);
            Console.WriteLine("The message is: " + msg);
            Console.WriteLine("The encrypted message is: " + cry);
            Console.WriteLine("The decrypted message is: " + alice.decrypt(cry));
            Console.WriteLine("The process is:" + (msg == alice.decrypt(cry)));
            Console.WriteLine("");
        }
    }
}
