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

        private BigInteger phi;

        // Public Key
        private BigInteger N;
        private BigInteger e;
        private PublicKey pubKey;

        // Private Key
        private BigInteger d;
        private BigInteger p;
        private BigInteger q;
        private PrivateKey priKey;


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

            // Random rand = new Random();

            int size_q = 128;
            int size_p = 128;
            
            this.priKey.Q = genPrimes(size_q); 
            this.priKey.P = genPrimes(size_p);

            BigInteger test_q = this.priKey.Q;
            BigInteger test_p = this.priKey.P;

            while (test_q / 2 != 0)
            {
                test_q /= 2;
                bitLength_q++;
            }
            bitLength_q += 1;

            while (test_p / 2 != 0)
            {
                test_p /= 2;
                bitLength_p++;
            }
            bitLength_p += 1;

            sw.Stop();

            //Console.WriteLine("Time elapsed calculating primes: {0}", sw.Elapsed);
            Console.WriteLine("Size of p is " + bitLength_p + " bits. Size of q is " + bitLength_q + " bits.");
        }

        void genKeys()
        {
            Random random = new Random();
            bool flag = false;
            BigInteger tmp = 0;


            while (flag == false)
            {
                tmp = getRandom(192);
                if (MathHelper.gcd_euclidean(tmp, this.phi) == 1)
                {
                    flag = true;
                }
            }

            this.pubKey.E = tmp;
            this.priKey.D = MathHelper.findInverse(this.pubKey.E, this.phi);
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

            if (message > this.pubKey.N)
                return -1;

            tmp = BigInteger.ModPow(message, this.pubKey.E, this.pubKey.N);

            return tmp;
        }

        public BigInteger decrypt(BigInteger cryptogram)
        {
            return BigInteger.ModPow(cryptogram, this.priKey.D, this.pubKey.N);
        }

        public BigInteger sign(BigInteger message)
        {
            return BigInteger.ModPow((BigInteger
                .Parse(MathHelper
                .GetHashString(message
                    .ToString()
                ))), this.priKey.D, this.pubKey.N);
        }

        public bool verify_sign(BigInteger signature, BigInteger cryptogram, BigInteger public_key_e, BigInteger private_key_d, BigInteger modulo)
        {
            return BigInteger.ModPow((BigInteger
                .Parse(MathHelper
                .GetHashString(signature
                    .ToString()
                ))), public_key_e, modulo) == 
                BigInteger.Parse(MathHelper.GetHashString(cryptogram.ToString()));
        }
    }
}