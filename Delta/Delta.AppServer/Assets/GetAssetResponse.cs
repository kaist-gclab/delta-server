namespace Delta.AppServer.Assets;

public record GetAssetResponse(Asset Asset, string PresignedDownloadUrl);