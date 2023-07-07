using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class AioRepo : IAioRepo
    {
         private DataContext _context;
         private SpecialMaps _spec;

         private UserManager<AppUser> _userManager;
        public AioRepo(DataContext context, SpecialMaps spec, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _spec=spec;
        }
        

        public async Task<List<CourseDetailsDto>> getCourses(string id)
        {
            var li = new List<CourseDetailsDto>();
            var user = await _userManager.Users.Include(a => a.Courses).FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(id));
            if (user != null)
            {
                foreach(Class_Course c in user.Courses){
                    li.Add(_spec.mapToCoursedto(c)); 	
                }   
            }
            return li;
        }
        

        public async Task<CourseDetailsDto> getSpecificCourse(string id)
        {
            var help = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == Convert.ToInt32(id));
             return _spec.mapToCoursedto(help);
        }

         public async Task<EpaDetailsDto> getEpa(string id)
        {
           var help = await _context.Epaas.FirstOrDefaultAsync(x => x.EpaId == Convert.ToInt32(id));
           return _spec.mapToepadto(help);
        }
        public async Task<List<EpaDetailsDto>> getEpas(string id)
        {
            var li = new List<EpaDetailsDto>();
            var l = await _userManager.Users.Include(a => a.Epa).FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(id));
            foreach(Class_Epa c in l.Epa){li.Add(_spec.mapToepadto(c)); }
            return li;
       }

        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0; }

        

         public async Task<int> UpdateAsync<T>(T entity) where T : class
        {
            _context.Update(entity);
            if(await SaveAll()){return 1;} else {return 0;}

        } 

         public async Task<int> AddAsync<T>(T entity) where T : class
        {
             _context.Add(entity);
             if(await SaveAll()){return 1;} else {return 0;}
        }

        public async Task<int> DeleteCourse(int id)
        {
            var selectedCourse = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == id);
             _context.Remove(selectedCourse);
            if(await SaveAll()){return 1;} else {return 0;}
        }

        public async Task<int> DeleteEpa(int id)
        {
             var selectedEp = await _context.Epaas.FirstOrDefaultAsync(x => x.EpaId == id);
             _context.Remove(selectedEp);
            if(await SaveAll()){return 1;} else {return 0;}
        }

      
    }
}