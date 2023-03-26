using BusinessObject.Models;

namespace WebAPI.DTOs
{
    /// <summary>
    /// UserDto with password
    /// </summary>
    public class UserDto2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
