using HRSYS.Application.Interfaces;
using HRSYS.Domain.DTOs;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Helpers;
using HRSYS.Domain.Interfaces;

namespace HRSYS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<EmployeeDto> GetAll()
        {
            var employees = _repo.GetAll();

            if (employees == null)
                return Enumerable.Empty<EmployeeDto>();



            return employees.Select(e => new EmployeeDto
            {
                EmpID = e.EmpID,
                FullName = e.FullName,
                Email = e.Email,
                Salary = e.Salary,
                Department = e.Department
            });

        }

        public EmployeeDto? GetById(int id)
        {
            var e = _repo.GetById(id);

            if (e == null) return null;

            return new EmployeeDto
            {
                EmpID = e.EmpID,
                FullName = e.FullName,
                Email = e.Email,
                Salary = e.Salary,
                Department = e.Department
            };
        }

        public int Create(CreateEmployeeDto dto, int hrUserId)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));
            if(hrUserId == 0)
                throw new ArgumentException("HR User ID cannot be zero.", nameof(hrUserId));    

            var emp = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Salary = dto.Salary,
                Department = dto.Department,
                CreatedByHR = hrUserId
            };

              

            return _repo.Insert(emp);
        }

        public EmployeeDto? Login(EmployeeLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return null;

            var employee = _repo.GetByEmail(dto.Email);
            if (employee == null)
                return null;

            if (string.IsNullOrEmpty(employee.PasswordHash))
                return null;

            if (employee.PasswordHash != PasswordHasher.Hash(dto.Password))
                return null;

            return new EmployeeDto
            {
                EmpID = employee.EmpID,
                FullName = employee.FullName,
                Email = employee.Email,
                Salary = employee.Salary,
                Department = employee.Department
            };
        }

        public int Register(EmployeeRegisterDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.", nameof(dto.Email));

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password is required.", nameof(dto.Password));

            var existing = _repo.GetByEmail(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email already exists.");

            var emp = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Salary = dto.Salary,
                Department = dto.Department,
                CreatedByHR = null,
                PasswordHash = PasswordHasher.Hash(dto.Password)
            };

            return _repo.Insert(emp);
        }

        public EmployeeProfileDto? GetProfile(int empId)
        {
            var employee = _repo.GetById(empId);
            if (employee == null)
                return null;

            return new EmployeeProfileDto
            {
                EmpID = employee.EmpID,
                FullName = employee.FullName,
                Email = employee.Email,
                Salary = employee.Salary,
                Department = employee.Department,
                CreatedAt = employee.CreatedAt
            };
        }

        public bool Update(UpdateEmployeeDto dto)
        {
            if(_repo.GetById(dto.EmpID) == null)
                return false;   

            var emp = new Employee
            {
                EmpID = dto.EmpID,
                FullName = dto.FullName,
                Email = dto.Email,
                Salary = dto.Salary,
                Department = dto.Department
            };

            return _repo.Update(emp);
        }

        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
