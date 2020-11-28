using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Users
{
    public class User
    {
        public long Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Username { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
    }
}