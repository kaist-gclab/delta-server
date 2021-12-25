using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Delta.AppServer.Encryption
{
    public class EncryptionService
    {
        private readonly DeltaContext _context;

        private readonly byte[] _salt =
        {
            0x30, 0x04, 0xa3, 0x66, 0x44, 0x6f, 0x7d, 0xd2,
            0x6d, 0xf0, 0x99, 0x8e, 0x01, 0x3a, 0x92, 0xc5,
            0x41, 0xe6, 0xd0, 0x18, 0x4f, 0x52, 0xe8, 0xe9,
            0xe2, 0x08, 0xe8, 0x8a, 0x31, 0xd3, 0x32, 0x46,
            0xb7, 0xed, 0x8e, 0x43, 0x3a, 0x1a, 0xd7, 0x72,
            0x45, 0x39, 0xdf, 0xee, 0xa8, 0x8e, 0xbc, 0x78,
            0x24, 0xd8, 0x65, 0xfc, 0xf3, 0x1d, 0x05, 0x5f,
            0xa4, 0xa1, 0x5a, 0xec, 0x70, 0x9d, 0x14, 0x0e
        };

        public EncryptionService(DeltaContext context)
        {
            _context = context;
        }

        public EncryptionKey AddEncryptionKey(CreateEncryptionKeyRequest createEncryptionKeyRequest)
        {
            var name = createEncryptionKeyRequest.Name;

            using var trx = _context.Database.BeginTransaction();
            if (_context.EncryptionKeys.Any(k => k.Name == name))
            {
                throw new Exception();
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception();
            }

            var value = Guid.NewGuid().ToString();
            var encryptionKey = new EncryptionKey
            {
                Name = name,
                Value = value,
                Enabled = false
            };
            encryptionKey = _context.Add(encryptionKey).Entity;
            _context.SaveChanges();
            trx.Commit();
            return encryptionKey;
        }

        public IQueryable<EncryptionKey> GetEncryptionKeys() => _context.EncryptionKeys;

        public byte[] Encrypt(EncryptionKey encryptionKey, byte[] plainData)
        {
            if (!encryptionKey.Enabled)
            {
                throw new Exception();
            }

            var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            RandomNumberGenerator.Fill(nonce);
            var cipherData = new byte[plainData.Length];
            var tag = new byte[AesGcm.TagByteSizes.MaxSize];
            using var aes = new AesGcm(GetKey(encryptionKey, _salt));
            aes.Encrypt(nonce, plainData, cipherData, tag);
            return cipherData;
        }

        public byte[] Decrypt(EncryptionKey encryptionKey, byte[] cipherData)
        {
            if (!encryptionKey.Enabled)
            {
                throw new Exception();
            }

            using var msDecrypt = new MemoryStream(cipherData);
            var iv = new byte[128 / 8];
            msDecrypt.Read(iv);

            using var aes = new AesCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                KeySize = 256,
                Key = GetKey(encryptionKey, _salt),
                BlockSize = 128,
                Padding = PaddingMode.PKCS7,
                IV = iv
            };
            using var csDecrypt =
                new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);

            using var stream = new MemoryStream();
            csDecrypt.CopyTo(stream);
            return stream.ToArray();
        }

        public EncryptionKey GetEncryptionKey(string name)
        {
            if (name == null)
            {
                return null;
            }

            return (from e in _context.EncryptionKeys
                where e.Name == name
                select e).FirstOrDefault();
        }

        private static byte[] GetKey(EncryptionKey key, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: key.Value,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
        }
    }
}