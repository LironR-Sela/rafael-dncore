using System.Threading.Tasks;
using day2efcoredemo.Infra;
using day2efcoredemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace day2efcoredemo.Controllers
{
    // [Authorize(Roles = "admin")]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _userService.Login(req);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userId = HttpContext.User.Claims.First().Value;
           return Ok(await _userService.GetAll());
        }
    }
}