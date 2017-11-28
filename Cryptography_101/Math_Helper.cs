using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace Cryptography_101
{
    public static class Math_Helper
    {
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
        * Fermat primality test
        */
        public static bool isPrimeFermatTest(BigInteger candidate)
        {
            BigInteger a = candidate - 1; //Candidate can't be divisor of itself - 1

            BigInteger result = 1;
            for (BigInteger i = 0; i < candidate; i++)
            {
                result = result * a;
                result = result % candidate;
            }

            result -= a;
            return result == 0;
        }

        /**
        * Miller-Rabin primality test
        * This test is probabilistic and the test 
        */
        public static bool isPrimeMillerRabin(BigInteger candidate, int certainty)
        {
            if (candidate == 2 || candidate == 3)
                return true;
            if (candidate < 2 || candidate % 2 == 0)
                return false;

            BigInteger d = candidate - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[candidate.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= candidate - 2);

                BigInteger x = BigInteger.ModPow(a, d, candidate);
                if (x == 1 || x == candidate - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, candidate);
                    if (x == 1)
                        return false;
                    if (x == candidate - 1)
                        break;
                }

                if (x != candidate - 1)
                    return false;
            }

            return true;
        }

        /**
         * Random number generator wrapper
         */

        public static BigInteger getRandom(int length)
        {
            Random random = new Random();
            byte[] data = new byte[length];
            random.NextBytes(data);
            data[data.Length - 1] &= (byte)0x7F;

            return new BigInteger(data);
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