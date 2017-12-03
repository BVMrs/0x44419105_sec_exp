using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Cryptography_101
{
    // The may be multiple interlocutors, therefore a provider is needed to spawn 
    // new RSA instances with new keys or load keys that already exist
    class ProviderRSA
    {
        // rsaPool - a list of RSA instances 
        // context - a string that leads to the path where one can find the config
        //         - files for the given ProviderRSA object
        private List<RSA> rsaPool;
        private string context;

        public ProviderRSA(List<RSA> rsaPool, string context)
        {
            this.rsaPool = rsaPool;
            this.context = context;
        }

        public ProviderRSA(List<RSA> rsaPool)
        {
            this.rsaPool = rsaPool;
        }

        public ProviderRSA(string context)
        {
            this.context = context;
        }

        public ProviderRSA()
        {
        }

        public List<RSA> RsaPool
        {
            get {
                return rsaPool;
            }

            set {
                this.rsaPool = value;
            }
        }

        public string Context
        {
            get {
                return context;
            }

            set {
                this.context = value;
            }
        }

        // One instance of RSA may speak with multiple instances. It will mave one or multiple
        // private keys (for itself) and multiple public keys (for others)
        // therefore 
        public void addToContext(string interlocutor, BigInteger public_key)
        {

        }

        // Check if we already spoke with the required interlocutor. 
        public void checkContext(string interlocutor, BigInteger public_key)
        {

        }

        // Remove the intelocutor in case the conversation is no longer relevant or 
        // if it's security was compromised
        public void removeFromContext(string interlocutor, BigInteger public_key)
        {

        }

        // Add a new RSA instance to the RSA pool
        public void addRSAtoPool(RSA rsa)
        {

        }


        // Add a new RSA instance to the RSA pool
        // One will have to identify each RSA instance via an identifier
        public void removeRSAfromPool(RSA rsa)
        {

        }

        
        public RSA fetchRSAfromPool()
        {
            RSA rsa = new RSA();
            return rsa;
        }
    }
}
