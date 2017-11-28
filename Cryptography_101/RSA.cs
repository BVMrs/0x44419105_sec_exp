using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;

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
    
    /**
     * Fermat primality test
     */
    private bool isPrimeFermatTest(BigInteger candidate)
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
    private bool IsPrimeMillerRabin(BigInteger candidate, int certainty)
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

    private BigInteger genPrime(int size)
    {
        BigInteger temp;
        Random rand = new Random();
        bool flag   = false;

        do
        {
            int myRandom = rand.Next((size > 256) ? (size - 256) : (256 - size));

            temp = getRandom(myRandom);

            if (isPrimeFermatTest(temp) == false)
                continue;

            if (IsPrimeMillerRabin(temp, 100) == true)
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

    BigInteger gcd_euclidean(BigInteger a, BigInteger b)
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

    void genKeys()
    {
        Random random = new Random();
        bool flag = false;
        BigInteger tmp = 0;

        while (flag == false)
        {
            tmp = getRandom(512);
            if (gcd_euclidean(tmp, this.phi) == 1)
            {
                flag = true;

            }
        }

        this.e = tmp;

        this.d = findInverse(this.e);
    }

    private BigInteger getRandom(int length)
    {
        Random random = new Random();
        byte[] data = new byte[length];
        random.NextBytes(data);
        data[data.Length - 1] &= (byte) 0x7F;

        return new BigInteger(data);
    }

    BigInteger findInverse(BigInteger a)
    {
        BigInteger n = this.phi;
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