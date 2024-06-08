using Keyboard.Common.Models;

namespace KeyboardApi.Repository.Auth;

public interface IUserRepository
{
    public User? GetUser(string name);
}