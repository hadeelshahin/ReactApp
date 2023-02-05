using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using API.Helper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet("GetClassesById/{id}")]
        public async Task<ActionResult<IEnumerable<ClassesVM>>> GetClassesById(Guid id)
        {
            var result = await _context.Classes.Where(c => c.TeacherId == id).OrderBy(o => o.CreatedDate).Select(c => new ClassesVM
            {
                ClassName = c.ClassName,
                EndTime = c.EndTime,
                Id = c.Id,
                IdCode = c.IdCode,
                NumberOfStudents = c.Students.Count(),
                StartTime = c.StartTime,
            }).ToListAsync();
            return result;
        }

        [HttpGet("GetCountOfStudents/{id}")]
        public async Task<ActionResult<int>> GetCountOfStudents(Guid id)
        {
            var classes = await _context.Classes.Include(c => c.Students).Where(c=>c.Id == id).SingleOrDefaultAsync();

            if (classes == null)
            {
                return NotFound();
            }

            return classes.Students?.Count ?? 0;
        }

        [HttpGet("GetStudents/{id}")]
        public async Task<ActionResult<IEnumerable<Users>>> GetStudents(Guid id)
        {
            try
            {
                var classes = await _context.Classes.Include("Students.User").Where(c=>c.Id == id).SingleOrDefaultAsync();

				if (classes == null)
				{
					return NotFound();
				}
                var students = classes.Students.Select(c => c.User).ToList();
				return students;
			}
			catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("CheckUsersClass/{id}/{classId}")]
        public ActionResult<bool> CheckUsersClass(Guid id, Guid classId)
        {
            return _context.UsersClass.Any(c => c.UserId == id && c.ClassId == classId);
        }

        [HttpGet("AddUsersClass/{userId}/{classId}")]
        public async Task<ActionResult<bool>> AddUsersClass(Guid userId, Guid classId)
        {
            var isUserClassExist = _context.UsersClass.Where(c => c.UserId == userId && c.ClassId == classId).SingleOrDefault();
            if (isUserClassExist == null)
            {
                _context.UsersClass.Add(new UsersClass { UserId = userId, ClassId = classId });
			    var result = await _context.SaveChangesAsync();
			    return result > 0;
            }
            else
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "User is already joined in this class");
            }
        }
        [HttpGet("AddUsersClassByCode/{userId}/{code}")]
        public async Task<ActionResult<bool>> AddUsersClassByCode(Guid userId, string code)
        {
            var classes = await _context.Classes.Where(c => c.IdCode.Equals(code)).FirstOrDefaultAsync();
            if (classes == null)
            {
                return NotFound();
            }
            var isUserClassExist = _context.UsersClass.Where(c => c.UserId == userId && c.ClassId == classes.Id).SingleOrDefault();
            if (isUserClassExist == null)
            {
                _context.UsersClass.Add(new UsersClass { UserId = userId, ClassId = classes.Id });
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            else
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "User is already joined in this class");
            }
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classes>> GetClasses(Guid id)
        {
            var classes = await _context.Classes.FindAsync(id);

            if (classes == null)
            {
                return NotFound();
            }

            return classes;
        }

        // PUT: api/Classes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClasses(Guid id, Classes classes)
        {
            if (id != classes.Id)
            {
                return BadRequest();
            }

            _context.Entry(classes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Classes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Classes>> PostClasses(ClassesVM classes)
        {
            var id = Guid.NewGuid();
            bool IsExistIdCode = false;
            string idCode = string.Empty;
            do
            {
                idCode = GenerateCode.RandomString(6);
                IsExistIdCode = _context.Classes.Any(c => c.IdCode == idCode);

            } while (IsExistIdCode);
            var model = new Classes 
            {
                Id = id,
                ClassName = classes.ClassName,
                StartTime = classes.StartTime,
                EndTime = classes.EndTime,
                IdCode = idCode,
                TeacherId = classes.TeacherId,
                CreatedDate = DateTime.Now,
            };
            _context.Classes.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClasses(Guid id)
        {
            var classes = await _context.Classes.FindAsync(id);
            if (classes == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(classes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassesExists(Guid id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
