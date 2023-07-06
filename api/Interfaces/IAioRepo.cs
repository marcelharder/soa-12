using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;

namespace api.Interfaces
{
    public interface IAioRepo
    {
        Task<List<CourseDetailsDto>> getCourses(string id);
        Task<CourseDetailsDto> getSpecificCourse(string id);
        Task<List<EpaDetailsDto>> getEpas(string id);
        Task<EpaDetailsDto> getEpa(string id);
        Task<int> DeleteCourse(int id);
        Task<int> DeleteEpa(int id);
        Task<int> UpdateAsync<T>(T entity) where T : class;    
        Task<int> AddAsync<T>(T entity) where T : class; 
        Task<bool> SaveAll();
    }
}