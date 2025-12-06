using HRSYS.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_MCV.Helpers;
using WEB_MCV.Services;

namespace WEB_MCV.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApiService _api;
        private readonly JwtService _jwt;

        public EmployeeController(ApiService api, JwtService jwt)
        {
            _api = api;
            _jwt = jwt;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id.HasValue)
            {
                var (success, json) = await _api.GetAsync($"api/Employee/getEmployeeById/{id.Value}");

                dynamic res = JsonConvert.DeserializeObject<dynamic>(json)!;

                if (res.success == false)
                {
                    AlertHelper.Error(this, (string)res.message);
                    return View(new List<EmployeeDto>());
                }

                AlertHelper.Success(this, (string)res.message);

                var emp = JsonConvert.DeserializeObject<EmployeeDto>(
                    Convert.ToString(res.data)
                );

                return View(new List<EmployeeDto>() { emp });
            }

            var (isSuccess, employeesJson) = await _api.GetAsync("api/Employee/GetAllEmployees");

            if (!isSuccess)
            {
                AlertHelper.Error(this, "Unauthorized or failed to load data.");
                return RedirectToAction("Login", "Auth");
            }

            dynamic listRes = JsonConvert.DeserializeObject<dynamic>(employeesJson)!;

            if (listRes.success == false)
            {
                AlertHelper.Error(this, (string)listRes.message);
                return View(new List<EmployeeDto>());
            }
            var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(
                Convert.ToString(listRes.data)
            );

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateEmployeeDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var json = await _api.PostAsync("api/Employee/CreateEmployee", dto);

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
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpGet]
public async Task<IActionResult> Edit(int id)
{
    var (isSuccess, json) = await _api.GetAsync($"api/Employee/getEmployeeById/{id}");
    if (!isSuccess)
    {
        AlertHelper.Error(this, "Employee not found.");
        return RedirectToAction("Index");
    }

    dynamic res = JsonConvert.DeserializeObject<dynamic>(json)!;

    if (res.success == false)
    {
        AlertHelper.Error(this, (string)res.message);
        return RedirectToAction("Index");
    }

    var emp = JsonConvert.DeserializeObject<EmployeeDto>(Convert.ToString(res.data));

    var dto = new UpdateEmployeeDto
    {
        FullName = emp.FullName,
        Email = emp.Email,
        Salary = emp.Salary,
        Department = emp.Department
    };

    return View(dto);
}


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateEmployeeDto dto)
        {
            var (isSuccess, json) = await _api.PutAsync($"api/Employee/updateEmployee/{id}", dto);

            if (!isSuccess)
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<dynamic>(json);
                    string message = error?.message ?? "Update failed.";
                    AlertHelper.Error(this, message);
                }
                catch
                {
                    AlertHelper.Error(this, "Update failed.");
                }
                return View(dto);
            }

            try
            {
                var response = JsonConvert.DeserializeObject<dynamic>(json);
                string message = response?.message ?? "Employee updated successfully.";
                AlertHelper.Success(this, message);
            }
            catch
            {
                AlertHelper.Success(this, "Employee updated successfully.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var (isSuccess, json) = await _api.DeleteAsync($"api/Employee/DeleteEmployeeByID/{id}");
            if (!isSuccess)
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<dynamic>(json);
                    string message = error?.message ?? "Delete failed.";
                    AlertHelper.Error(this, message);
                }
                catch
                {
                    AlertHelper.Error(this, "Delete failed.");
                }
                return RedirectToAction("Index");
            }

            try
            {
                var response = JsonConvert.DeserializeObject<dynamic>(json);
                string message = response?.message ?? "Employee deleted successfully.";
                AlertHelper.Success(this, message);
            }
            catch
            {
                AlertHelper.Success(this, "Employee deleted successfully.");
            }

            return RedirectToAction("Index");
        }
    }
}
