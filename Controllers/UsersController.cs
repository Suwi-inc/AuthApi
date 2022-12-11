using AuthTestAPI.Data;
using AuthTestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Cors;

namespace AuthTestAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class UsersController : ControllerBase
    {
        private readonly UserDBContext _dbContext;
        public UsersController(UserDBContext dbContext) => _dbContext = dbContext;

        [HttpGet("Users")]
        public async Task<IEnumerable<User>> Get() => await _dbContext.User.ToListAsync();

       
        [HttpGet("Users/{id}")]
        [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _dbContext.User.FindAsync(id);
            return user == null ? NotFound() : Ok(user);
        }
        [HttpPost("Users")]
        public async Task<IActionResult> Create(User user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = user.Id}, user);
        }
        [HttpPost("Users/Login")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(string Password, string Email )
        {
            bool ok = false;
           
           
           foreach (var user in _dbContext.User)
            {
                if((user.Email == Email) && (user.Password == Password))
                     return Ok(user);


            }

            return NotFound();

        }

        [HttpPut("Users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("Users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = await _dbContext.User.FindAsync(id);
            if (userToDelete == null) return NotFound();
            _dbContext.User.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();

        }
      

    }
}
