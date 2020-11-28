using Delta.AppServer.Users;

namespace Delta.AppServer.Core.Security
{
    public class AuthInfo
    {
        public User User { get; set; }
        public string Role { get; set; }
    }
}