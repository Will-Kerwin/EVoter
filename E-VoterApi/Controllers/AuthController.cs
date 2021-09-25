using E_VoterApi.Models;
using E_VoterApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_VoterApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository Auth;

        public AuthController()
        {
            Auth = new AuthRepository();
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            throw new NotImplementedException();
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyToken()
        {
            throw new NotImplementedException();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel body)
        {
            var data = await Auth.VerifyUserCredentials(body);
            if (data.verified)
            {
                var user = await Auth.GetUserDetails(data.userID);
                if (user.success)
                    return Ok(user.user);
                else
                    return Unauthorized();
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] UserDetails newUser)
        {
            var registerUser = await Auth.RegisterUser(newUser);
            if (registerUser.success)
            {
                newUser.password = "";
                return Ok();
            }
            else
            {
                return BadRequest("user was not created");
            }
        }
    }
}
