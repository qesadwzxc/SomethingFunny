using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestConsoleApplication
{
    /// <summary>
    /// Encryption helper.
    /// </summary>
    public static class EncryptionHelper
    {
        private static string SaltValue => "cx-user.";
        private static string PwdValue => "cxuserpwd";

        /// <summary>   
        /// 对字符串进行加密
        /// </summary>   
        /// <param name="input">需要加密的字符串</param>   
        /// <returns>加密后的字符串</returns>   
        public static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] salt = Encoding.UTF8.GetBytes(SaltValue);

            var rfc = new Rfc2898DeriveBytes(PwdValue, salt);
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = rfc.GetBytes(aes.KeySize / 8); aes.IV = rfc.GetBytes(aes.BlockSize / 8);
            ICryptoTransform encryptTransform = aes.CreateEncryptor();

            // 加密后的输出流   
            MemoryStream encryptStream = new MemoryStream();
            CryptoStream encryptor = new CryptoStream(encryptStream, encryptTransform, CryptoStreamMode.Write);

            encryptor.Write(data, 0, data.Length);
            encryptor.Close();

            string encryptedString = Convert.ToBase64String(encryptStream.ToArray());

            return encryptedString;
        }

        /// <summary>
        /// 对加密字符串进行解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns>解密后的原字符串</returns>
        public static string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            byte[] encryptBytes = Convert.FromBase64String(input);
            byte[] salt = Encoding.UTF8.GetBytes(SaltValue);

            AesManaged aes = new AesManaged();

            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(PwdValue, salt);

            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);
            ICryptoTransform decryptTransform = aes.CreateDecryptor();

            // 解密后的输出流   
            MemoryStream decryptStream = new MemoryStream();
            CryptoStream decryptor = new CryptoStream(
                decryptStream, decryptTransform, CryptoStreamMode.Write);

            // 将一个字节序列写入当前 CryptoStream （完成解密的过程）   
            decryptor.Write(encryptBytes, 0, encryptBytes.Length);
            decryptor.Close();

            // 将解密后所得到的流转换为字符串   
            byte[] decryptBytes = decryptStream.ToArray();
            string decryptedString = Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

            return decryptedString;
        }
    }
}

