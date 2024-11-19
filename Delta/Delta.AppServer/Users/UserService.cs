using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Users;

public class UserService(DeltaContext context, AuthConfig authConfig)
{
    public async Task<User?> Login(string username, string password)
    {
        var user = await GetUserByUsername(username);
        if (user == null)
        {
            return null;
        }

        if (user.Username == authConfig.AdminUsername)
        {
            return password == authConfig.AdminPassword ? user : null;
        }

        return user.ValidatePassword(password) ? user : null;
    }

    public async Task AddUser(string username, string password, string name)
    {
        if (username == authConfig.AdminUsername)
        {
            return;
        }

        await using var trx = await context.Database.BeginTransactionAsync();

        var duplicates = from u in context.User
            where u.Username == username
            select u;
        if (duplicates.Any())
        {
            return;
        }

        var user = new User
        {
            Name = name,
            Username = username
        };
        user.ChangePassword(password);

        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
        await trx.CommitAsync();
    }

    public async Task ChangePassword(string username, string newPassword)
    {
        if (username == authConfig.AdminUsername)
        {
            return;
        }

        var user = await GetUserByUsername(username);
        user?.ChangePassword(newPassword);
        await context.SaveChangesAsync();
    }

    public string GetRole(User user)
    {
        return user.Username == authConfig.AdminUsername ? "Admin" : "User";
    }

    private async Task<User?> GetUserByUsername(string username)
    {
        if (username == authConfig.AdminUsername)
        {
            return new User
            {
                Name = authConfig.AdminUsername,
                Username = authConfig.AdminUsername
            };
        }

        var q = from u in context.User
            where u.Username == username
            select u;

        return await q.FirstOrDefaultAsync();
    }
}