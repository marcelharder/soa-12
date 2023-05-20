using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api.Implementations
{
    public class HospitalRepository : IHospitalRepository
    {
        private UserManager<AppUser> _manager;
        private DataContext _context;
        private IHttpContextAccessor _http;
        private IWebHostEnvironment _env;
        private SpecialMaps _sm;
        private readonly IOptions<ComSettings> _com;


        public HospitalRepository(
            DataContext context,
            UserManager<AppUser> manager,
            SpecialMaps sm,
            IHttpContextAccessor http,
            IWebHostEnvironment env,
            IOptions<ComSettings> com)
        {
            _manager = manager;
            _context = context;
            _env = env;
            _http = http;
            _sm = sm;
            _com = com;
        }

        public async Task<int> addHospital(Class_Hospital p)
        {
            var result = _context.Hospitals.Add(p);
            if (await SaveAll()) { return p.hospitalId; } else { return 99; };
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
            if (await SaveAll()) { return 1; } else { return 0; }
        }

        public List<Class_Item> GetAllCities()
        {
            HashSet<string> myHashSet = new HashSet<string>();// used to prevent duplicates
            var lis = new List<Class_Item>();
            var listOfHospitals = GetAllHospitals();
            foreach (HospitalForReturnDTO item in listOfHospitals)
            {
                var help = new Class_Item();
                help.value = item.id;
                help.description = item.city;
                if (myHashSet.Add(item.city))// used to prevent duplicates, returns false if the item exists
                { lis.Add(help); }
            }
            return lis;
        }

        public List<Class_Item> GetAllCitiesPerCountry(string id)
        {
            var lis = new List<Class_Item>();
            Class_Item dr;
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/countries.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Country")
                                         where d.Element("ISO").Value == id
                                         select d;

            foreach (XElement country in help)
            {

                IEnumerable<XElement> cities = from d in country.Descendants("cities").Elements("items") select d;
                foreach (XElement city in cities)
                {
                    dr = new Class_Item();
                    dr.description = city.Element("description").Value;
                    dr.value = Convert.ToInt32(city.Element("value").Value);
                    lis.Add(dr);
                }
            }
            return lis;
        }

        public List<Class_Country> GetAllCountries()
        {
            var lis = new List<Class_Country>();
            Class_Country dr;
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/countries.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Country") select d;
            foreach (XElement x in help)
            {
                dr = new Class_Country();
                dr.description = x.Element("Description").Value;
                dr.value = x.Element("ISO").Value;
                lis.Add(dr);
            }
            return lis;
        }

        public async Task<List<Class_Hospital>> getAllFullHospitals()
        {
            var result = await _context.Hospitals.ToListAsync();
            return result;
        }

        public async Task<List<Class_Hospital>> getAllFullHospitalsPerCountry(string id)
        {
            var result = await _context.Hospitals.Where(a => a.Country == id).ToListAsync();
            return result;
        }
        public List<HospitalForReturnDTO> GetAllHospitals()
        {
            var hospitals = new List<HospitalForReturnDTO>();
            var result = _context.Hospitals.ToList();
            foreach (Class_Hospital x in result) { hospitals.Add(_sm.mapToHospitalForReturn(x)); }
            return hospitals;
        }

        public async Task<List<HospitalForReturnDTO>> GetAllHospitalsThisSurgeonWorkedIn(int id)
        {
            var currentUser = _manager.Users.FirstOrDefault(x => x.Id == id);
            string[] hospitalIds = currentUser.worked_in.Split(new string[] { "," }, StringSplitOptions.None);
            var list = new List<HospitalForReturnDTO>();

            await Task.Run(async () =>
            {

                foreach (string t in hospitalIds) { list.Add(await this.GetSpecificHospital(t.makeSureTwoChar())); };

            });


            return list;
        }

        public async Task<Class_Hospital> getClassHospital(string id)
        {
            var result = await _context.Hospitals.Where(a => a.HospitalNo == id.makeSureTwoChar()).FirstOrDefaultAsync();
            return result;
        }

        public async Task<HospitalForReturnDTO> GetSpecificHospital(string id)
        {
            var result = await getClassHospital(id);
            return _sm.mapToHospitalForReturn(result);
        }

        public async Task<HospitalForReturnDTO> GetSpecificHospitalFromInventory(string code)
        {
            
            var invaddress = _com.Value.valveURL;
            var test = invaddress + "getHospitalDetails/" + code;
            Class_Hospital hos = new Class_Hospital();

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(test),
                    Content = new StringContent("your json", Encoding.UTF8, "application/json"),
                };

                using (var response = await httpClient.SendAsync(request))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var intermediate = System.Text.Json.JsonSerializer.Deserialize<ReceiveFromInventoryDTO>(apiResponse, new JsonSerializerOptions{
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true
                    });
                    hos.Address = intermediate.adres;
                    hos.Country = intermediate.country;
                    hos.Telephone = intermediate.telephone;
                    hos.HospitalNo = intermediate.hospitalNo;
                    hos.HospitalName = intermediate.naam;
                    hos.ImageUrl = intermediate.image;
                }
            }
            


            return _sm.mapToHospitalForReturn(hos);
        }

        public async Task<bool> HospitalImplementsOVI(string id)
        {
            var ch = await getClassHospital(id);
            var result = ch.usesOnlineValveInventory;
            if (result) { return true; } else { return false; }
        }

        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0; }

        public async Task<int> updateHospital(Class_Hospital p)
        {
            var result = _context.Hospitals.Update(p);
            if (await SaveAll()) { return 1; } else { return 99; };
        }

        public async Task<int> checkHospitalExists(string id)
        {
            var result = 0;
            var help = await _context.Hospitals.Where(a => a.HospitalNo == id.makeSureTwoChar()).FirstOrDefaultAsync();            
            if(help == null){
                result = 1;
                // add new hospital now, first get the hospital details from the inventory
                Class_Hospital ch = new Class_Hospital();
                HospitalForReturnDTO hfr = new HospitalForReturnDTO();
                hfr = await GetSpecificHospitalFromInventory(id);
                ch = _sm.mapToHospital(hfr,ch);
                var help2 = await addHospital(ch);
            }
            return result;

        }
    }
}