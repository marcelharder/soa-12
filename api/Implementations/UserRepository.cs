using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class UserRepository : IUserRepository
    {
       private RoleManager<AppRole> _roleManager;
       private UserManager<AppUser> _userManager;
       private readonly IHttpContextAccessor _httpContextAccessor;
      

       private DataContext _context;

       public string Role {get; set;}
       

        public UserRepository(
            IHttpContextAccessor httpContextAccessor,
            RoleManager<AppRole> roleManager, 
            UserManager<AppUser> userManager, 
            DataContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<AppUser> GetUser(int id)
        {
            if (id != 0) {
            var result = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            return result;
            }
            return null;
        }
        
        public async Task<List<AppUser>> GetUsers()
        {
           
            var users = _userManager.Users.OrderByDescending(u => u.Id).AsQueryable();
            
            return await users.ToListAsync();
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Delete<T>(T entity) where T : class
        {
             _context.Remove(entity);
        }
        public void Update(AppUser p)
        {
           _context.Users.Update(p);
           
        }
        public void AddUser(AppUser p)
        {
            _context.Users.Add(p);
        }

        public async Task<PagedList<AppUser>> GetUsersByHospital(UserParams userParams)
        {   var centerId = Convert.ToInt32(userParams.center_id);
            var users = _userManager.Users.OrderByDescending(u => u.UserName).AsQueryable();
            users = users.Where(x => x.hospital_id == centerId);
            return await PagedList<AppUser>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
      
        }
         public async Task<PagedList<AppUser>> GetAiosByHospital(UserParams userParams)
        {   
            // get the center_id from the logedin user
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // get the hospital id from the logged in user
            var loggedinUser = await GetUser(Convert.ToInt32(userId));
            var centerId = loggedinUser.hospital_id;
            var users = _userManager.Users.OrderByDescending(u => u.UserName).AsQueryable();
            users = users.Where(x => x.hospital_id == centerId);
            users = users.Where(x => x.active == true);
            users = users.Where(x => x.ltk == false);
                 

            return await PagedList<AppUser>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
      
        }

        public async Task<bool> GetUserLtk(int id)
        {
           var user = await GetUser(id);
           return user.ltk;
        }

        public async Task<PagedList<AppUser>> GetSurgeonsByHospital(UserParams userParams)
        {
            var centerId = Convert.ToInt32(userParams.center_id);
            var users = _userManager.Users.OrderByDescending(u => u.UserName).AsQueryable();
            users = users.Where(x => x.hospital_id == centerId);
            users = users.Where(x => x.active == true);
            users = users.Where(x => x.ltk == true);
            return await PagedList<AppUser>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetChefsByHospital(int center_id)
        {  
            var selectedChef = new AppUser();
            selectedChef.hospital_id = 9999;
            var chefs = await _userManager.GetUsersInRoleAsync("Chef");
            foreach(AppUser u in chefs){
               if(u.hospital_id == center_id){
                  selectedChef = u;
               }
            }
            return selectedChef;
        }

        public async Task<List<Class_Course>> GetCourses(int id)
        {
            var selectedUser = await _userManager.Users
            .Include(a => a.Courses)
            .FirstOrDefaultAsync(x => x.Id == id);
            return selectedUser.Courses.ToList();
        }

        public async Task<List<Class_Epa>> GetEpaas(int id)
        {
             var selectedUser = await _userManager.Users
            .Include(a => a.Epa)
            .FirstOrDefaultAsync(x => x.Id == id);
            return selectedUser.Epa.ToList();
        }

        public async Task<bool> UpdatePayment(DateTime d, int id)
        {
           var help = false;
           var user = await GetUser(id);
           user.PaidTill = d;
           _context.Update(user);
           if(await _context.SaveChangesAsync() > 0){ help = true;}
           return help;
        }
    }
}