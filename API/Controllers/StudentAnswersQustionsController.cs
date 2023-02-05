using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using ModelsShared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAnswersQustionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentAnswersQustionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentAnswersQustions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentAnswersQustions>>> GetStudentAnswersQustions()
        {
            return await _context.StudentAnswersQustions.Include(c => c.User).Include(c => c.QuizList).ToListAsync();
        }

        // GET: api/StudentAnswersQustions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<StudentAnswersQustions>>> GetStudentAnswersQustions(Guid id)
        {
            var studentAnswersQustions = await _context.StudentAnswersQustions.Include(c => c.User).Include(c => c.QuizList).Include("StudentQustion.Qustion").Include("StudentQustion.StudentAnswer.Answer").Where(c => c.QuizListId == id).ToListAsync();

            if (studentAnswersQustions == null)
            {
                return NotFound();
            }

            return studentAnswersQustions;
        }
        [HttpGet("GetQuizForStudent/{id}")]
        public async Task<ActionResult<List<QuizList>>> GetQuizForStudent(Guid id)
        {
            var studentAnswersQustions = await _context.StudentAnswersQustions.Include(c => c.QuizList).Where(c => c.UserId == id).ToListAsync();

            if (studentAnswersQustions == null)
            {
                return NotFound();
            }

            return studentAnswersQustions.Select(c=>c.QuizList).ToList();
        }
        [HttpGet("CheckStudentTakeQuiz/{quizId}/{userId}")]
        public ActionResult<bool> CheckStudentTakeQuiz(Guid quizId, Guid userId)
        {
            return _context.StudentAnswersQustions.Include(c => c.QuizList).Any(c => c.UserId == userId && c.QuizListId == quizId);
        }

        // PUT: api/StudentAnswersQustions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentAnswersQustions(Guid id, StudentAnswersQustions studentAnswersQustions)
        {
            if (id != studentAnswersQustions.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentAnswersQustions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentAnswersQustionsExists(id))
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

        // POST: api/StudentAnswersQustions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentAnswersQustions>> PostStudentAnswersQustions(StudentAnswersQustions studentAnswersQustions)
        {
            var countAnswers = _context.StudentAnswersQustions.Count(c => c.QuizListId == studentAnswersQustions.QuizListId) + 1;
            var countStudents = _context.UsersClass.Where(c => c.ClassId == studentAnswersQustions.QuizList.ClassesId).Count();
            studentAnswersQustions.QuizList = null;
            _context.StudentAnswersQustions.Add(studentAnswersQustions);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
				var quizList = await _context.QuizList.FindAsync(studentAnswersQustions.QuizListId);
				if (quizList != null)
				{
					quizList.Result = (countStudents / countAnswers) * 100;
				}
				await _context.SaveChangesAsync();
			}
			return CreatedAtAction("GetStudentAnswersQustions", new { id = studentAnswersQustions.Id }, studentAnswersQustions);
        }

        // DELETE: api/StudentAnswersQustions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAnswersQustions(Guid id)
        {
            var studentAnswersQustions = await _context.StudentAnswersQustions.FindAsync(id);
            if (studentAnswersQustions == null)
            {
                return NotFound();
            }

            _context.StudentAnswersQustions.Remove(studentAnswersQustions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentAnswersQustionsExists(Guid id)
        {
            return _context.StudentAnswersQustions.Any(e => e.Id == id);
        }
    }
}
