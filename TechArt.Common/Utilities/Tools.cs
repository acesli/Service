using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TechArt.Common.Utilities
{
    public class Tools
    {
        public static string EncryptPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                sha256.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);
                return Encoding.UTF8.GetString(sha256.Hash);
            }
        }

    }
}
