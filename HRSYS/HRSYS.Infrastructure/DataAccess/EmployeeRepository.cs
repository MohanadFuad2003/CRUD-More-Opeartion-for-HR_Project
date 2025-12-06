using System.Data;
using Microsoft.Data.SqlClient;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Interfaces;
using HRSYS.Infrastructure.Helpers;
using HRSYS.Infrastructure.StoredProcedures;

namespace HRSYS.Infrastructure.DataAccess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlHelper _sql;

        public EmployeeRepository(SqlHelper sql)
        {
            _sql = sql;
        }

        public IEnumerable<Employee> GetAll()
        {
            DataTable dt = _sql.ExecuteDataTable(EmployeeProcedures.GetAll);
            if (dt.Rows.Count == 0)
                return Enumerable.Empty<Employee>();


            return FillEmployeesIntoList(dt);   
        }
         
        private IEnumerable<Employee> FillEmployeesIntoList(DataTable dt)
        {
            var employees = new List<Employee>();
            foreach (DataRow row in dt.Rows)
            {
                employees.Add(MapToEmployee(row));
            }
            return employees;
        }



        public Employee? GetById(int id)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@EmpID", id)
            };

            DataTable dt = _sql.ExecuteDataTable(EmployeeProcedures.GetById, p);

            if (dt.Rows.Count == 0)
                return null;

            return MapToEmployee(dt.Rows[0]);
        }

        public Employee? GetByEmail(string email)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@Email", email)
            };

            DataTable dt = _sql.ExecuteDataTable(EmployeeProcedures.Login, p);

            if (dt.Rows.Count == 0)
                return null;

            return MapToEmployee(dt.Rows[0]);
        }

        public int Insert(Employee employee)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@FullName", employee.FullName),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Salary", employee.Salary),
                new SqlParameter("@Department", employee.Department),
                new SqlParameter("@CreatedByHR", employee.CreatedByHR ?? (object)DBNull.Value),
                new SqlParameter("@PasswordHash", employee.PasswordHash ?? (object)DBNull.Value)
            };

            object result = _sql.ExecuteScalar(EmployeeProcedures.Insert, p);
            return Convert.ToInt32(result);
        }

        public bool Update(Employee employee)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@EmpID", employee.EmpID),
                new SqlParameter("@FullName", employee.FullName),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Salary", employee.Salary),
                new SqlParameter("@Department", employee.Department)
            };

            return _sql.ExecuteNonQuery(EmployeeProcedures.Update, p) > 0;
        }

        public bool Delete(int empId)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@EmpID", empId)
            };

            return _sql.ExecuteNonQuery(EmployeeProcedures.Delete, p) > 0;
        }

        private Employee MapToEmployee(DataRow row)
        {
            return new Employee
            {
                EmpID = Convert.ToInt32(row["EmpID"]),
                FullName = row["FullName"].ToString()!,
                Email = row["Email"].ToString()!,
                Salary = Convert.ToDecimal(row["Salary"]),
                Department = row["Department"].ToString()!,
                CreatedByHR = row["CreatedByHR"] == DBNull.Value ? null : Convert.ToInt32(row["CreatedByHR"]),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                PasswordHash = row["PasswordHash"] == DBNull.Value ? null : row["PasswordHash"].ToString()
            };
        }
    }
}
