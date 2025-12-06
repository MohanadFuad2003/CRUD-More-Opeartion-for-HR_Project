using System.Data;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Interfaces;
using System.Collections.Generic;
using HRSYS.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using HRSYS.Infrastructure.StoredProcedures;
namespace HRSYS.Infrastructure.DataAccess
{
    public class UserRepository : IUserRepository {
        private readonly SqlHelper _sql;

        public UserRepository(SqlHelper sql)
        {
            _sql = sql;
        }

        public User? Login(string username)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", username)
            };

            DataTable dt = _sql.ExecuteDataTable(UserProcedures.Login, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapToUser(dt.Rows[0]);
        }

        public User? GetById(int userId)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@UserID", userId)
            };

            DataTable dt = _sql.ExecuteDataTable(UserProcedures.GetById, p);
            if (dt.Rows.Count == 0)
                return null;

            return MapToUser(dt.Rows[0]);
        }

        public IEnumerable<User> GetAll()
        {
            DataTable dt = _sql.ExecuteDataTable(UserProcedures.GetAll);

            return dt.AsEnumerable().Select(MapToUser).ToList();
        }

        public int Insert(User user)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@Role", user.Role)
            };

            object result = _sql.ExecuteScalar(UserProcedures.Insert, p);
            return Convert.ToInt32(result);
        }

        public bool Update(User user)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@UserID", user.UserID),
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@IsActive", user.IsActive)
            };

            return _sql.ExecuteNonQuery(UserProcedures.Update, p) > 0;
        }

        public bool Delete(int userId)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@UserID", userId)
            };

            return _sql.ExecuteNonQuery(UserProcedures.Delete, p) > 0;
        }

        private User MapToUser(DataRow row)
        {
            return new User
            {
                UserID = Convert.ToInt32(row["UserID"]),
                Username = row["Username"].ToString()!,
                PasswordHash = row["PasswordHash"].ToString()!,
                Role = row["Role"].ToString()!,
                IsActive = Convert.ToBoolean(row["IsActive"]),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"])
            };
        }
    }
}
