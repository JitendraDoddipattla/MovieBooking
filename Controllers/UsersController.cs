using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MovieBooking.Data;
using MovieBooking.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UsersController(MovieDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
           var userWithSameEmail = _dbContext.Users.Where(u=>u.Email==user.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                return BadRequest("user With this Email already exists");
            }
            var userobj = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = "User"
            };

            _dbContext.Users.Add(userobj);
            _dbContext.SaveChanges();
            return Ok(userobj);
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user) 
        {
            var userEmail = _dbContext.Users.FirstOrDefault(u=>u.Email == user.Email);
            if(userEmail == null)
            {
                return NotFound();
            }
            if (user.Password!= userEmail.Password)
            {
                return Unauthorized();
            }
            var claims = new[]
             {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role, userEmail.Role)
             };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_Id = userEmail.Id,
            });
        }
    }
}
