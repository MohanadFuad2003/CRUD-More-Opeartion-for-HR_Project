using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSYS.Infrastructure.StoredProcedures
{
    public static class EmployeeProcedures
    {
        public const string Insert = "usp_Employees_Insert";
        public const string Update = "usp_Employees_Update";
        public const string Delete = "usp_Employees_Delete";
        public const string GetAll = "usp_Employees_GetAll";
        public const string GetById = "usp_Employees_GetById";
        public const string Login = "usp_Employees_Login";
    }
}
