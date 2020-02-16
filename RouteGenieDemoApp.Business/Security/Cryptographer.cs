using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Business.Security
{
    public class Cryptographer : IDisposable
    {

        public string CreateSalt()
        {

            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);


            return Convert.ToBase64String(data);
        }


        public string HashPassword(string password, string salt)
        {
            password = password.Trim();
            salt = salt.Trim();

            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            byte[] inArray = null;
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            inArray = algorithm.ComputeHash(dst);


            return Convert.ToBase64String(inArray);
        }


        public string CreateUniqueKey(int length)
        {
            string guidResult = string.Empty;


            while (guidResult.Length < length)
            {

                guidResult += Guid.NewGuid().ToString().GetHashCode().ToString("x");
            }


            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);


            return guidResult.Substring(0, length);
        }


        public void Dispose()
        {
            GC.Collect();

        }
    }
}
