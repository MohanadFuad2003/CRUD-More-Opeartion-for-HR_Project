using HRSYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSYS.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> ?  GetAll();
        Employee? GetById(int id);
        Employee? GetByEmail(string email);
        int   Insert(Employee employee);
        bool   Update(Employee employee);
        bool   Delete(int empId);
    }
}
