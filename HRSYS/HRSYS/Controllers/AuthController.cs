using HRSYS.API.Configurations;
using HRSYS.Application.Interfaces;
using HRSYS.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HRSYS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwt;

        public AuthController(IUserService userService, JwtTokenGenerator jwt)
        {
            _userService = userService;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _userService.Login(dto);

            if (user == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Your password is not correct, try again."
                });
            }

            var token = _jwt.GenerateToken(user);

            return Ok(new
            {
                success = true,
                message = "Login successful , Page index showing",
                token = token,
                User = user
            });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            int newUserId = _userService.RegisterDto(dto);
            if (newUserId <= 0)
            {
                return Ok(new
                {
                    success = false,
                    Message = "Registration failed, try again."
                });
            }


            return Ok(new
            {
                Message = "User created successfully , Back to Login page",
            });
        }

    }
}
