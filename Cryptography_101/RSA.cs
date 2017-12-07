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
        private PublicKey pubKey = new PublicKey();

        // Private Key
        private BigInteger d;
        private BigInteger p;
        private BigInteger q;
        private PrivateKey priKey = new PrivateKey();

        internal PublicKey PubKey
        {
            get {
                return pubKey;
            }

            set {
                this.pubKey = value;
            }
        }

        // This is only for debug
        internal PrivateKey PriKey
        {
            get {
                return priKey;
            }

            set {
                this.priKey = value;
            }
        }

        public RSA()
        {
            getPrimes();
            calcInt();
            genKeys();
        }

        void calcInt()
        {
            pubKey.N = this.priKey.P * this.priKey.Q;
            this.phi = (this.priKey.P - 1) * (this.priKey.Q - 1);
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

            pubKey.E = tmp;
            priKey.D = MathHelper.findInverse(this.pubKey.E, this.phi);
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
            // This is basically a RSA decryption
            //return decrypt(BigInteger.Parse("0" + MathHelper.GetHashString(message.ToString())));
            String hash = "0" + MathHelper
                .GetHashString(message.ToString());

            Console.WriteLine("Message Hash is : " + hash);

            return BigInteger.ModPow((BigInteger
                .Parse(hash, 
                System.Globalization.NumberStyles.AllowHexSpecifier
               )), this.priKey.D, this.pubKey.N);
        }

        public bool verify_sign(BigInteger signature, BigInteger cryptogram, BigInteger public_key_e, BigInteger private_key_d, BigInteger modulo)
        {
            // This is basically a RSA encryption
            // A message decryption has to be done
            BigInteger message = BigInteger.ModPow(cryptogram, private_key_d, modulo);
            String message_hash = "0" + MathHelper.GetHashString(message.ToString().ToLower());
            BigInteger hashed_message = BigInteger.Parse(message_hash,
                System.Globalization.NumberStyles.AllowHexSpecifier);
            Console.WriteLine("Message from cryptogram is: " + message);
            Console.WriteLine("Message digest after decryption: " + hashed_message);
            Console.WriteLine("Message digest alt: " + message_hash);

            BigInteger pseudo_encryption = BigInteger.ModPow(signature, public_key_e, modulo);
            String stringif = "0" + MathHelper.GetHashString(pseudo_encryption.ToString().ToLower());
            Console.WriteLine("Signature Hash after \"encryption\" is: " + stringif);
            BigInteger signature_candidate = BigInteger.Parse(stringif, 
                System.Globalization.NumberStyles.AllowHexSpecifier);
            Console.WriteLine("Signature candidate: " + signature_candidate);

            return hashed_message == signature_candidate;
        }
    }
}