namespace Delta.AppServer.Core.Security
{
    public class AuthService
    {
        private readonly AuthConfig _authConfig;

        public AuthService(AuthConfig authConfig)
        {
            _authConfig = authConfig;
        }

        public Account Login(string username, string password)
        {
            if (username == _authConfig.AdminUsername &&
                password == _authConfig.AdminPassword)
            {
                return CreateAdmin();
            }

            return null;
        }

        private Account CreateAdmin()
        {
            return new Account
            {
                Id = 1,
                Username = _authConfig.AdminUsername
            };
        }

        public string GetRole(Account account)
        {
            if (CreateAdmin().Id == account?.Id)
            {
                return "Admin";
            }

            return "Guest";
        }
    }
}