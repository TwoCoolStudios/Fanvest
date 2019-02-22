using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Fanvest.Core.Models;
using Microsoft.Extensions.Options;
namespace Fanvest.Services.Security
{
    public partial class EncryptionService : IEncryptionService
    {
        #region Fields
        private readonly SiteSettings _siteSettings;
        #endregion

        #region Ctors
        public EncryptionService(IOptions<SiteSettings> siteSettings) =>
            _siteSettings = siteSettings.Value;
        #endregion

        #region Utilities
        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider()
                    .CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    var toEncrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }
                return ms.ToArray();
            }
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider()
                    .CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
        #endregion

        #region Methods
        public virtual string CreateSaltKey(int size)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
        }

        public virtual string CreatePasswordHash(string password, string saltkey,
            string passwordFormat)
        {
            return CreateHash(Encoding.UTF8.GetBytes(string.Concat(password,
                saltkey)), passwordFormat);
        }

        public virtual string CreateHash(byte[] data, string hashAlgorithm)
        {
            if (string.IsNullOrEmpty(hashAlgorithm))
                throw new ArgumentNullException(nameof(hashAlgorithm));
            var algorithm = HashAlgorithm.Create(hashAlgorithm);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name.");
            var hashByteArray = algorithm.ComputeHash(data);
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _siteSettings.EncryptionKey;
            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));
                var encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
                return Convert.ToBase64String(encryptedBinary);
            }
        }

        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;
            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _siteSettings.EncryptionKey;
            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));
                var buffer = Convert.FromBase64String(cipherText);
                return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
            }
        }
        #endregion
    }
}