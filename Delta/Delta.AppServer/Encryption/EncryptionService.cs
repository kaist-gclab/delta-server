using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Encryption;

public class EncryptionService(DeltaContext context)
{
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

    public async Task AddEncryptionKey(
        CreateEncryptionKeyRequest createEncryptionKeyRequest)
    {
        var name = createEncryptionKeyRequest.Name;

        await using var trx = await context.Database.BeginTransactionAsync();
        if (await context.EncryptionKey.AnyAsync(k => k.Name == name))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        var value = Guid.NewGuid().ToString();
        var encryptionKey = new EncryptionKey
        {
            Name = name,
            Value = value,
            Enabled = false,
            Optimized = false
        };
        await context.AddAsync(encryptionKey);
        await context.SaveChangesAsync();
        await trx.CommitAsync();
    }

    public async Task<IEnumerable<EncryptionKeyView>> GetEncryptionKeys()
    {
        var q = from k in context.EncryptionKey
            select new EncryptionKeyView(k.Id, k.Name, k.Enabled, k.Optimized,
                k.Buckets.Count, k.Buckets.Sum(b => b.Assets.Count));

        return await q.ToListAsync();
    }

    public byte[]? Encrypt(EncryptionKey encryptionKey, byte[] plainData)
    {
        if (!encryptionKey.Enabled)
        {
            return null;
        }

        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);
        var cipherData = new byte[plainData.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        var key = GetKey(encryptionKey, _salt);
        if (key == null)
        {
            return null;
        }

        using var aes = new AesGcm(key, 16);
        aes.Encrypt(nonce, plainData, cipherData, tag);
        return nonce.Concat(cipherData).Concat(tag).ToArray();
    }

    public byte[]? Decrypt(EncryptionKey encryptionKey, byte[] cipherData)
    {
        if (!encryptionKey.Enabled)
        {
            return null;
        }

        var nonce = new ArraySegment<byte>(cipherData, 0, AesGcm.NonceByteSizes.MaxSize);
        var dataLength = cipherData.Length - AesGcm.NonceByteSizes.MaxSize - AesGcm.TagByteSizes.MaxSize;
        var data = new ArraySegment<byte>(cipherData, AesGcm.NonceByteSizes.MaxSize, dataLength);
        var tag = new ArraySegment<byte>(cipherData,
            AesGcm.NonceByteSizes.MaxSize + dataLength, AesGcm.TagByteSizes.MaxSize);
        var plainText = new byte[data.Count];

        var key = GetKey(encryptionKey, _salt);
        if (key == null)
        {
            return null;
        }

        using var aes = new AesGcm(key, 16);
        aes.Decrypt(nonce, data, tag, plainText);

        return plainText;
    }

    public async Task<EncryptionKeyView?> GetEncryptionKey(long id)
    {
        var q = from k in context.EncryptionKey
            where k.Id == id
            select new EncryptionKeyView(k.Id, k.Name, k.Enabled, k.Optimized,
                k.Buckets.Count, k.Buckets.Sum(b => b.Assets.Count));

        return await q.FirstOrDefaultAsync();
    }

    private static byte[]? GetKey(EncryptionKey key, byte[] salt)
    {
        if (key.Value == null)
        {
            return null;
        }

        return KeyDerivation.Pbkdf2(
            password: key.Value,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);
    }

    public async Task EnableKey(long encryptionKeyId)
    {
        var key = await context.EncryptionKey.FindAsync(encryptionKeyId);
        if (key == null)
        {
            return;
        }

        key.Enabled = true;
        await context.SaveChangesAsync();
    }

    public async Task DeleteEncryptionKey(long id)
    {
        var key = await context.EncryptionKey.FindAsync(id);
        if (key == null)
        {
            return;
        }
        
        context.Remove(key);
        await context.SaveChangesAsync();
    }
}