using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public IUserService _UserService;
        public IJwtService _jwtService;
        public IMapper _mapper;


        public AuthController(ILogger<AuthController> logger, IUserService userService,IJwtService jwtService, IMapper mapper)
        {
            _jwtService = jwtService;
            _logger = logger;
            _UserService = userService;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserViewModel registerVM)
        {
            try
            {
                bool checkUserExist = await _UserService.CheckUserNameExists(registerVM.Username);
                if (checkUserExist == true)
                    return BadRequest("Username already exists");

                if (!ModelState.IsValid)
                {
                    var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                }
                
                if (registerVM != null)
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerVM.Password);
                    
                    // Map RegisterViewModel to UserViewModel
                    var userVM = _mapper.Map<UserViewModel>(registerVM);

                    // Enrich unmapped fields
                    userVM.Id = Guid.NewGuid();
                    userVM.PasswordHash = passwordHash;
                    
                    // Map to ViewModel
                    var viewModel = _mapper.Map<UserViewModel>(userVM);
                    
                    //Save user details to register
                    viewModel = _UserService.AddUserDetails (userVM);
                }
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to Register the user" + ex.Message);
            }            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel userVM)
        {
            try
            {                
                var user = await _UserService.GetUserDetails(userVM.Username);
                if (user == null)
                {
                    _logger.LogWarning("User not found for User: {Username}", userVM.Username);
                    return Unauthorized("Invalid credentials");
                }
                else if (!BCrypt.Net.BCrypt.Verify(userVM.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid password for User: {Username}", userVM.Username);
                    return Unauthorized("Invalid credentials");
                }
                else if (userVM.Email!= user.Email)
                {
                    _logger.LogWarning("Invalid Email for User: {Username}", userVM.Username);
                    return Unauthorized("Invalid credentials");
                }

                var token = _jwtService.GenerateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthController/Login failed with an exception: " + ex.Message);
                return BadRequest();
            }

        }
    }

}
