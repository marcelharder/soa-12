using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{

    public interface IUserRepository
    {
        Task<List<AppUser>> GetUsers();
        Task<List<Class_Course>> GetCourses(int id);
        Task<List<Class_Epa>> GetEpaas(int id);
        Task<PagedList<AppUser>> GetUsersByHospital(UserParams userParams);
        Task<PagedList<AppUser>> GetAiosByHospital(UserParams userParams);
        Task<PagedList<AppUser>> GetSurgeonsByHospital(UserParams userParams);
        Task<AppUser> GetChefsByHospital(int center_id);
        Task<AppUser> GetUser(int id);
        Task<bool> GetUserLtk(int id);
        Task<bool> UpdatePayment(DateTime d, int id);
        void AddUser(AppUser p);
        Task<bool> SaveAll();
        void Update(AppUser p);
        void Delete<T>(T entity) where T : class;
    }
}