namespace Delta.AppServer.Encryption;

public record CreateEncryptionKeyRequest(string Name, bool Enabled, bool Optimized);
