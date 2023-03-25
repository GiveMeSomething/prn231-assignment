using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;
using WebAPI.Services;
using WebAPI.DTOs.Resource;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseGuard(typeof(AuthGuard))]
    [UseGuard(typeof(RoleGuard))]
    public class ResourceController : ControllerBase
    {
        private readonly AssignmentPRNContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ResourceController(AssignmentPRNContext context, IUserService service, IMapper mapper)
        {
            _context = context;
            _userService = service;
            _mapper = mapper;
        }

        [HttpGet("{resourceId}")]
        public IActionResult GetResource(int resourceId)
        {
            if (!AuthorizeUser(resourceId: resourceId))
            {
                return Unauthorized();
            }

            var cFile = _context.ClassFiles.Where(f => f.Id == resourceId).FirstOrDefault();
            var content = System.IO.File.ReadAllBytes(cFile.FilePath);

            return File(content, cFile.ContentType, cFile.Name);
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult> GetClassFiles(int classId)
        {
            if (await _context.Classes.FindAsync(classId) == null)
            {
                return NotFound();
            }

            if (!AuthorizeUser(classId: classId))
            {
                return Unauthorized();
            }

            var list = _context.Classes.Where(c => c.Id == classId).FirstOrDefault().ClassFiles.ToList();
            list.ForEach(f => f.Class = null);
            return Ok(_mapper.Map<List<ClassFileDto>>(list));
        }

        [HttpPost("class/{classId}")]
        [Roles(Role.Teacher, Role.Admin)]
        public async Task<IActionResult> UploadClassFiles([FromRoute] int classId, IFormFile fFile, string? name)
        {
            //Check inputs
            if (await _context.Classes.FindAsync(classId) == null)
            {
                return NotFound($"class with id {classId} not found");
            }
            if (fFile == null)
            {
                return BadRequest("file not submitted");
            }

            if (!AuthorizeUser(classId: classId))
            {
                return Unauthorized();
            }

            if (name == null)
            {
                name = fFile.FileName;
            }
            var path = GetStoragePath(classId, name);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fFile.CopyToAsync(stream);
            }

            var existingFile = await _context.ClassFiles.Where(f => f.FilePath == path).FirstOrDefaultAsync();

            if (existingFile != null)
            {
                existingFile.ContentType = fFile.ContentType;
                _context.Attach(existingFile);
                _context.SaveChanges();
                return Ok(new
                {
                    message = "A file with similar name already existed. The file was overwritten.",
                    file = _mapper.Map<ClassFileDto>(existingFile)
                });
            }
            else
            {
                var file = new ClassFile()
                {
                    Name = name,
                    ContentType = fFile.ContentType,
                    FilePath = path,
                    ClassId = classId
                };
                _context.Add(file);
                _context.SaveChanges();
                file.Class = null;
                return Ok(new { file = _mapper.Map<ClassFileDto>(file) });
            }
        }

        [HttpDelete("{resourceId}")]
        [Roles(Role.Teacher, Role.Admin)]
        public async Task<IActionResult> DeleteResource(int resourceId)
        {
            var resource = await _context.ClassFiles.FindAsync(resourceId);
            if (resource == null)
            {
                return NotFound();
            }

            if (!AuthorizeUser(resourceId: resourceId))
            {
                return Unauthorized();
            }

            try
            {
                System.IO.File.Delete(resource.FilePath);
                _context.Remove(resource);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private bool AuthorizeUser(int? classId = null, int? resourceId = null)
        {
            var userJwt = Request.GetUserJwt();
            if (userJwt == null)
            {
                return false;
            }

            var user = _context.Users
                .Include(u => u.Classes)
                .ThenInclude(c => c.ClassFiles)
                .FirstOrDefault(u => u.Id == userJwt.UserId);

            if (user == null)
            {
                return false;
            }

            if (classId != null && !user.Classes.Any(c => c.Id == classId))
            {
                return false;
            }

            if (resourceId != null && !user.Classes.Any(c => c.ClassFiles.Any(f => f.Id == resourceId)))
            {
                return false;
            }

            return true;
        }

        private static string GetStoragePath(int classId, string fileName)
            => Path.Combine(Directory.GetCurrentDirectory(), "UserFiles", "class-" + classId,  fileName);
    }
}
