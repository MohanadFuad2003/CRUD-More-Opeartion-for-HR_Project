using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text;

namespace HRSYS.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Client-side validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Username and password are required.";
                return View();
            }

            var client = _clientFactory.CreateClient("HR_API");

            var loginData = new
            {
                Username = username,
                Password = password
            };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Invalid username or password.";
                return View();
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(apiResponse);
           
            string token = data.token;

            Response.Cookies.Append("JWTToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            TempData["Success"] = "Login successful! Welcome back.";

            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(role))
            {
                TempData["Error"] = "All fields are required.";
                return View();
            }

            var client = _clientFactory.CreateClient("HR_API");

            var registerData = new
            {
                Username = username,
                Password = password,
                Role = role
            };

            var json = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Auth/register", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Registration failed, please try again.";
                return View();
            }

            TempData["Success"] = "Account created successfully! Please login.";

            return RedirectToAction("Login");
        }
    }
}
