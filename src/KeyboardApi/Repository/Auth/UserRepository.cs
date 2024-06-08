using Keyboard.Common.Models;
using Keyboard.Common.Models.Config;

namespace KeyboardApi.Repository.Auth;

public class UserRepository : IUserRepository
{
    private readonly SeedUsers _seedUsers;
    
    public UserRepository(SeedUsers seedUsers)
    {
        _seedUsers = seedUsers;
    }
    
    public User? GetUser(string email)
    {
        return _seedUsers.Find(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
    }
}