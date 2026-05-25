using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace KE03_INTDEV_SE_1_Base.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            AccountDAL accountDAL = new AccountDAL();

            bool success = accountDAL.LoginByUsernameAndPassword(
                request.Username,
                request.Password
            );

            return Ok(new
            {
                success = success
            });
        }

        [HttpGet("profile/{username}")]
        public IActionResult GetProfile(string username)
        {
            CustomerDAL customerDAL = new CustomerDAL();

            CustomerProfile? profile = customerDAL.GetCustomerProfileByUsername(username);

            if (profile == null)
            {
                return NotFound(new { message = "Profiel niet gevonden" });
            }

            return Ok(profile);
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] CustomerProfile profile)
        {
            CustomerDAL customerDAL = new CustomerDAL();

            bool success = customerDAL.UpdateCustomerProfile(profile);

            return Ok(new
            {
                success = success
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}