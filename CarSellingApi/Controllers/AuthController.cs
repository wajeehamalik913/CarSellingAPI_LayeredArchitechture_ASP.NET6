using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarSellingAPI.Business.Interfaces;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using CarSellingAPI.DAL.Models;

namespace CarSellingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _Authenticate;

        //Constructor to initialize interface
        public AuthController(IAuth Authenticate)
        {
            _Authenticate = Authenticate;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {


            //checking if user exists
            var userExists = await _Authenticate.Register(registerVM);
            if (userExists == null)
            {
                return BadRequest("User could not be created");
            }
            //retrning success 
            return Ok("User created");
        }
        //POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {


            var tokenValue = await _Authenticate.Login(loginVM);
            //checking if Token is generated and valid
            if (tokenValue == null)
            {
                return Unauthorized();
            }
            return Ok(tokenValue);

        }

        //POST: api/Auth/refreshToken
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM)
        {

            var tokenValue = await _Authenticate.RefreshToken(refreshTokenVM);
            return Ok(tokenValue);

        }
    }
}
