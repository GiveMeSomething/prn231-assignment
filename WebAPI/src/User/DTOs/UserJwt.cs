using BusinessObject.Models;

namespace WebAPI.DTOs
{
	public class UserJwt
	{
		public int UserId { get; set; }

		public Role Role { get; set; }
	}
}

