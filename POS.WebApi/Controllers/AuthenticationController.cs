using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.Models.DTO;
using POS.API.Models.Entities;
using POS.API.Services;
using POS.API.Services.UserServices;
using POS.API.Models.Validation;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static POS.API.Middlewares.CustomExceptions;

namespace POS.API.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _log;
        private readonly IMapper _mapper;
        private readonly UserService _userServices;
        private readonly TokenServices _tokenService;

        public AuthenticationController(UserService userServices, IMapper mapper, TokenServices tokenService, ILogger<AuthenticationController> logger)
        {
            _userServices = userServices;
            _tokenService = tokenService;
            _mapper = mapper;
            _log = logger;
        }

        [HttpPost("token")]
        public IActionResult GetToken([FromBody] LoginDTO login)
        {
            try
            {
                var token = _tokenService.GenerateToken(login);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _log.LogError($"Error in generating token: {ex.Message}");
                throw new Exception("Error generating token");
            }
        }

        [Authorize]
        [HttpPost("SeedUsers")]
        public async Task<IActionResult> SeedUsers()
        {
            try
            {
                await _userServices.SeedUsers();
                _log.LogInformation("Users added successfully!");
                return Ok("Users seeded");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error while seeding users: {ex.Message}");
                throw; 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                var user = await _userServices.Login(username, password);
                if (user == null)
                {
                    throw new UnauthorizedAccessEx("Invalid credentials");
                }

                var loggedInUser = _mapper.Map<LoginDTO>(user);
                _log.LogInformation("User logged in!");
                return Ok(GetToken(loggedInUser));
            }
            catch (UnauthorizedAccessEx ex)
            {
                _log.LogError($"Error during login: {ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error while logging in: {ex.Message}");
                throw; 
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.name) || string.Equals(user.name, "string"))
                {
                    throw new ValidationException("Invalid Name");
                }

                if (string.IsNullOrEmpty(user.email) || string.Equals(user.email, "string") || !EmailValidation.IsEmailValid(user.email))
                {
                    throw new ValidationException("Invalid Email");
                }

                Regex validatePassword = Password.ValidatePassword();
                if (string.IsNullOrEmpty(user.password) || !validatePassword.IsMatch(user.password))
                {
                    throw new ValidationException("Invalid Password (must contain numbers, at least one capital letter)");
                }

                user.password = Password.EncodePasswordToBase64(user.password);
                user.role = UserRole.Cashier;


                var userEntity = _mapper.Map<User>(user);

                if (await _userServices.CreateUserAsync(userEntity))
                {
                    _log.LogInformation("User registered successfully!");
                    return Ok("User registered syccessfully as a Cashier");
                }
                else
                {
                    throw new ValidationException("User already exists!");
                }
            }
            catch (ValidationException ex)
            {
                _log.LogError($"Validation error during registration: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error while registering: {ex.Message}");
                throw; 
            }
        }

        [Authorize]
        [HttpPost("setrole")]
        public async Task<IActionResult> SetRole([FromBody] RoleModelDTO model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UnauthorizedAccessEx("Unauthorized access");
                }

                var user = await _userServices.GetUserById(int.Parse(userId));
                if (user == null || user.role != UserRole.Admin)
                {
                    throw new UnauthorizedAccessEx("Only admins can update roles");
                }
                UserRole role;
                if (model.role.ToLower() == "admin")
                {
                    role = UserRole.Admin;
                }
                else if( model.role.ToLower() == "cashier")
                {
                  role = UserRole.Cashier;
                }
                else
                {
                    throw new Exception("Invalid role");
                }
                var success = await _userServices.UpdateUserRole(model.username, role);
                if (!success)
                {
                    throw new NotFoundException("User not found");
                }

                return Ok(new { message = "User role updated successfully" });
            }
            catch (UnauthorizedAccessEx ex)
            {
                _log.LogError($"Authorization error: {ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _log.LogError($"Not found error: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error while setting role: {ex.Message}");
                throw; 
            }
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userServices.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error while getting users: {ex.Message}");
                throw; 
            }
        }
    }
}
