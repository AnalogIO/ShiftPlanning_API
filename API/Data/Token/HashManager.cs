using System;
using System.Security.Cryptography;

namespace Data.Token
{
    public static class HashManager
    {
        private static readonly HashAlgorithm HashAlgorithm = new SHA256Managed();
        private static readonly RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();
        /// <summary>
        /// Hashes a given password (SHA256 Hashing Algorithm)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Hash(string password)
        {
            var byteArr = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = HashAlgorithm.ComputeHash(byteArr);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateSalt()
        {
            var byteArr = new byte[256];
            RngCsp.GetBytes(byteArr);
            var salt = BitConverter.ToString(byteArr);
            return salt;
        }
    }
}