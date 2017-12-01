using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace Cryptography_101
{
    public class PrimalityTest
    {
        public enum NumberType
        {
            Composite,
            Prime
        }

        public bool IsPrimeMillerRabin(BigInteger candidate, int iterations)
        {
            NumberType nType = MillerRabin(candidate, iterations);
            return nType == NumberType.Prime;
        }

        public bool IsPrimeFermat(BigInteger candidate)
        {
            NumberType type = FermatPseudoPrime(candidate);
            return type == NumberType.Prime;
        }

        // Primality Test based on the Miller-Rabin algorithm
        public NumberType MillerRabin(BigInteger candidate, BigInteger iterations)
        {
            //Candidate can't be divisor of itself - 1
            BigInteger candidateMinusOne = BigInteger.Subtract(candidate, 1);
            
            for(int i = 1; i <= iterations; i++) {
                BigInteger rand = Random(1, candidateMinusOne);

                if(Witness(rand, candidate)) {
                    return NumberType.Composite;
                }
            }

            return NumberType.Prime;
        }

        public NumberType FermatPseudoPrime(BigInteger n)
        {
            BigInteger modExp = BigInteger.ModPow(2, BigInteger.Subtract(n, 1), n);

            if (!modExp.IsOne) {
                return NumberType.Composite;
            } else {
                return NumberType.Prime;
            }
        }

        // Some evil bit level hacking here idk
        public KeyValuePair<int, BigInteger> Get_T_and_U(BigInteger candidateMinusOne)
        {
            // Convert candidate - 1 to byte array
            byte[] candidateBytes = candidateMinusOne.ToByteArray();
            BitArray candidateBits = new BitArray(candidateBytes);

            int t = 0;
            BigInteger u = new BigInteger();

            int n = candidateBits.Count - 1;
            bool lastBit = candidateBits[n];

            // Calc t
            while (!lastBit) {
                t++;
                n--;
                lastBit = candidateBits[n];
            }

            for (int k = ((candidateBits.Count - 1) - t); k >= 0; k--) {
                BigInteger bitValue = 0;

                if (candidateBits[k]) {
                    bitValue = BigInteger.Pow(2, k);
                }

                u = BigInteger.Add(u, bitValue);
            }

            KeyValuePair<int, BigInteger> t_and_u = new KeyValuePair<int, BigInteger>(t, u);
            return t_and_u;
        }

        public bool Witness(BigInteger rand, BigInteger candidate) {
            KeyValuePair<int, BigInteger> t_and_u = Get_T_and_U(BigInteger.Subtract(candidate, 1));
            int t = t_and_u.Key;
            BigInteger u = t_and_u.Value;
            BigInteger[] x = new BigInteger[t + 1];

            x[0] = BigInteger.ModPow(rand, u, candidate);

            for(int i = 1; i <= t; i++) {
                // x[i] = x[i - 1] ^ 2 mod n
                x[i] = BigInteger.ModPow(BigInteger.Multiply(x[i - 1], x[i - 1]), 1, candidate);
                BigInteger minus = BigInteger.Subtract(x[i - 1], BigInteger.Subtract(candidate, 1));

                if(x[i] == 1 && x[i - 1] != 1 && !minus.IsZero) {
                    return true;
                }
            }

            if (!x[t].IsOne) {
                return true;
            }

            return false;
        }

        //Generate a random BigInteger between min and max
        public BigInteger Random(BigInteger min, BigInteger max) {
            byte[] maxBytes = max.ToByteArray();
            BitArray maxBits = new BitArray(maxBytes);
            Random rand = new System.Random(DateTime.Now.Millisecond);

            for(int i = 0; i < maxBits.Length; i++) {
                // Randomly set the bit
                int randomInt = rand.Next();
                if((randomInt % 2) == 0) {
                    // Reverse the bit
                    maxBits[i] = !maxBits[i];
                }
            }

            BigInteger result = new BigInteger();

            // Convert the bits back to BigInteger
            for(int k = (maxBits.Count - 1); k >= 0; k--) {
                BigInteger bitValue = 0;

                if (maxBits[k]) {
                    bitValue = BigInteger.Pow(2, k);
                }

                result = BigInteger.Add(result, bitValue);
            }

            // Ultimately generate the random number
            BigInteger randomBigInt = BigInteger.ModPow(result, 1, BigInteger.Add(max, min));
            return randomBigInt;
        }
    }
}