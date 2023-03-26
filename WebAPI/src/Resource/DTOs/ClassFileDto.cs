using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DTOs
{
    public class ClassFileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int ClassId { get; set; }
    }
}
