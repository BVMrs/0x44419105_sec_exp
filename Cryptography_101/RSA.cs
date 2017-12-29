using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
        public PublicKey pubKey = new PublicKey();

        // Private Key
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
            //Task<BigInteger[]> [] primeTask = new Task<BigInteger[]>[8];
            //BigInteger[] values = { 0 };

            //for (int i = 0; i < primeTask.Length; i++) {
            //    primeTask[i] = Task<BigInteger[]>.Factory.StartNew(() => this.getPrimes());
            //}

            //Console.WriteLine("Entering deadlock");

            //bool flag = false;
            //while (!flag) {
            //    for (int i = 0; i < primeTask.Length; i++) {
            //        if (primeTask[i].IsCompleted) {
            //            flag = true;
            //            values = primeTask[i].Result;
            //            break;
            //        }
            //    }
            //}

            //Console.WriteLine("Leaving deadlock");

            BigInteger[] primes = getPrimes();

            this.PriKey.Q = primes[0];
            this.PriKey.P = primes[1];

            calcInt();
            genKeys();
        }

        public RSA(PublicKey puk, PrivateKey pk) {
            this.PubKey = puk;
            this.PriKey = pk;
        }

        void calcInt()
        {
            pubKey.N = this.priKey.P * this.priKey.Q;

            BigInteger test_N = pubKey.N;
            BigInteger bitLength_N = new BigInteger(0);

            while (test_N / 2 != 0)
            {
                test_N /= 2;
                bitLength_N++;
            }
            bitLength_N += 1;
            
            Console.WriteLine("Size of N is {0}", bitLength_N);
            
            this.phi = (this.priKey.P - 1) * (this.priKey.Q - 1);
        }

        private BigInteger genPrimes(int size)
        {
            // we want the key to be equal to 2048 bits 
            // (as only RSA786 was cracked yet)

            return MathHelper.GenerateLargePrime(size);
        }

        private BigInteger[] getPrimes()
        {
            // Random rand = new Random();
            int bitLength_q = 0;
            int bitLength_p = 0;

            int size_q = 130;
            int size_p = 127;

            BigInteger some_q; ;
            BigInteger test_q;
            BigInteger some_p;
            BigInteger test_p;

            some_q = genPrimes(size_q);
            test_q = some_q;
            some_p = genPrimes(size_p);
            test_p = some_p;

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

            Console.WriteLine("Q size is {0}. P size is {1}.", bitLength_q, bitLength_p);

            BigInteger[] rsa_params = new BigInteger[2];
            rsa_params[0] = some_q;
            rsa_params[1] = some_p;

            return rsa_params;
        }

        private void getPrimes(int key_size)
        {
            // Random rand = new Random();
            int bitLength_q = 0;
            int bitLength_p = 0;

            int size_q = 128;
            int size_p = 129;
            do
            {
                bitLength_q = 0;
                bitLength_p = 0;

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

            } while ((bitLength_p + bitLength_q) != 2048);
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

        public BigInteger sign(BigInteger hash_message)
        {
            // This is basically a RSA decryption
            //return decrypt(BigInteger.Parse("0" + MathHelper.GetHashString(message.ToString())));
            
            Console.WriteLine("Message to be signed is: " + hash_message);
            
            return BigInteger.ModPow(hash_message, this.priKey.D, this.pubKey.N);
        }

        public bool verify_sign(BigInteger signature, BigInteger cryptogram, BigInteger public_key_e, BigInteger private_key_d, BigInteger modulo, BigInteger orig_msg)
        {
            // This is basically a RSA encryption
            // A message decryption has to be done
            BigInteger message = BigInteger.ModPow(cryptogram, private_key_d, modulo);
            String message_hash = MathHelper.GetHashString(message.ToString());
            
            BigInteger pseudo_encryption = BigInteger.ModPow(signature, public_key_e, modulo);

            return BigInteger.Parse("0" + message_hash, System.Globalization.NumberStyles.AllowHexSpecifier) 
                == pseudo_encryption;
        }

        public void storePublicKey(PublicKey pku) {

        }
    }
}