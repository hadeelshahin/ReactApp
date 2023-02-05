using API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsShared.Models;
using ModelsShared.ViewModels;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public UsersController(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        [HttpPost("Login")]
        public ActionResult<Users> Login(LoginVM login)
        {
            try
            {
                var User = _dbContext.Users.Where(c => c.Email.Equals(login.Email.Trim())).SingleOrDefault();
                if (User != null)
                {
                    var valid = User.Password.Equals(login.Password);
                    if (valid) return User;
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Password is not correct");
                }
                return StatusCode(StatusCodes.Status406NotAcceptable, "Your email not registered yet, please go to sign up page");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        [HttpPost("GetResult/{result}")]
        public ActionResult<bool> GetResult(bool result)
        {
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<Users> GetUsers(Guid id)
        {
            var User = _dbContext.Users.Where(c=>c.Id == id).SingleOrDefault();
            if (User != null)
            {
                return User;
            }
            return StatusCode(StatusCodes.Status406NotAcceptable, "Not Found User, please go to sign up page");
        }
        [HttpPost("UpdateProfile")]
        public async Task<bool> UpdateProfile(Users user)
        {
            _dbContext.Users.Update(user);
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<bool>> Register(RegisterVM register)
        {
            try
            {
                if (register.Email == null) return false;
                if (register.Password == null) return false;

                var user = new Users
                {
                    Id = Guid.NewGuid(),
                    FullName = register.FullName,
                    Email = register.Email,
                    EmailConfirmed = false,
                    Gender = register.Gender,
                    Password = register.Password,
                    UserType = register.UserType,
                };
                var isUserExist = _dbContext.Users.Any(c => c.Email.ToLower().Contains(register.Email.ToLower()));
                if (!isUserExist)
                {
                    _dbContext.Users.Add(user);
                    var saved = await _dbContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Your email is registered");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

    }
}
