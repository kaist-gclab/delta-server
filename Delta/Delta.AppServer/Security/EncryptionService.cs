using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Delta.AppServer.Security
{
    public class EncryptionService
    {
        private readonly DeltaContext _context;
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(DeltaContext context, ILogger<EncryptionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public EncryptionKey AddEncryptionKey(string name)
        {
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

            _logger.LogWarning("EncryptionService.Encrypt is not secure.");
            var str = Convert.ToBase64String(plainData) + encryptionKey.Value;
            return Encoding.UTF8.GetBytes(str);
        }

        public byte[] Decrypt(EncryptionKey encryptionKey, byte[] cipherData)
        {
            if (!encryptionKey.Enabled)
            {
                throw new Exception();
            }

            _logger.LogWarning("EncryptionService.Decrypt is not secure.");
            var str = Encoding.UTF8.GetString(cipherData);
            str = str.Substring(0, str.Length - encryptionKey.Value.Length);
            return Convert.FromBase64String(str);
        }
    }
}