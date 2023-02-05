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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Quizs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizVM>>> GetQuiz()
        {
            return await _context.Quiz.Include(q => q.Answers).Select(q => new QuizVM
            {
                Id = q.Id,
                Question = q.Question,
                QuizListId = q.QuizListId,
                Answers = q.Answers.Select(a => new ModelsShared.ViewModels.AnswersVM
                {
                    QuizId = a.QuizId,
                    Id = a.Id,
                    Answer = a.Answer,
                    IsAnswerTrue = a.IsAnswerTrue,
                }).ToList()
            }).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<QuizVM>>> GetQuiz(Guid id)
        {
            return await _context.Quiz.Include(q => q.Answers).Where(c => c.QuizListId == id).Select(q => new QuizVM
            {
				Id = q.Id,
				Question = q.Question,
                QuizListId = q.QuizListId,
                Answers = q.Answers.Select(a => new ModelsShared.ViewModels.AnswersVM
                {
                    QuizId = a.QuizId,
                    Id = a.Id,
                    Answer = a.Answer,
                    IsAnswerTrue = a.IsAnswerTrue,
                }).ToList()
            }).ToListAsync();
        }

        //// GET: api/Quizs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Quiz>> GetQuiz(Guid id)
        //{
        //    var quiz = await _context.Quiz.FindAsync(id);

        //    if (quiz == null)
        //    {
        //        return NotFound();
        //    }

        //    return quiz;
        //}

        // PUT: api/Quizs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(Guid id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return BadRequest();
            }

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // POST: api/Quizs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizListVM>> PostQuiz(List<QuizVM> quiz)
        {
            try
            {
                var models = quiz.Select(q => new Quiz
                {
                    Id = q.Id,
                    QuizListId = q.QuizListId,
                    Answers = q.Answers.Select(a => new Answers
                    {
                        Answer = a.Answer,
                        IsAnswerTrue = a.IsAnswerTrue,
                        QuizId = a.QuizId
                    }).ToList(),
                    Question = q.Question,
                });
                _context.Quiz.AddRange(models);
                await _context.SaveChangesAsync();
                return Ok(_context.QuizList.Where(c=> c.Id == models.FirstOrDefault().QuizListId).Select(c=> new QuizListVM { IdCode = c.IdCode, Id = c.Id}).SingleOrDefault());
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // DELETE: api/Quizs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quiz.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(Guid id)
        {
            return _context.Quiz.Any(e => e.Id == id);
        }
    }
}
