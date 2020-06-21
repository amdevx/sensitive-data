using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace sensitiveAPI.Helper
{
    public class DataProtector
    {
        protected virtual AesCryptoServiceProvider GetAesProvider()
        {
            var provider = new AesCryptoServiceProvider
            {
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            provider.GenerateIV();

            return provider;
        }

        protected virtual byte[] GetSalt()
        {
            var maximumSaltLength = 32;
            var salt = new byte[maximumSaltLength];

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public virtual string Encrypt(string password, string cleartext)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"{nameof(password)} has no value", nameof(password));
            }

            var aesModel = new AesModel
            {
                Algorithm = "aes-gcm-256",
                Version = "1"
            };

            var provider = GetAesProvider();
            var keyDer = new KeyDerivation {Salt = GetSalt(), WorkFactor = 12000, Function = "PBDKF2"};

            var key = new Rfc2898DeriveBytes(password, keyDer.Salt, keyDer.WorkFactor);
            provider.Key = key.GetBytes(provider.KeySize / 8);
            aesModel.InitializationVector = provider.IV;
            aesModel.KeyDerivation = keyDer;
            var transform = provider.CreateEncryptor();
            var encryptedByte = transform.TransformFinalBlock(Encoding.UTF8.GetBytes(cleartext), 0, cleartext.Length);
            aesModel.Payload = Convert.ToBase64String(encryptedByte);
            var stringAes = JsonSerializer.Serialize(aesModel);
            if (string.IsNullOrWhiteSpace(stringAes))
            {
                throw new NotFoundException($"Could not Serialize {nameof(aesModel)}.");
            }

            return stringAes;
        }

        public virtual string Decrypt(string password, AesModel aesModel)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"{nameof(password)} has no value", nameof(password));
            var key = new Rfc2898DeriveBytes(password, aesModel.KeyDerivation.Salt, aesModel.KeyDerivation.WorkFactor);

            var provider = GetAesProvider();
            provider.Key = key.GetBytes(provider.KeySize / 8);
            provider.IV = aesModel.InitializationVector;

            var cipherText = Convert.FromBase64String(aesModel.Payload);

            var transform = provider.CreateDecryptor();
            var decryptedBytes = transform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}