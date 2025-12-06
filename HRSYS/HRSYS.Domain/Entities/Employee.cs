using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSYS.Domain.Entities
{
    public class Employee
    {
        public int EmpID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Department { get; set; } = string.Empty;
        public int? CreatedByHR { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PasswordHash { get; set; }
    }
}
