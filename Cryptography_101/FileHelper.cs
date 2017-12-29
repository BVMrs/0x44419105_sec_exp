using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_101
{
    class FileHelper
    {
        FileStream fs;

        public FileStream Fs
        {
            get {
                return fs;
            }

            set {
                this.fs = value;
            }
        }
        
    }
}
