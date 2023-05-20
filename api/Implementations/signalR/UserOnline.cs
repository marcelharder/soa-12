using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Interfaces.signalR;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations.signalR
{
    public class UserOnline : IUserOnline
    {
        private DataContext _context;
        public UserOnline(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Class_User_Online cuo) 
        {
            _context.online_users.Add(cuo);
            if (await SaveAll()) { return 1; }
            return 0;
        }

        public async Task<int> DeleteAsync(Class_User_Online cuo) 
        {
             _context.online_users.Remove(cuo);
            if (await SaveAll()) { return 1; }
            return 0;
        }

        public async Task<Class_User_Online> findUser(string name)
        {
            return await _context.online_users.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<List<Class_User_Online>> getOnlineUsers()
        {
            return  await _context.online_users.ToListAsync();
        }

        public Task<bool> SaveAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Class_User_Online cuo) 
        {
            _context.online_users.Update(cuo);
            if (await SaveAll()) { return 1; }
            return 0;
        }
    }
}