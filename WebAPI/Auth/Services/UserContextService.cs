using BusinessObject.Models;
using Microsoft.Net.Http.Headers;
using WebAPI.Auth.Jwt;

namespace WebAPI.Auth.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AssignmentPRNContext _dbContext;

        public UserContextService(IHttpContextAccessor context, AssignmentPRNContext dbContext)
        {
            _contextAccessor = context;
            _dbContext = dbContext;
        }

        public User? GetUser()
        {
            var sToken = _contextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var token = CustomJwt.ReadToken(sToken);
            if (token == null)
            {
                return null;
            }

            var sUserId = token.Claims.Where(c => c.Type == "userId").First().Value;
            var userId = int.Parse(sUserId);
            return _dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
        }
    }
}
