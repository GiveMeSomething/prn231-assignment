using BusinessObject.Models;

namespace WebAPI.Auth.Services
{
    public interface IUserContextService
    {
        public User? GetUser();
    }
}
