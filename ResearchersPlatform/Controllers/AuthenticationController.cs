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
        private readonly IStudentRepository _studentRepository; 
        public AuthenticationController(
            UserManager<Student> userManager, IAuthService authService, IMapper mapper, IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _authService = authService;
            _mapper = mapper;
            _studentRepository = studentRepository;
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
            var student = await _userManager.FindByNameAsync(user.UserName!);
            _studentRepository.CreateTrails(studentId: student!.Id);
            return Ok(
                new
                {
                    UserId = await _userManager.GetUserIdAsync(student!)
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
        [HttpPost("AdminLogin")]

        public async Task<IActionResult> AuthenticateToAdmin([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
            {
                return Unauthorized();
            }


            var admin = await _userManager.FindByEmailAsync(user.Email!);
            var useradmin = await _userManager.IsInRoleAsync(admin!, "Admin");
            if (!useradmin)
                return NotFound();
            return Ok(
            new
            {
                Token = await _authService.CreateToken(),
                UserId = await _userManager.GetUserIdAsync(admin!)
            }
            );
        }
    }
}
