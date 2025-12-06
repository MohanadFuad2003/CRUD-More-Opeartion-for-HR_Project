using HRSYS.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_MCV.Helpers;
using WEB_MCV.Services;

namespace WEB_MCV.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _api;
        private readonly JwtService _jwt; 

        public AuthController(ApiService api, JwtService jwt)
        {
            _api = api;
            _jwt = jwt;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var json = await _api.PostAsync("api/Auth/login", dto);

            if (string.IsNullOrWhiteSpace(json))
            {
                AlertHelper.Error(this, "No response from server.");
                return View(dto);
            }

            dynamic response = null!;
            try
            {
                response = JsonConvert.DeserializeObject<dynamic>(json)!;
            }
            catch
            {
                AlertHelper.Error(this, "Failed to parse server response.");
                return View(dto);
            }

            bool isSuccess = response?.success != null ? (bool)response.success : true;
            string message = response?.message != null ? (string)response.message : "Operation completed.";

            if (!isSuccess)
            {
                AlertHelper.Error(this, message);
                return View(dto);
            }

            if (response?.token != null)
                _jwt.Save((string)response.token);

            AlertHelper.Success(this, message);
            return RedirectToAction("Index", "Employee");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var json = await _api.PostAsync("api/Auth/register", dto);

            if (string.IsNullOrWhiteSpace(json))
            {
                AlertHelper.Error(this, "No response from server.");
                return View(dto);
            }

            dynamic response = null!;
            try
            {
                response = JsonConvert.DeserializeObject<dynamic>(json)!;
            }
            catch
            {
                AlertHelper.Error(this, "Failed to parse server response.");
                return View(dto);
            }

            bool isSuccess = response?.success != null ? (bool)response.success : true;
            string message = response?.message != null ? (string)response.message : "Operation completed.";

            if (!isSuccess)
            {
                AlertHelper.Error(this, message);
                return View(dto);
            }

            AlertHelper.Success(this, message);
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            _jwt.Clear();
            return RedirectToAction("Login");
        }
    }
}
