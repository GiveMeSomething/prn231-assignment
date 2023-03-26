using BusinessObject.Models;

namespace FAPortal.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<UserDto>? Members { get; set; }
    }
}
