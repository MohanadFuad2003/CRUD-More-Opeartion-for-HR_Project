using HRSYS.API.Configurations;
using HRSYS.Application.Interfaces;
using HRSYS.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRSYS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly JwtTokenGenerator _jwt;

        public EmployeeController(IEmployeeService service, JwtTokenGenerator jwt)
        {
            _service = service;
            _jwt = jwt;
        }

        [Authorize]
        [HttpGet("GetAllEmployees")]
        public IActionResult GetAll()
        {
            var employees = _service.GetAll();

            return Ok(new
            {
                success = true,
                message = "Employees loaded",
                data = employees
            });
        }


        [Authorize(Roles = "HR")]

        [HttpGet("getEmployeeById/{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
                return Ok(new { success = false, message = "Invalid employee ID , must be greater than [0]." });

            var emp = _service.GetById(id);
            if (emp == null)
                return Ok(new { success = false, message = $"Employee with ID [ {id} ] not found." });

            return Ok(new { success = true, message = $"Employee found with ID [{id}]", data = emp });
        }


        [Authorize(Roles = "HR")]
        [HttpPost("CreateEmployee")]
        public IActionResult Create(CreateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return Ok(new { success = false, message = "FullName is required , fill it" });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Ok(new { success = false, message = "Email is required , fill it" });

            if (!dto.Email.Contains("@"))
                return Ok(new { success = false, message = "Email format is invalid." });

            if (dto.Salary <= 0)
                return Ok(new { success = false, message = "Salary must be greater than 0" });

            if (string.IsNullOrWhiteSpace(dto.Department))
                return Ok(new { success = false, message = "Department is required , HR or User" });

            int hrUserId = int.Parse(User.FindFirstValue("UserID")!);
            int newId = _service.Create(dto, hrUserId);

            return Ok(new { success = true, EmpID = newId, message = "Employee created successfully." });
        }

        [Authorize(Roles = "HR")]
        [HttpPut("updateEmployee/{id}")]
        public IActionResult Update(int id, UpdateEmployeeDto dto)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid employee ID." });

            var existing = _service.GetById(id);
            if (existing == null)
                return NotFound(new { message = $"Employee with ID {id} not found." });

            if (string.IsNullOrWhiteSpace(dto.FullName))
                return BadRequest(new { message = "FullName is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            if (!dto.Email.Contains("@"))
                return BadRequest(new { message = "Email format is invalid." });

            if (dto.Salary <= 0)
                return BadRequest(new { message = "Salary must be greater than 0." });

            if (string.IsNullOrWhiteSpace(dto.Department))
                return BadRequest(new { message = "Department is required." });

            dto.EmpID = id;

            bool updated = _service.Update(dto);
            if (!updated)
                return BadRequest(new { message = "Update failed." });

            return Ok(new { message = "Employee updated successfully." });
        }

        [Authorize(Roles = "HR")]
        [HttpDelete("DeleteEmployeeByID/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid employee ID." });

            var existing = _service.GetById(id);
            if (existing == null)
                return NotFound(new { message = $"Employee with ID {id} not found." });

            bool deleted = _service.Delete(id);
            if (!deleted)
                return BadRequest(new { message = "Delete failed." });

            return Ok(new { message = "Employee deleted successfully." });
        }

        [HttpPost("register")]
        public IActionResult Register(EmployeeRegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return Ok(new { success = false, message = "FullName is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Ok(new { success = false, message = "Email is required." });

            if (!dto.Email.Contains("@"))
                return Ok(new { success = false, message = "Email format is invalid." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return Ok(new { success = false, message = "Password is required." });

            if (dto.Password.Length < 6)
                return Ok(new { success = false, message = "Password must be at least 6 characters." });

            if (dto.Salary <= 0)
                return Ok(new { success = false, message = "Salary must be greater than 0." });

            if (string.IsNullOrWhiteSpace(dto.Department))
                return Ok(new { success = false, message = "Department is required." });

            try
            {
                int newEmpId = _service.Register(dto);
                return Ok(new
                {
                    success = true,
                    message = "Employee registered successfully.",
                    EmpID = newEmpId
                });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(EmployeeLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return Ok(new { success = false, message = "Email is required." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return Ok(new { success = false, message = "Password is required." });

            var employee = _service.Login(dto);

            if (employee == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Invalid email or password."
                });
            }

            var token = _jwt.GenerateEmployeeToken(employee);

            return Ok(new
            {
                success = true,
                message = "Login successful.",
                token = token,
                employee = employee
            });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var empIdClaim = User.FindFirstValue("EmpID");
            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empId))
            {
                return Ok(new { success = false, message = "Invalid employee token" });
            }

            var profile = _service.GetProfile(empId);
            if (profile == null)
            {
                return Ok(new { success = false, message = "Employee profile not found" });
            }

            return Ok(new
            {
                success = true,
                message = "Profile retrieved successfully",
                data = profile
            });
        }
    }
}
