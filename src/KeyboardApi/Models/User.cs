namespace KeyboardApi.Models;

public class User
{
    public User(string fullname, string email, string password)
    {
        Fullname = fullname;
        Email = email;
        Password = password;
    }

    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}