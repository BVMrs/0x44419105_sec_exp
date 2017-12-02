using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace Cryptography_101
{
    public static class MathHelper
    {
        static private Random rand = new Random(DateTime.Now.Millisecond);

        public static BigInteger GenerateLargePrime(int length)
        {
            PrimalityTest primalityTest = new PrimalityTest();
            byte[] numbers = new byte[length];
            rand.NextBytes(numbers);


            BigInteger number = new BigInteger(numbers);
            int iterations = 500;

            if (primalityTest.IsPrimeMillerRabin(number, iterations)) {
                return number;
            } else {
                return GenerateLargePrime(length);
            }
        }

        /**
         * Find the greatest common divisor by the euclidean algoritm
         */
        public static BigInteger gcd_euclidean(BigInteger a, BigInteger b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            if (a == 0)
                return b;
            else
                return a;
        }

        /**
         * Find inverse of multiplication in a modulo field 
         */

        public static BigInteger findInverse(BigInteger a, BigInteger mod)
        {
            BigInteger n = mod;
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}