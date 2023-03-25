using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using WebAPI.Base.Jwt;

namespace WebAPI.Base.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AssignmentPRNContext _dbContext;
        private User _user;

        public UserContextService(IHttpContextAccessor context, AssignmentPRNContext dbContext)
        {
            _contextAccessor = context;
            _dbContext = dbContext;
        }

        public User? GetUser()
        {
            if (_user != null)
            {
                return _user;
            }

            var sToken = _contextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var token = CustomJwt.ReadToken(sToken);
            if (token == null)
            {
                return null;
            }

            var sUserId = token.Claims.Where(c => c.Type == "userId").First().Value;
            var userId = int.Parse(sUserId);
            _user = _dbContext.Users.Include(u => u.Classes).Where(u => u.Id == userId).FirstOrDefault();
            return _user;
        }
    }
}
