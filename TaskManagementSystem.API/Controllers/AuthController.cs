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
        
        public AuthController(ILogger<AuthController> logger, IUserService userService,IJwtService jwtService)
        {
            _jwtService = jwtService;
            _logger = logger;
            _UserService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserViewModel userVM)
        {
            try
            {
                bool checkUserExist = await _UserService.CheckUserNameExists(userVM.Username);
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

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(userVM.Password);
                var userViewModel = new UserViewModel();
                if (userVM != null)
                {
                    //Add details to VM
                    userViewModel.Id = new Guid();
                    userViewModel.Username = userVM.Username;
                    userViewModel.Email = userVM.Email;
                    userViewModel.PasswordHash = passwordHash;

                    //Save user details to register
                    userViewModel = _UserService.AddUserDetails (userViewModel);
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
