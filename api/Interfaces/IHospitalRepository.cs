using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;

namespace api.Interfaces
{
    public interface IHospitalRepository
    {
        List<HospitalForReturnDTO> GetAllHospitals();       
        Task<HospitalForReturnDTO> GetSpecificHospital(string hospitalId);
        Task<HospitalForReturnDTO> GetSpecificHospitalFromInventory(string hospitalId);     
        Task<List<HospitalForReturnDTO>> GetAllHospitalsThisSurgeonWorkedIn(int id) ;
        List<Class_Country> GetAllCountries();
        List<Class_Item> GetAllCities();
        Task<int> updateHospital(Class_Hospital p);
        Task<int> addHospital(Class_Hospital p);
        Task<bool> SaveAll();
        Task<int> checkHospitalExists(string hospitalNo);
        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<Class_Hospital> getClassHospital(string id);
        List<Class_Item> GetAllCitiesPerCountry(string id);
        Task<bool> HospitalImplementsOVI(string id);
        Task<List<Class_Hospital>> getAllFullHospitals();
        Task<List<Class_Hospital>> getAllFullHospitalsPerCountry(string id);
        
    }
}