using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Cryptography_101
{
    public class PrivateKey
    {
        private BigInteger d;
        private BigInteger p;
        private BigInteger q;
        private string owner;

        public BigInteger D
        {
            get {
                return d;
            }

            set {
                this.d = value;
            }
        }

        public BigInteger P
        {
            get {
                return p;
            }

            set {
                this.p = value;
            }
        }

        public BigInteger Q
        {
            get {
                return q;
            }

            set {
                this.q = value;
            }
        }

        public string Owner
        {
            get {
                return owner;
            }

            set {
                this.owner = value;
            }
        }

        public PrivateKey(BigInteger d, BigInteger p, BigInteger q, string owner)
        {
            this.d = d;
            this.p = p;
            this.q = q;
            this.owner = owner;
        }
        public PrivateKey()
        {
        }
    }
}
