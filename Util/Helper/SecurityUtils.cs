using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Util.Helper
{
    public class SecurityUtils
    {
        public static string Encrypt(HashAlgorithm hashAlgorithm, string dataToHash)
        {
            byte[] hashBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));

            return string.Join("", hashBytes.Select(i => i.ToString("x2")));
        }

        public static string EncryptMD5(string dataToHash)
        {
            return Encrypt(new MD5CryptoServiceProvider(), dataToHash);
        }

        public static string EncryptSHA1(string dataToHash)
        {
            return Encrypt(new SHA1CryptoServiceProvider(), dataToHash);
        }

        public static string EncryptSHA256(string dataToHash)
        {
            return Encrypt(new SHA256CryptoServiceProvider(), dataToHash);
        }

        public static string EncryptSHA512(string dataToHash)
        {
            return Encrypt(new SHA512CryptoServiceProvider(), dataToHash);
        }

        public static bool IsHashMatch(HashAlgorithm hashAlgorithm, string hashedText, string unhashedText)
        {
            string hashedTextToCompare = Encrypt(hashAlgorithm, unhashedText);
            return (String.Compare(hashedText, hashedTextToCompare, false) == 0);
        }
    }
}
