using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private AssignmentPRNContext _context;
        public ResourceController(AssignmentPRNContext context)
        {
            _context = context;
        }

        //[HttpGet("{id}")]
        //public IActionResult GetResource(int id)
        //{
        //    var cFile = _context.ClassFiles.Where(f => f.Id == id).FirstOrDefault();
        //    var file = System.IO.File.Create(cFile.FilePath);
        //    var content = System.IO.File.ReadAllBytes(cFile.FilePath);
        //    return File(content)
        //}
    }
}
