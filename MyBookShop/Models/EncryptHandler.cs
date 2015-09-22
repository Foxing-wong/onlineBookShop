using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MyBookShop.Models
{
    public class EncryptHandler
    {
        private static string password = "bookshop";
        //JIAMI
        public static string Encrypt(string cleartext)
        {
            string password2 = "Ahbool";

            string cipher;
            char[] key = new char[8];
            if (password.Length > 8)
            {
                password = password.Remove(8);
            }
            password.CopyTo(0, key, 0, password.Length);

            char[] iv = new char[8];
            if (password2.Length > 8)
            {
                password2 = password2.Remove(8);
            }
            password2.CopyTo(0, iv, 0, password2.Length);

            if (cleartext == null)
            {
                return string.Empty;
            }

            SymmetricAlgorithm serviceProvider = new DESCryptoServiceProvider();
            serviceProvider.Key = Encoding.ASCII.GetBytes(key);
            serviceProvider.IV = Encoding.ASCII.GetBytes(iv);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, serviceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(cleartext);
            streamWriter.Dispose();
            cryptoStream.Dispose();

            byte[] signData = memoryStream.ToArray();
            memoryStream.Dispose();
            serviceProvider.Clear();
            cipher = Convert.ToBase64String(signData);

            return cipher;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string Decrypt(string ciphertext)
        {
            string password2 = "Ahbool";

            string cipher = string.Empty;

            try
            {
                char[] key = new char[8];
                if (password.Length > 8)
                {
                    password = password.Remove(8);
                }
                password.CopyTo(0, key, 0, password.Length);

                char[] iv = new char[8];
                if (password2.Length > 8)
                {
                    password2 = password2.Remove(8);
                }
                password2.CopyTo(0, iv, 0, password2.Length);

                if (ciphertext == null)
                {
                    return cipher;
                }

                SymmetricAlgorithm serviceProvider = new DESCryptoServiceProvider();
                serviceProvider.Key = Encoding.ASCII.GetBytes(key);
                serviceProvider.IV = Encoding.ASCII.GetBytes(iv);

                byte[] contentArray = Convert.FromBase64String(ciphertext);
                MemoryStream memoryStream = new MemoryStream(contentArray);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, serviceProvider.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader streamReader = new StreamReader(cryptoStream);

                cipher = streamReader.ReadToEnd();

                streamReader.Dispose();
                cryptoStream.Dispose();
                memoryStream.Dispose();
                serviceProvider.Clear();

            }
            catch
            {
                cipher = "123";
            }

            return cipher;
        }
    }
}