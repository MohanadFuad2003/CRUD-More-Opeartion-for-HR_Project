using HRSYS.Application.Interfaces;
using HRSYS.Domain.DTOs;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Helpers;
using HRSYS.Domain.Interfaces;

namespace HRSYS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;   

        public UserService(IUserRepository repo)
        {
            _repo = repo; 
        }

        public UserDto? Login(LoginDto dto)
        {
            var user = _repo.Login(dto.Username); 
            if (user == null) return null;

            if (user.PasswordHash != PasswordHasher.Hash(dto.Password) || (!user.IsActive))
                return null;
         
                return new UserDto
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Role = user.Role
                };
        }


        public int RegisterDto(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            return _repo.Insert(user);
        }


        public IEnumerable<UserDto> GetAll()
        {
            return _repo.GetAll()!.Select(u => new UserDto
            {
                UserID = u.UserID,
                Username = u.Username,
                Role = u.Role
            });
        }

        public UserDto? GetById(int id)
        {
            var u = _repo.GetById(id);
            if (u == null) return null;

            return new UserDto
            {
                UserID = u.UserID,
                Username = u.Username,
                Role = u.Role
            };
        }

        public int CreateUser(UserDto dto)
        {
            return _repo.Insert(new Domain.Entities.User
            {
                Username = dto.Username,
                Role = dto.Role,
                PasswordHash = PasswordHasher.Hash("Default123") // OR custom
            });
        }

        public bool UpdateUser(UserDto dto)
        {
            return _repo.Update(new Domain.Entities.User
            {
                UserID = dto.UserID,
                Username = dto.Username,
                Role = dto.Role,
                IsActive = true
            });
        }

        public bool DeleteUser(int id)
        {
            return _repo.Delete(id);
        }
    }
}
