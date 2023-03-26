using BusinessObject.Models;

namespace FAPortal.DTOs
{
	public class UserJwt
	{
		public int UserId { get; set; }

		public Role Role { get; set; }
	}
}

