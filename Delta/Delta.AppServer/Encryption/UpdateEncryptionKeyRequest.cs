namespace Delta.AppServer.Encryption;

public record UpdateEncryptionKeyRequest(string Name, bool Enabled, bool Optimized);
