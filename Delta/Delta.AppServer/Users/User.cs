namespace Delta.AppServer.Users;

public class User
{
    public User(string name, string username)
    {
        Name = name;
        Username = username;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }

    public string? Password { get; set; }
    public string? Salt { get; set; }

    public void ChangePassword(string password)
    {
        Salt = PasswordHelper.GenerateSalt();
        Password = PasswordHelper.ComputeSaltedPassword(password, Salt);
    }

    public bool ValidatePassword(string password)
    {
        if (Password == null || Salt == null)
        {
            return false;
        }

        var hashed = PasswordHelper.ComputeSaltedPassword(password, Salt);
        return Password == hashed;
    }
}