using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSYS.Infrastructure.StoredProcedures
{
    public static class UserProcedures
    {
        public const string Login = "usp_Users_Login";
        public const string Insert = "usp_Users_Insert";
        public const string Update = "usp_Users_Update";
        public const string Delete = "usp_Users_Delete";
        public const string GetAll = "usp_Users_GetAll";
        public const string GetById = "usp_Users_GetById";
    }
}
