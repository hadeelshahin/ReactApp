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
    public class QuizListsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizListsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/QuizLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizListVM>>> GetQuizList()
        {
            return await _context.QuizList.Select(q => new QuizListVM
            {
                Id = q.Id,
                Title = q.Title,
                Subject = q.Subject,
                QuizTimeType = q.QuizTimeType,
                IsMarker = q.IsMarker,
                ClassesId = q.ClassesId,
                CreatedDate = q.CreatedDate,
                FromTime = q.FromTime,
                ToTime = q.ToTime,
                Result = q.Result,
                IdCode = q.IdCode,
                QuizType = q.QuizType
            }).ToListAsync();
        }

        // GET: api/QuizLists/5
        [HttpGet("GetQuizListsByClassId/{id}")]
        public async Task<ActionResult<IEnumerable<QuizListVM>>> GetQuizListsByClassId(Guid id)
        {
			var quizList = await _context.QuizList.Where(c => c.ClassesId == id).Select(q => new QuizListVM
			{
				Id = q.Id,
				Title = q.Title,
				Subject = q.Subject,
				QuizTimeType = q.QuizTimeType,
				IsMarker = q.IsMarker,
				ClassesId = q.ClassesId,
				CreatedDate = q.CreatedDate,
				FromTime = q.FromTime,
				ToTime = q.ToTime,
				Result = q.Result,
				IdCode = q.IdCode,
                QuizType = q.QuizType
			}).ToListAsync();

			if (quizList == null)
            {
                return NotFound();
            }

            return quizList;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizListVM>> GetQuizList(Guid id)
        {
			var quizList = await _context.QuizList.Where(c => c.Id == id).Select(q => new QuizListVM
			{
				Id = q.Id,
				Title = q.Title,
				Subject = q.Subject,
				QuizTimeType = q.QuizTimeType,
				IsMarker = q.IsMarker,
				ClassesId = q.ClassesId,
				CreatedDate = q.CreatedDate,
				FromTime = q.FromTime,
				ToTime = q.ToTime,
				Result = q.Result,
				IdCode = q.IdCode,
                QuizType = q.QuizType
			}).SingleOrDefaultAsync();

			if (quizList == null)
            {
                return NotFound();
            }

            return quizList;
        }
        [HttpGet("GetQuizListByCode/{code}")]
        public async Task<ActionResult<QuizListVM>> GetQuizListByCode(string code)
        {
            var quizList = await _context.QuizList.Where(c => c.IdCode.Equals(code)).Select(q => new QuizListVM
            {
                Id = q.Id,
                Title = q.Title,
                Subject = q.Subject,
                QuizTimeType = q.QuizTimeType,
                IsMarker = q.IsMarker,
                ClassesId = q.ClassesId,
                CreatedDate = q.CreatedDate,
                FromTime = q.FromTime,
                ToTime = q.ToTime,
                Result = q.Result,
                IdCode = q.IdCode,
                QuizType = q.QuizType
            }).SingleOrDefaultAsync();

            if (quizList == null)
            {
                return NotFound();
            }

            return quizList;
        }

        // PUT: api/QuizLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizList(Guid id, QuizList quizList)
        {
            if (id != quizList.Id)
            {
                return BadRequest();
            }

            _context.Entry(quizList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizListExists(id))
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

        // POST: api/QuizLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizListVM>> PostQuizList(QuizListVM quizList)
        {
            bool IsExistIdCode = false;
            string idCode = string.Empty;
            do
            {
                idCode = GenerateCode.RandomString(6);
                IsExistIdCode = _context.Classes.Any(c => c.IdCode == idCode);

            } while (IsExistIdCode);

            var model = new QuizList
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                FromTime = quizList.FromTime,
                ToTime = quizList.ToTime,
                ClassesId = quizList.ClassesId,
                IsMarker = quizList.IsMarker,
                QuizTimeType = quizList.QuizTimeType,
                Subject = quizList.Subject,
                Title = quizList.Title,
                IdCode = idCode,
                QuizType = quizList.QuizType,
            };
            _context.QuizList.Add(model);
            await _context.SaveChangesAsync();
            quizList.Id = model.Id;
            quizList.IdCode = idCode;
            return CreatedAtAction("GetQuizList", new { id = quizList.Id }, quizList);
        }

        // DELETE: api/QuizLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizList(Guid id)
        {
            var quizList = await _context.QuizList.FindAsync(id);
            if (quizList == null)
            {
                return NotFound();
            }

            _context.QuizList.Remove(quizList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizListExists(Guid id)
        {
            return _context.QuizList.Any(e => e.Id == id);
        }
    }
}
