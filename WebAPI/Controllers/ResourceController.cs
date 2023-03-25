using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAPI.Auth.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly AssignmentPRNContext _dbContext;
        private readonly IUserContextService _userService;
        private readonly IMapper _mapper;

        public ResourceController(AssignmentPRNContext context, IUserContextService service, IMapper mapper)
        {
            _dbContext = context;
            _userService = service;
            _mapper = mapper;
        }

        [HttpGet("{resourceId}")]
        public IActionResult GetResource(int resourceId)
        {
            if (!AuthorizeUser(false, resourceId: resourceId))
            {
                return Unauthorized();
            }

            var cFile = _dbContext.ClassFiles.Where(f => f.Id == resourceId).FirstOrDefault();
            var content = System.IO.File.ReadAllBytes(cFile.FilePath);

            return File(content, cFile.ContentType, cFile.Name);
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult> GetClassFiles(int classId)
        {
            if (await _dbContext.Classes.FindAsync(classId) == null)
            {
                return NotFound();
            }

            if (!AuthorizeUser(false, classId: classId))
            {
                return Unauthorized();
            }

            var list = _dbContext.Classes.Where(c => c.Id == classId).FirstOrDefault().ClassFiles.ToList();
            list.ForEach(f => f.Class = null);
            return Ok(_mapper.Map<List<ClassFileDto>>(list));
        }

        [HttpPost("class/{classId}")]
        public async Task<IActionResult> PostClassFiles([FromRoute] int classId, IFormFile fFile, string? name)
        {
            //Check inputs
            if (await _dbContext.Classes.FindAsync(classId) == null)
            {
                return NotFound($"class with id {classId} not found");
            }
            if (fFile == null || fFile.Length == 0)
            {
                return BadRequest("file not submitted");
            }

            //Authorize
            var user = _userService.GetUser();
            if (!AuthorizeUser(true, classId: classId))
            {
                return Unauthorized();
            }

            if (name == null)
            {
                name = fFile.FileName;
            }
            var path = GetStoragePath(name);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fFile.CopyToAsync(stream);
            }

            var existingFile = await _dbContext.ClassFiles.Where(f => f.FilePath == path).FirstOrDefaultAsync();

            if (existingFile != null)
            {
                existingFile.ContentType = fFile.ContentType;
                _dbContext.Attach(existingFile);
                _dbContext.SaveChanges();
                return Ok(new
                {
                    message = "A file with similar name already existed. The file was overriden.",
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
                _dbContext.Add(file);
                _dbContext.SaveChanges();
                file.Class = null;
                return Ok(new { file = _mapper.Map<ClassFileDto>(file) });
            }
        }

        [HttpDelete("{resourceId}")]
        public async Task<IActionResult> DeleteResource(int resourceId)
        {
            var resource = await _dbContext.ClassFiles.FindAsync(resourceId);
            if (resource == null)
            {
                return NotFound();
            }

            if (!AuthorizeUser(true, resourceId: resourceId))
            {
                return Unauthorized();
            }

            try
            {
                System.IO.File.Delete(resource.FilePath);
                _dbContext.Remove(resource);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private bool AuthorizeUser(bool isModify, int? classId = null, int? resourceId = null)
        {
            var user = _userService.GetUser();
            if (user == null)
            {
                return false;
            }

            if (isModify && user.Role != Role.Teacher && user.Role != Role.Admin)
            {
                return false;
            }

            _dbContext.Classes.Include(c => c.ClassFiles).ToList();

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

        private string GetStoragePath(string fileName) => Path.Combine(Directory.GetCurrentDirectory(), "UserFiles", fileName);
    }
}
