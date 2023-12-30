namespace Delta.AppServer.Encryption;

public record CreateEncryptionKeyResponse(EncryptionKeyView EncryptionKey, string Value);