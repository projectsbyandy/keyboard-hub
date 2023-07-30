using KeyboardApi.Models;

namespace KeyboardApi.Repository.Auth;

public interface IUserRepository
{
    public User? GetUser(string name);
}