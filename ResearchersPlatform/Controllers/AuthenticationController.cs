using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Student> _userManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthenticationController(
            UserManager<Student> userManager, IAuthService authService, IMapper mapper)
        {
            _userManager = userManager;
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost]

        public async Task<IActionResult> RegisterUser([FromBody] StudentForRegisterDto userForRegistration)
        {
            var user = _mapper.Map<Student>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRoleAsync(user, "Student");
            var Student = await _userManager.FindByNameAsync(user.UserName!);
            return Ok(
                new
                {
                    UserId = await _userManager.GetUserIdAsync(Student!)
                }
                );

        }
        [HttpPost("login")]

        public async Task<IActionResult> Authenticate([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
                 return Unauthorized();
            
            var student = await _userManager.FindByEmailAsync(user.Email!);
            return Ok(
            new
            {
                Token = await _authService.CreateToken(),
                UserId = await _userManager.GetUserIdAsync(student!)
            }
            );
        }
    }
}
