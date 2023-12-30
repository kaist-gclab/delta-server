using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Core.Security;

namespace Delta.AppServer.Users;

public class UserService
{
    private readonly DeltaContext _context;
    private readonly AuthConfig _authConfig;

    public UserService(DeltaContext context, AuthConfig authConfig)
    {
        _context = context;
        _authConfig = authConfig;
    }

    public User? Login(string username, string password)
    {
        var user = GetUserByUsername(username);
        if (user == null)
        {
            return null;
        }

        if (user.Username == _authConfig.AdminUsername)
        {
            return password == _authConfig.AdminPassword ? user : null;
        }

        return user.ValidatePassword(password) ? user : null;
    }

    public async Task AddUser(string username, string password, string name)
    {
        if (username == _authConfig.AdminUsername)
        {
            return null;
        }

        await using var trx = await _context.Database.BeginTransactionAsync();

        var duplicates = from u in _context.Users
            where u.Username == username
            select u;
        if (duplicates.Any())
        {
            return null;
        }

        var user = new User
        {
            Name = name,
            Username = username
        };
        user.ChangePassword(password);

        user = _context.Add(user).Entity;
        _context.SaveChanges();
        await trx.CommitAsync();
        return user;
    }

    public void ChangePassword(string username, string newPassword)
    {
        if (username == _authConfig.AdminUsername)
        {
            return;
        }

        var user = GetUserByUsername(username);
        user?.ChangePassword(newPassword);
        _context.SaveChanges();
    }

    public string GetRole(User user)
    {
        return user.Username == _authConfig.AdminUsername ? "Admin" : "User";
    }

    private User? GetUserByUsername(string username)
    {
        if (username == _authConfig.AdminUsername)
        {
            return new User
            {
                Name = _authConfig.AdminUsername,
                Username = _authConfig.AdminUsername
            };
        }

        return (from u in _context.Users
            where u.Username == username
            select u).FirstOrDefault();
    }
}