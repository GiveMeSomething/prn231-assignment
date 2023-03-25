using BusinessObject.Models;

namespace WebAPI.Base.Services
{
    public interface IUserContextService
    {
        public User? GetUser();
    }
}
