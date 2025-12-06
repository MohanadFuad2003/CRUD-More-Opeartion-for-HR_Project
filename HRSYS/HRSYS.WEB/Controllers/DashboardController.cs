using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRSYS.WEB.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public DashboardController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private HttpClient GetClient()
        {
            var token = Request.Cookies["JWTToken"];
            var client = _clientFactory.CreateClient("HR_API");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        // --- 1. Show All Employees ---
        public async Task<IActionResult> Index()
        {
            var client = GetClient();
            var response = await client.GetAsync("api/Employee/GetAllEmployees");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unauthorized or failed to load data.";
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic employees = JsonConvert.DeserializeObject(json);

            return View(employees);
        }

        // --- 2. Search By ID ---
        public async Task<IActionResult> SearchById(int id)
        {
            var client = GetClient();
            var response = await client.GetAsync($"api/Employee/getEmployeeById/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Employee not found!";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic employee = JsonConvert.DeserializeObject(json);

            return View("SearchResult", employee);
        }

        // --- 3. Create (GET) ---
        public IActionResult Create()
        {
            return View();
        }

        // --- 3. Create (POST) ---
        [HttpPost]
        public async Task<IActionResult> Create(string fullName, string email, decimal salary, string department)
        {
            var client = GetClient();

            var data = new
            {
                FullName = fullName,
                Email = email,
                Salary = salary,
                Department = department
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Employee/CreateEmployee", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to create employee.";
                return View();
            }

            return RedirectToAction("Index");
        }

        // --- 4. Edit (GET) ---
        public async Task<IActionResult> Edit(int id)
        {
            var client = GetClient();
            var response = await client.GetAsync($"api/Employee/getEmployeeById/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Employee not found!";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic employee = JsonConvert.DeserializeObject(json);

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int empID, string fullName, string email, decimal salary, string department)
        {
            var client = GetClient();

            var data = new
            {
                EmpID = empID,
                FullName = fullName,
                Email = email,
                Salary = salary,
                Department = department
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Employee/updateEmployee/{empID}", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to update employee.";
                return View();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = GetClient();
            var response = await client.DeleteAsync($"api/Employee/DeleteEmployeeByID/{id}");

            return RedirectToAction("Index");
        }
    }
}
