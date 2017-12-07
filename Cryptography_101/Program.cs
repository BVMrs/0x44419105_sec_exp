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
            BigInteger msg = BigInteger.Parse("07381132303085281035641787418335421616958461");
            BigInteger cry = 0;
            String stringMsg = "0" + MathHelper.GetHashString(msg.ToString()).ToLower();
            Console.WriteLine("Digest is :" + BigInteger.Parse(stringMsg, System.Globalization.NumberStyles.AllowHexSpecifier));

            Console.WriteLine("---------- Commencing test session ----------");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            RSA alice = new Cryptography_101.RSA();
            sw.Stop();
            Console.WriteLine("Time elapsed creating a new RSA instance is: {0}.", sw.Elapsed);
            Console.WriteLine("---------- Test session done ----------");
            
            cry = alice.encrypt(msg);

            Console.WriteLine("The message is: " + msg);
            Console.WriteLine("The encrypted message is: " + cry);
            Console.WriteLine("The decrypted message is: " + alice.decrypt(cry));
            Console.WriteLine("The process is: " + (msg == alice.decrypt(cry)));
            Console.WriteLine("Public Key size is: " + alice.PubKey.N);
            Console.WriteLine("");

            BigInteger digest = alice.sign(BigInteger
                .Parse("0" + MathHelper.GetHashString(msg.ToString()), 
                System.Globalization.NumberStyles.AllowHexSpecifier));

            Console.WriteLine("Message is: " + msg);
            Console.WriteLine("Message digest is: " + MathHelper.GetHashString(msg.ToString()));


            bool verify = alice.verify_sign(digest, cry, alice.PubKey.E, alice.PriKey.D, alice.PubKey.N, msg);

            Console.WriteLine("Signature is: " + digest);
            Console.WriteLine("Signature check is: " + verify);
            Console.WriteLine("");
        }
    }
}