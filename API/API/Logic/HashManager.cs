using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace API.Logic
{
    public static class HashManager
    {
        private static HashAlgorithm _hash = new SHA256Managed();
        private static RNGCryptoServiceProvider _rngCsp = new RNGCryptoServiceProvider();
        /// <summary>
        /// Hashes a given password (SHA256 Hashing Algorithm)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Hash(string password)
        {
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = _hash.ComputeHash(byteArr);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateSalt()
        {
            var byteArr = new byte[256];
            _rngCsp.GetBytes(byteArr);
            string salt = BitConverter.ToString(byteArr);
            return salt;
        }
    }
}