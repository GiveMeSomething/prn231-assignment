using BusinessObject.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class ClassInputDto
    {
        [Required(ErrorMessage = "Name must not be null")]
        [RegularExpression(@".*[^s].*", ErrorMessage = "Name can not consist of only white spaces")]
        public string Name { get; set; }
    }
}
