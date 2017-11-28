using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;

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

        private BigInteger genPrime(int size)
        {
            BigInteger temp;
            Random rand = new Random();
            bool flag = false;

            do
            {
                int myRandom = rand.Next((size > 256) ? (size - 256) : (256 - size));

                temp = getRandom(myRandom);

                if (Math_Helper.isPrimeFermatTest(temp) == false)
                    continue;

                if (Math_Helper.isPrimeMillerRabin(temp, 100) == true)
                    flag = true;

            } while (flag == false);

            return temp;
        }

        private void getPrimes()
        {
            Random rand = new Random();
            int size_q = rand.Next(128, 384);
            int size_p = (size_q > 256) ? (size_q - 256) : (256 - size_q);
            this.q = genPrime(size_q);
            this.p = genPrime(size_p);
        }

        void genKeys()
        {
            Random random = new Random();
            bool flag = false;
            BigInteger tmp = 0;

            while (flag == false)
            {
                tmp = getRandom(256);
                if (Math_Helper.gcd_euclidean(tmp, this.phi) == 1)
                {
                    flag = true;
                }
            }

            this.e = tmp;
            this.d = Math_Helper.findInverse(this.e, this.phi);
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