using BusinessObject.Models;

namespace WebAPI.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<UserDto>? Members { get; set; }
    }
}
