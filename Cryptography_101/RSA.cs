using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Cryptography_101
{
    public class RSA
    {
        /**
        *  Any external user shall only have access to
        *  N, d and phi
        *  While
        *  e, p, q 
        *  shall be kept secret
        */

        private BigInteger N;
        private BigInteger phi;
        private BigInteger e;
        private BigInteger d;
        private BigInteger p;
        private BigInteger q;

        public RSA()
        {
            getPrimes();
            calcInt();
            genKeys();
        }

        void calcInt()
        {
            this.N = this.p * this.q;
            this.phi = (this.q - 1) * (this.p - 1);
        }

        private BigInteger genPrimes(int size)
        {
            // we want the key to be equal to 2048 bits 
            // (as only RSA786 was cracked yet)
    
            return MathHelper.GenerateLargePrime(size);
        }

        private void getPrimes()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            int bitLength_q = 0;
            int bitLength_p = 0;

            Random rand = new Random();
            int size_q = rand.Next(96, 192);
            int size_p = 256 - size_q;
            this.q = genPrimes(size_q);
            this.p = genPrimes(size_p);

            BigInteger test_q = this.q;
            BigInteger test_p = this.p;

            do
            {
                bitLength_q++;
                test_q /= 2;
            } while (test_q != 0);

            do
            {
                bitLength_p++;
                test_p /= 2;
            } while (test_p != 0);

            sw.Stop();

            Console.WriteLine("Time elapsed calculating primes: {0}", sw.Elapsed);
            Console.WriteLine("Size of p is " + bitLength_p + " bits. Size of q is " + bitLength_q + " bits.");
        }

        void genKeys()
        {
            Random random = new Random();
            bool flag = false;
            BigInteger tmp = 0;

            while (flag == false)
            {
                tmp = getRandom(256);
                if (MathHelper.gcd_euclidean(tmp, this.phi) == 1)
                {
                    flag = true;
                }
            }

            this.e = tmp;
            this.d = MathHelper.findInverse(this.e, this.phi);
        }

        private BigInteger getRandom(int length)
        {
            Random random = new Random();
            byte[] data = new byte[length];
            random.NextBytes(data);
            data[data.Length - 1] &= (byte)0x7F;

            return new BigInteger(data);
        }

        public BigInteger encrypt(BigInteger message)
        {
            BigInteger tmp;

            if (message > this.N)
                return -1;

            tmp = BigInteger.ModPow(message, this.e, this.N);

            return tmp;
        }

        public BigInteger decrypt(BigInteger cryptogram)
        {
            return BigInteger.ModPow(cryptogram, this.d, this.N);
        }

    }
}