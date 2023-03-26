using AutoMapper;
using BusinessObject.Models;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth;
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
    public class ResourceController : ControllerBase
    {
        private readonly StorageClient _firebaseStorage;
        private readonly AssignmentPRNContext _context;
        private readonly IMapper _mapper;


        public ResourceController(
            AssignmentPRNContext context,
            IMapper mapper,
            StorageClient storageClient
        )
        {
            _context = context;
            _mapper = mapper;
            _firebaseStorage = storageClient;
        }

        [HttpGet("{resourceId}")]
        public async Task<IActionResult> GetResource(int resourceId)
        {
            var cFile = _context.ClassFiles.Where(f => f.Id == resourceId).FirstOrDefault();
            if (cFile == null)
            {
                return NotFound();
            }

            if (!AuthorizeUser(resourceId: resourceId))
            {
                return Unauthorized();
            }

            using (var stream = new MemoryStream())
            {
                await _firebaseStorage.DownloadObjectAsync(Resource.BucketName, Resource.GetStoragePath(cFile.Id), stream);

                // Set the position of the stream to the beginning
                stream.Seek(0, SeekOrigin.Begin);

                var reader = new BinaryReader(stream);
                var content = reader.ReadBytes((int)stream.Length);
                return File(content, cFile.ContentType, cFile.Name);
            }

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

            var list = _context.Classes.Include(c => c.ClassFiles)
                .Where(c => c.Id == classId).FirstOrDefault().ClassFiles.ToList();
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

            var existingFile = await _context.ClassFiles
                .Where(f => f.ClassId == classId && f.Name == name)
                .FirstOrDefaultAsync();

            string path = "";

            if (existingFile != null)
            {
                existingFile.ContentType = fFile.ContentType;
                _context.Attach(existingFile);
                _context.SaveChanges();
                using (var stream = fFile.OpenReadStream())
                {
                    await _firebaseStorage.UploadObjectAsync(Resource.BucketName, Resource.GetStoragePath(existingFile.Id), null, stream);
                }
                return Ok(new
                {
                    message = "A file with similar name already existed. The file will be overwritten.",
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
                using (var stream = fFile.OpenReadStream())
                {
                    await _firebaseStorage.UploadObjectAsync(Resource.BucketName, Resource.GetStoragePath(file.Id), null, stream);
                }
                file.Class = null;
                return Ok(new { file = _mapper.Map<ClassFileDto>(file) });
            }

            //var url = $"https://storage.googleapis.com/{BucketName}/{objectName}";
            //return Ok(url);
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

            if (user.Role == Role.Admin)
            {
                return true;
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
    }
}
