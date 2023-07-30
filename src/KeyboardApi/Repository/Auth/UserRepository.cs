using KeyboardApi.Models;

namespace KeyboardApi.Repository.Auth;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User("Andy Peters", "Andy.Peters@test.com", "tester123"),
        new User("Emma Smith", "Emma.Smith@test.com", "tester456")
    };
    
    public User? GetUser(string email)
    {
        return _users.Find(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
    }
}