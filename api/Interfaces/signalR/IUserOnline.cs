using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces.signalR
{
    public interface IUserOnline
    {
        Task<List<Class_User_Online>> getOnlineUsers();
        Task<Class_User_Online> findUser(string name);
        Task<int> UpdateAsync(Class_User_Online cuo);    
        Task<int> AddAsync(Class_User_Online cuo);
        Task<int> DeleteAsync(Class_User_Online cuo);
        Task<bool> SaveAll();

    }
}