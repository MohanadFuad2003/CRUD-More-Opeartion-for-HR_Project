using HRSYS.Domain.DTOs;

namespace HRSYS.Application.Interfaces
{
    public interface IUserService
    {
        int RegisterDto(RegisterDto registerDto);
        UserDto? Login(LoginDto dto);
        IEnumerable<UserDto> GetAll();
        UserDto? GetById(int id);
        int CreateUser(UserDto user);
        bool UpdateUser(UserDto user);
        bool DeleteUser(int id);
    }
}
