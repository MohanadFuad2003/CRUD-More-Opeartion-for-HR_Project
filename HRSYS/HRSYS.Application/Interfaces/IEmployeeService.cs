using HRSYS.Domain.DTOs;

namespace HRSYS.Application.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAll();
        EmployeeDto? GetById(int id);
        int Create(CreateEmployeeDto dto, int hrUserId);
        bool Update(UpdateEmployeeDto dto);
        bool Delete(int id);
        EmployeeDto? Login(EmployeeLoginDto dto);
        int Register(EmployeeRegisterDto dto);
        EmployeeProfileDto? GetProfile(int empId);
    }
}
