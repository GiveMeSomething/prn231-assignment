using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<ClassFile>? ClassFiles { get; set; }
        public IList<User>? Members { get; set; }
    }
}
