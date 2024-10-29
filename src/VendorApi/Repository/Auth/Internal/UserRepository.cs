using VendorApi.Models.Auth;
using VendorApi.Models.Config;

namespace VendorApi.Repository.Auth.Internal;

internal class UserRepository(SeedUsers seedUsers) : IUserRepository
{
    public User? GetUser(string email)
    {
        return seedUsers.Find(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
    }
}