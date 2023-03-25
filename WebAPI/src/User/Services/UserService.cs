using System;
using BusinessObject.Models;

namespace WebAPI.Services
{
	public class UserService: IUserService
	{
        private readonly ILogger<UserService> _logger;
        private readonly AssignmentPRNContext _context;

		public UserService(ILogger<UserService> logger, AssignmentPRNContext context)
		{
            _logger = logger;
            _context = context;
		}

        public User? GetUserById(int id)
        {
            var foundUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if(foundUser == null)
            {
                _logger.LogWarning("User with id: {id} not found", id);
                return null;
            }

            return foundUser;
        }
    }
}

