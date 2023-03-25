using BusinessObject.Models;

namespace WebAPI.AutoMapper.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String Role { get; set; }
        public string Email { get; set; }
    }
}
