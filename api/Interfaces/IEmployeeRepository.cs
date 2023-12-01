

using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Class_Item>> GetEmployees(EmployeeParams emp); 
        Task<Class_Employee> getSpecificEmployee(int id);
        Task<int> updateEmployee(int id, Class_Employee p);
        Task<Class_Employee> addEmployee();
        Task<int> deleteEmployee(int id);
        Task<bool> SaveAll();
    }
}