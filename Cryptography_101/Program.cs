using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;
using System.IO;

namespace Cryptography_101
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger msg = BigInteger.Parse("7381132303085281035641787418335421616958461");
            BigInteger cry = 0;
            String stringMsg = MathHelper.GetHashString(msg.ToString());
            Console.WriteLine("Message to String: " + msg.ToString());
            Console.WriteLine("The hash of the message is:" + stringMsg);

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
            Console.WriteLine("");

            String message_digest = MathHelper.GetHashString(msg.ToString());

            Console.WriteLine("Message is: " + msg);
            Console.WriteLine("Message digest is: " + message_digest);
            Console.WriteLine("Message digest is toString : " + "0" + message_digest);

            BigInteger sign_cand1 = BigInteger
                .Parse(message_digest,
                System.Globalization.NumberStyles.AllowHexSpecifier);
            BigInteger sign_cand2 = BigInteger
                .Parse("0" + message_digest,
                System.Globalization.NumberStyles.AllowHexSpecifier);

            Console.WriteLine("Sig cand 1 is: " + sign_cand1);
            Console.WriteLine("Sig cand 2 is toString : " + sign_cand2);

            BigInteger signature1 = alice.sign(sign_cand1);
            BigInteger signature2 = alice.sign(sign_cand2);

            bool verify1 = alice.verify_sign(signature1, cry, alice.PubKey.E, alice.PriKey.D, alice.PubKey.N, msg);

            Console.WriteLine("Signature is: " + signature1);
            Console.WriteLine("Signature check is: " + verify1);
            Console.WriteLine("");

            bool verify2 = alice.verify_sign(signature2, cry, alice.PubKey.E, alice.PriKey.D, alice.PubKey.N, msg);

            Console.WriteLine("Signature is: " + signature2);
            Console.WriteLine("Signature check is: " + verify2);
            Console.WriteLine("");

            Console.WriteLine("----------------------- ALGORITHM VERIFICATION DONE -----------------------");

            try
            {
                using (FileStream fs = new FileStream("C:\\Users\\0x44419105\\Desktop\\remote.c", FileMode.Open, FileAccess.Read))
                using (FileStream fsEnc = new FileStream("C:\\Users\\0x44419105\\Desktop\\remoteEnc.c", FileMode.OpenOrCreate, FileAccess.Write))
                using (FileStream fsDec = new FileStream("C:\\Users\\0x44419105\\Desktop\\remoteDec.c", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int size = MathHelper.computeFloorByteLength(alice.PriKey.Q + alice.PriKey.P);
                    int buffer_size = 128;
                    // Preparing for OLAP, to be implemented
                    byte[] byte_arr = new byte[buffer_size];
                    byte[] byte_arr_dec = new byte[buffer_size];
                    long fileSize = fs.Length;


                    long iterations = fileSize / buffer_size;

                    int offset = 0;
                    for (int i = 0; i < iterations - 2; i++)
                    {
                        fs.Read(byte_arr, 0, (int)buffer_size);
                        byte[] encryption = alice.encrypt(new BigInteger(byte_arr)).ToByteArray();
                        fsEnc.Write(encryption, 0, buffer_size);
                        offset += (int)buffer_size;
                    }

                }
            } catch (Exception e)
            {
                Console.WriteLine("Boss, we're f$#@ed...");
            }

            try
            {
                using (FileStream fsEnc = new FileStream("C:\\Users\\0x44419105\\Desktop\\remoteEnc.c", FileMode.OpenOrCreate, FileAccess.Read))
                using (FileStream fsDec = new FileStream("C:\\Users\\0x44419105\\Desktop\\remoteDec.c", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int size = MathHelper.computeFloorByteLength(alice.PriKey.Q + alice.PriKey.P);
                    int buffer_size = 128;
                    // Preparing for OLAP, to be implemented
                    byte[] byte_arr = new byte[buffer_size];
                    byte[] byte_arr_dec = new byte[buffer_size];
                    long fileSize = fsEnc.Length;

                    long iterations = fileSize / buffer_size;

                    for (int i = 0; i < iterations - 2; i++)
                    {
                        fsEnc.Read(byte_arr_dec, 0, buffer_size);
                        byte[] decryption = alice.decrypt(new BigInteger(byte_arr_dec)).ToByteArray();
                        fsDec.Write(decryption, 0, buffer_size);
                    }

                    Console.WriteLine("");
                }
            } catch (Exception e)
            {
                Console.WriteLine("Boss, we're f$#@ed...");
            }
        }
    }
}