using Delta.AppServer.Users;

namespace Delta.AppServer.Core.Security;

public record AuthInfo(User User, string Role);