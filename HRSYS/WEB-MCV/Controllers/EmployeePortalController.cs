using HRSYS.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_MCV.Helpers;
using WEB_MCV.Services;

namespace WEB_MCV.Controllers
{
    public class EmployeePortalController : Controller
    {
        private readonly ApiService _api;
        private readonly EmployeeSessionService _employeeSession;

        public EmployeePortalController(ApiService api, EmployeeSessionService employeeSession)
        {
            _api = api;
            _employeeSession = employeeSession;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new EmployeeLoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(EmployeeLoginDto dto)
        {
            var json = await _api.PostAsync("api/Employee/login", dto, string.Empty);

            if (string.IsNullOrWhiteSpace(json))
            {
                AlertHelper.Error(this, "No response from server.");
                return View(dto);
            }

            dynamic response;
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

            if (!isSuccess || response?.token == null)
            {
                AlertHelper.Error(this, message);
                return View(dto);
            }

            _employeeSession.Save((string)response.token);
            AlertHelper.Success(this, message);
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token = _employeeSession.Get();
            if (string.IsNullOrEmpty(token))
            {
                AlertHelper.Warning(this, "Please login first.");
                return RedirectToAction("Login");
            }

            var (isSuccess, json) = await _api.GetAsync("api/Employee/profile", token);

            if (!isSuccess || string.IsNullOrWhiteSpace(json))
            {
                AlertHelper.Error(this, "Failed to fetch employee profile.");
                return RedirectToAction("Login");
            }

            dynamic response;
            try
            {
                response = JsonConvert.DeserializeObject<dynamic>(json)!;
            }
            catch
            {
                AlertHelper.Error(this, "Failed to parse profile response.");
                return RedirectToAction("Login");
            }

            if (response?.success == false)
            {
                AlertHelper.Error(this, (string)response.message);
                return RedirectToAction("Login");
            }

            var profileJson = Convert.ToString(response?.data);
            var profile = JsonConvert.DeserializeObject<EmployeeProfileDto>(profileJson ?? string.Empty);

            if (profile == null)
            {
                AlertHelper.Error(this, "Employee profile not found.");
                return RedirectToAction("Login");
            }

            return View(profile);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new EmployeeRegisterDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(EmployeeRegisterDto dto)
        {
            var json = await _api.PostAsync("api/Employee/register", dto, string.Empty);

            if (string.IsNullOrWhiteSpace(json))
            {
                AlertHelper.Error(this, "No response from server.");
                return View(dto);
            }

            dynamic response = JsonConvert.DeserializeObject<dynamic>(json)!;

            bool isSuccess = response?.success ?? false;
            string message = response?.message ?? "Something went wrong.";

            if (!isSuccess)
            {
                AlertHelper.Error(this, message);
                return View(dto);
            }

            AlertHelper.Success(this, "Registration successful! You can now login.");

            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            _employeeSession.Clear();
            AlertHelper.Success(this, "You have been logged out.");
            return RedirectToAction("Login");
        }
    }
}

