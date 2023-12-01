using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private DataContext _context;

        private SpecialMaps _sm;

        private UserManager<AppUser> _userManager;

        public EmployeeRepository(DataContext context, SpecialMaps sm, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _sm = sm;
        }

        public async Task<List<Class_Item>> GetEmployees(EmployeeParams emp)
        {
            var result = new List<Class_Item>();
            var z = new Class_Item(); z.value = 0; z.description = "Choose"; result.Add(z);
            await Task.Run(() =>
            {
                var selectedHospital = Convert.ToInt32(emp.hospital_id);
                var killLis = false;
                if (emp.liscense_to_kill == "Yes") { killLis = true; }


                if (emp.job_id == "Surgery")
                {
                    // get the surgeons from the users

                    var users = _userManager.Users.OrderByDescending(u => u.Id).AsQueryable();
                    users = users.Where(x => x.hospital_id == selectedHospital);
                    //users = users.Where(j => j.Role == emp.job_id);
                    if (emp.activeState)
                    {
                        users = users.Where(j => j.active == emp.activeState);
                    }
                    if (killLis)
                    {
                        users = users.Where(j => j.ltk == killLis);
                    }
                    foreach (AppUser x in users) { result.Add(_sm.mapUserToClassItem(x)); }

                }
                else
                {
                    var employees = _context.Employees.OrderByDescending(u => u.Id).AsQueryable();
                    employees = employees.Where(x => x.selected_hospital_id == emp.hospital_id);
                    employees = employees.Where(j => j.profession == emp.job_id);
                    if (emp.activeState)
                    {
                        employees = employees.Where(j => j.active == emp.activeState);
                    }

                    foreach (Class_Employee x in employees) { result.Add(_sm.mapToEmployeeToClassItem(x)); }
                }
            });
            return result;
        }

        public async Task<Class_Employee> getSpecificEmployee(int id)
        {
            Class_Employee result;
            if (id == 0)
            {
                result = new Class_Employee();
                result.name = "n/a";
            }
            else
            {
                result = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            }
            return result;
        }
        public async Task<int> updateEmployee(int id, Class_Employee p)
        {
            var result = _context.Employees.Update(p);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Class_Employee> addEmployee()
        {
            var new_record = new Class_Employee();
            _context.Employees.Add(new_record);
            if (await SaveAll()) { return new_record; }
            return null;
        }

        public async Task<int> deleteEmployee(int id)
        {
            var proc = await _context.Employees.FirstOrDefaultAsync(u => u.Id == id);
            _context.Employees.Remove(proc);
            if (await SaveAll()) { return 1; }
            return 0;
        }


    }
}

    
