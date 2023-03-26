using AutoMapper;
using BusinessObject.Models;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth;
using WebAPI.AutoMapper.Models;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;
using WebAPI.DTOs;
using WebAPI.src.Resource;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseGuard(typeof(AuthGuard))]
    [UseGuard(typeof(RoleGuard))]
    public class ClassController : ControllerBase
    {
        private readonly AssignmentPRNContext _context;
        private readonly IMapper _mapper;
        private readonly StorageClient _firebaseStorage;

        public ClassController(
            AssignmentPRNContext context,
            IMapper mapper,
            StorageClient client)
        {
            _context = context;
            _mapper = mapper;
            _firebaseStorage = client;
        }

        [HttpGet]
        public IActionResult GetClassList()
        {
            var classList = _context.Classes.Include(c => c.Members).ToList();
            return Ok(_mapper.Map<List<ClassDto>>(classList));
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetClass(int classId)
        {
            var c = await _context.Classes.Include(c => c.Members)
                .Where(c => c.Id == classId).FirstOrDefaultAsync();
            if (c == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClassDto>(c));
        }

        [HttpPost("class")]
        [Roles(Role.Admin)]
        public IActionResult CreateClass(ClassInputDto classInputDto)
        {
            var c = new Class
            {
                Name = classInputDto.Name
            };
            _context.Add(c);
            _context.SaveChanges();
            return Ok(_mapper.Map<ClassDto>(c));
        }

        [HttpPut("class/{classId}")]
        [Roles(Role.Admin)]
        public async Task<IActionResult> UpdateClass(int classId, ClassInputDto classInputDto)
        {
            var c = await _context.Classes.Include(c => c.Members).FirstOrDefaultAsync(c => c.Id == classId);
            if (c == null)
            {
                return NotFound();
            }

            c.Name = classInputDto.Name;
            _context.Entry(c).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(_mapper.Map<ClassDto>(c));
        }

        [HttpDelete("class/{classId}")]
        [Roles(Role.Admin)]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var c = await _context.Classes.Include(c => c.Members)
                .Include(c => c.ClassFiles).FirstOrDefaultAsync(c => c.Id == classId);
            if (c == null)
            {
                return NotFound();
            }

            foreach (var f in c.ClassFiles)
            {
                _firebaseStorage.DeleteObjectAsync(Resource.BucketName, Resource.GetStoragePath(f.Id));
            };
            c.ClassFiles = null;
            c.Members = null;
            _context.Remove(c);
            _context.SaveChanges();
            return Ok(_mapper.Map<ClassDto>(c));
        }

        [HttpPatch("class/{classId}/{userId}")]
        [Roles(Role.Admin)]
        public async Task<IActionResult> AddUser(int classId, int userId)
        {
            var c = await _context.Classes.Include(c => c.Members)
                .Where(c => c.Id == classId).FirstOrDefaultAsync();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == userId);
            if (c == null || user == null)
            {
                return NotFound();
            }

            if (c.Members.Contains(user))
            {
                return Conflict($"User {userId} is already a member of class {classId} ");
            }

            c.Members.Add(user);
            _context.SaveChanges();
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpDelete("class/{classId}/{userId}")]
        [Roles(Role.Admin)]
        public async Task<IActionResult> DeleteUser(int classId, int userId)
        {
            var c = await _context.Classes.Include(c => c.Members)
                .Where(c => c.Id == classId).FirstOrDefaultAsync();
            if (c == null)
            {
                return NotFound();
            }

            var user = c.Members.FirstOrDefault(m => m.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            c.Members.Remove(user);
            _context.SaveChanges();
            return Ok(_mapper.Map<ClassDto>(c));
        }
    }
}
