using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Cryptography_101
{
    public class PublicKey
    {
        private BigInteger e;
        private BigInteger n;

        public PublicKey(BigInteger e, BigInteger n)
        {
            this.e = e;
            this.n = n;
        }

        public PublicKey()
        {
        }

        public BigInteger E
        {
            get {
                return e;
            }

            set {
                this.e = value;
            }
        }

        public BigInteger N
        {
            get {
                return n;
            }

            set {
                this.n = value;
            }
        }
    }
}
