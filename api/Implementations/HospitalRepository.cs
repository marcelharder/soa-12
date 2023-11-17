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
                    var intermediate = System.Text.Json.JsonSerializer.Deserialize<ReceiveFromInventoryDTO>(apiResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
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
            if (help == null)
            {
                result = 1;
                // add new hospital now, first get the hospital details from the inventory
                Class_Hospital ch = new Class_Hospital();
                HospitalForReturnDTO hfr = new HospitalForReturnDTO();
                hfr = await GetSpecificHospitalFromInventory(id);
                ch = _sm.mapToHospital(hfr, ch);
                var help2 = await addHospital(ch);
            }
            return result;

        }
       
        #region <!--institutional report suggestions -->


        #region <!--institutional stuff-->
        public InstitutionalDTO getInstitutionalReport(int hospitalNo, int soort)
        {
            InstitutionalDTO it = new InstitutionalDTO();


            Hospital rep = new Hospital();
            //  get the XML file in memory
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
            XDocument order = XDocument.Load(filename);
            // select hospital where id attribute = hospitalNo
            IEnumerable<XElement> help = from d in order.Descendants("hospital")
                                         where d.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar()
                                         select d;
            foreach (XElement x in help)
            {
                if (help.Any())
                {
                    foreach (XElement original in help)
                    {
                        IEnumerable<XElement> help2 = from d in original.Elements("reports")
                        .Elements("text_by_type_of_surgery")
                        .Elements("soort")
                                                      where d.Attribute("id").Value == soort.ToString()
                                                      select d;
                        foreach (XElement f in help2)
                        {
                            it = getIDTO(it, f);
                            checkForNullValues(it);
                        }
                    }
                }
            }
            return it;
        }
        public string createInstitutionalReport(int hospitalNo)
        {
            var result = "";
            // get the xml file in memory
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
            XDocument doc = XDocument.Load(filename);
            // see if there is already an XElelemt with this id
            IEnumerable<XElement> test = from d in doc.Descendants("hospital")
                                         where d.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar()
                                         select d;
            if (test.Any()) { result = "this hospital already exists"; }
            else
            {
                // get the element for the correct language
                IEnumerable<XElement> help = from d in doc.Descendants("hospital")
                                             where d.Attribute("id").Value == "99999"
                                             select d;
                foreach (XElement x in help)
                {
                    XElement copy = new XElement(x);
                    copy = changeHospitalNo(copy, hospitalNo.ToString().makeSureTwoChar());
                    doc.Root.Add(copy);
                }
                doc.Save(filename);
                result = "created";
            }
            return result;
        }
        public string updateInstitutionalReport(InstitutionalDTO rep, int soort, int hospitalNo)
        {

            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
            XDocument doc = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in doc.Descendants("hospital")
                                         where d.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar()
                                         select d;
            if (help.Any())
            {
                foreach (XElement original in help)
                {
                    IEnumerable<XElement> help2 = from d in original.Elements("reports")
                    .Elements("text_by_type_of_surgery")
                    .Elements("soort")
                                                  where d.Attribute("id").Value == soort.ToString()
                                                  select d;
                    foreach (XElement f in help2)
                    {
                        updateXML(f, rep);
                    }
                    doc.Save(filename);
                }
            }

            return "";
        }
        #endregion

        #region <!--additionalReport stuff-->
        public AdditionalReportDTO getAdditionalReportItems(int hospitalNo, int which)
        {
            
            var ar = new AdditionalReportDTO();
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "assets/json/additionalReportItems.json");
            var jsonData = System.IO.File.ReadAllText(filename);
            var oldjson = System.Text.Json.JsonSerializer.Deserialize<List<Data.Root>>(jsonData);
            var selectedARep = oldjson.Find(x => x.hospitalNo == hospitalNo);

            if(selectedARep == null){
                this.createAdditionalReport(hospitalNo);
                jsonData = System.IO.File.ReadAllText(filename);
                oldjson = System.Text.Json.JsonSerializer.Deserialize<List<Data.Root>>(jsonData);
                selectedARep = oldjson.Find(x => x.hospitalNo == hospitalNo);
               }


            switch (which)
            {
                // request circ. support items
                case 1:
                    ar.line_1 = selectedARep.circulation_support.items[0].content;
                    ar.line_2 = selectedARep.circulation_support.items[1].content;
                    ar.line_3 = selectedARep.circulation_support.items[2].content;
                    ar.line_4 = selectedARep.circulation_support.items[3].content;
                    ar.line_5 = selectedARep.circulation_support.items[4].content;
                    break;
                // request iabp. support items
                case 2:
                    ar.line_1 = selectedARep.iabp.items[0].content;
                    ar.line_2 = selectedARep.iabp.items[1].content;
                    ar.line_3 = selectedARep.iabp.items[2].content;
                    ar.line_4 = selectedARep.iabp.items[3].content;
                    ar.line_5 = selectedARep.iabp.items[4].content;
                    break;
                // request pmwires. support items
                case 3:
                    ar.line_1 = selectedARep.pmwires.items[0].content;
                    ar.line_2 = selectedARep.pmwires.items[1].content;
                    ar.line_3 = selectedARep.pmwires.items[2].content;
                    ar.line_4 = selectedARep.pmwires.items[3].content;
                    ar.line_5 = selectedARep.pmwires.items[4].content;
                    break;
            }


            /*  var contentRoot = _env.ContentRootPath;
             var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
             var doc = XDocument.Load(filename);

             var hospital = doc.Descendants("hospital")
                .FirstOrDefault(h => h.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar());

             if (hospital != null)
             {
                 var reports = hospital.Element("reports");

                 switch (which)
                 {
                     case 1:
                         var circulationSupport = reports.Element("circulation_support");
                         var items = circulationSupport.Elements("items");
                         foreach (var item in items)
                         {
                             l.Add(item.Element("regel_21").Value);
                         }
                         break;
                     case 2:
                         var iabp = reports.Element("iabp");
                         var iabpItems = iabp.Elements("items");
                         foreach (var item in iabpItems)
                         {
                             l.Add(item.Element("regel_22").Value);
                         }
                         break;
                     case 3:
                         var pmwires = reports.Element("pmwires");
                         var pmwiresItems = pmwires.Elements("items");
                         foreach (var item in pmwiresItems)
                         {
                             l.Add(item.Element("regel_23").Value);
                         }
                         break;
                 } */



            return ar;
        }
        public int updateAdditionalReportItem(AdditionalReportDTO up, int hospitalNo, int which)
        {

            up = checkforNullInAdditionalReport(up);
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "assets/json/additionalReportItems.json");
            var jsonData = System.IO.File.ReadAllText(filename);
            var oldjson = System.Text.Json.JsonSerializer.Deserialize<List<Data.Root>>(jsonData);

            var selectedARep = oldjson.Find(x => x.hospitalNo == hospitalNo);

            switch (which)
            {

                case 1:
                    selectedARep.circulation_support.items.Clear();
                    selectedARep.circulation_support.items.Add(new Data.Item { content = up.line_1 });
                    selectedARep.circulation_support.items.Add(new Data.Item { content = up.line_2 });
                    selectedARep.circulation_support.items.Add(new Data.Item { content = up.line_3 });
                    selectedARep.circulation_support.items.Add(new Data.Item { content = up.line_4 });
                    selectedARep.circulation_support.items.Add(new Data.Item { content = up.line_5 });
                    break;
                case 2:
                    selectedARep.iabp.items.Clear();
                    selectedARep.iabp.items.Add(new Data.Item { content = up.line_1 });
                    selectedARep.iabp.items.Add(new Data.Item { content = up.line_2 });
                    selectedARep.iabp.items.Add(new Data.Item { content = up.line_3 });
                    selectedARep.iabp.items.Add(new Data.Item { content = up.line_4 });
                    selectedARep.iabp.items.Add(new Data.Item { content = up.line_5 });
                    break;
                case 3:
                    selectedARep.pmwires.items.Clear();
                    selectedARep.pmwires.items.Add(new Data.Item { content = up.line_1 });
                    selectedARep.pmwires.items.Add(new Data.Item { content = up.line_2 });
                    selectedARep.pmwires.items.Add(new Data.Item { content = up.line_3 });
                    selectedARep.pmwires.items.Add(new Data.Item { content = up.line_4 });
                    selectedARep.pmwires.items.Add(new Data.Item { content = up.line_5 });
                    break;
            }

            var test_json = System.Text.Json.JsonSerializer.Serialize(oldjson);
            File.WriteAllText(filename, test_json);
            return 1;
        }
        public string createAdditionalReport(int hospitalNo)
        {
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "assets/json/additionalReportItems.json");
            var jsonData = System.IO.File.ReadAllText(filename);
            var oldjson = System.Text.Json.JsonSerializer.Deserialize<List<Data.Root>>(jsonData);


            var circ = new Data.CirculationSupport
            {
                items = new List<Data.Item>
    {
        new Data.Item { content = "Circ-1" },
        new Data.Item { content = "Circ-2" },
        new Data.Item { content = "Circ-3" },
        new Data.Item { content = "Circ-4" },
        new Data.Item { content = "Circ-5" }
    }
            };

            var iabp = new Data.Iabp
            {
                items = new List<Data.Item>
    {
        new Data.Item { content = "iabp_1" },
        new Data.Item { content = "iabp_2" },
        new Data.Item { content = "iabp_3" },
        new Data.Item { content = "iabp_4" },
        new Data.Item { content = "iabp_5" }
    }
            };

            var pm = new Data.Pmwires
            {
                items = new List<Data.Item>
    {
        new Data.Item { content = "pm_1" },
        new Data.Item { content = "pm_2" },
        new Data.Item { content = "pm_3" },
        new Data.Item { content = "pm_4" },
        new Data.Item { content = "pm_5" }
    }
            };

            var test = new Root
            {
                hospitalNo = hospitalNo,
                circulation_support = circ,
                iabp = iabp,
                pmwires = pm
            };

            oldjson.Add(test);

            var test_json = System.Text.Json.JsonSerializer.Serialize(oldjson);

            File.WriteAllText(filename, test_json);

            // now write this to the json file 
            Console.WriteLine(oldjson);

            return test_json;
        }

        #endregion

        private AdditionalReportDTO checkforNullInAdditionalReport(AdditionalReportDTO up)
        {

            up.line_1 = up.line_1 == null ? "" : up.line_1;
            up.line_2 = up.line_2 == null ? "" : up.line_2;
            up.line_3 = up.line_3 == null ? "" : up.line_3;
            up.line_4 = up.line_4 == null ? "" : up.line_4;
            up.line_5 = up.line_5 == null ? "" : up.line_5;
            return up;
        }
        private InstitutionalDTO getIDTO(InstitutionalDTO it, XElement el)
        {
            it.regel1A = el.Element("regel_1_a").Value;
            it.regel1B = el.Element("regel_1_b").Value;
            it.regel1C = el.Element("regel_1_c").Value;

            it.regel2A = el.Element("regel_2_a").Value;
            it.regel2B = el.Element("regel_2_b").Value;
            it.regel2C = el.Element("regel_2_c").Value;

            it.regel3A = el.Element("regel_3_a").Value;
            it.regel3B = el.Element("regel_3_b").Value;
            it.regel3C = el.Element("regel_3_c").Value;

            it.regel4A = el.Element("regel_4_a").Value;
            it.regel4B = el.Element("regel_4_b").Value;
            it.regel4C = el.Element("regel_4_c").Value;

            it.regel5A = el.Element("regel_5_a").Value;
            it.regel5B = el.Element("regel_5_b").Value;
            it.regel5C = el.Element("regel_5_c").Value;

            it.regel6A = el.Element("regel_6_a").Value;
            it.regel6B = el.Element("regel_6_b").Value;
            it.regel6C = el.Element("regel_6_c").Value;

            it.regel7A = el.Element("regel_7_a").Value;
            it.regel7B = el.Element("regel_7_b").Value;
            it.regel7C = el.Element("regel_7_c").Value;

            it.regel8A = el.Element("regel_8_a").Value;
            it.regel8B = el.Element("regel_8_b").Value;
            it.regel8C = el.Element("regel_8_c").Value;

            it.regel9A = el.Element("regel_9_a").Value;
            it.regel9B = el.Element("regel_9_b").Value;
            it.regel9C = el.Element("regel_9_c").Value;

            it.regel10A = el.Element("regel_10_a").Value;
            it.regel10B = el.Element("regel_10_b").Value;
            it.regel10C = el.Element("regel_10_c").Value;

            it.regel11A = el.Element("regel_11_a").Value;
            it.regel11B = el.Element("regel_11_b").Value;
            it.regel11C = el.Element("regel_11_c").Value;

            it.regel12A = el.Element("regel_12_a").Value;
            it.regel12B = el.Element("regel_12_b").Value;
            it.regel12C = el.Element("regel_12_c").Value;

            it.regel13A = el.Element("regel_13_a").Value;
            it.regel13B = el.Element("regel_13_b").Value;
            it.regel13C = el.Element("regel_13_c").Value;

            it.regel14A = el.Element("regel_14_a").Value;
            it.regel14B = el.Element("regel_14_b").Value;
            it.regel14C = el.Element("regel_14_c").Value;

            it.regel15 = el.Element("regel_15").Value;
            it.regel16 = el.Element("regel_16").Value;
            it.regel17 = el.Element("regel_17").Value;
            it.regel18 = el.Element("regel_18").Value;
            it.regel19 = el.Element("regel_19").Value;
            it.regel20 = el.Element("regel_20").Value;
            it.regel21 = el.Element("regel_21").Value;
            it.regel22 = el.Element("regel_22").Value;
            it.regel23 = el.Element("regel_23").Value;
            it.regel24 = el.Element("regel_24").Value;
            it.regel25 = el.Element("regel_25").Value;
            it.regel26 = el.Element("regel_26").Value;
            it.regel27 = el.Element("regel_27").Value;
            it.regel28 = el.Element("regel_28").Value;
            it.regel29 = el.Element("regel_29").Value;
            it.regel30 = el.Element("regel_30").Value;
            it.regel31 = el.Element("regel_31").Value;
            it.regel32 = el.Element("regel_32").Value;
            it.regel33 = el.Element("regel_33").Value;



            return it;
        }
        private XElement changeHospitalNo(XElement el, string help)
        {

            el.SetAttributeValue("id", help);

            return el;

        }
        private XElement updateXML(XElement el, InstitutionalDTO rep)
        {
            rep = checkForNullValues(rep);

            el.Element("regel_1_a").SetValue(rep.regel1A);
            el.Element("regel_1_b").SetValue(rep.regel1B);
            el.Element("regel_1_c").SetValue(rep.regel1C);

            el.Element("regel_2_a").SetValue(rep.regel2A);
            el.Element("regel_2_b").SetValue(rep.regel2B);
            el.Element("regel_2_c").SetValue(rep.regel2C);

            el.Element("regel_3_a").SetValue(rep.regel3A);
            el.Element("regel_3_b").SetValue(rep.regel3B);
            el.Element("regel_3_c").SetValue(rep.regel3C);

            el.Element("regel_4_a").SetValue(rep.regel4A);
            el.Element("regel_4_b").SetValue(rep.regel4B);
            el.Element("regel_4_c").SetValue(rep.regel4C);

            el.Element("regel_5_a").SetValue(rep.regel5A);
            el.Element("regel_5_b").SetValue(rep.regel5B);
            el.Element("regel_5_c").SetValue(rep.regel5C);

            el.Element("regel_6_a").SetValue(rep.regel6A);
            el.Element("regel_6_b").SetValue(rep.regel6B);
            el.Element("regel_6_c").SetValue(rep.regel6C);

            el.Element("regel_7_a").SetValue(rep.regel7A);
            el.Element("regel_7_b").SetValue(rep.regel7B);
            el.Element("regel_7_c").SetValue(rep.regel7C);

            el.Element("regel_8_a").SetValue(rep.regel8A);
            el.Element("regel_8_b").SetValue(rep.regel8B);
            el.Element("regel_8_c").SetValue(rep.regel8C);

            el.Element("regel_9_a").SetValue(rep.regel9A);
            el.Element("regel_9_b").SetValue(rep.regel9B);
            el.Element("regel_9_c").SetValue(rep.regel9C);

            el.Element("regel_10_a").SetValue(rep.regel10A);
            el.Element("regel_10_b").SetValue(rep.regel10B);
            el.Element("regel_10_c").SetValue(rep.regel10C);

            el.Element("regel_11_a").SetValue(rep.regel11A);
            el.Element("regel_11_b").SetValue(rep.regel11B);
            el.Element("regel_11_c").SetValue(rep.regel11C);

            el.Element("regel_12_a").SetValue(rep.regel12A);
            el.Element("regel_12_b").SetValue(rep.regel12B);
            el.Element("regel_12_c").SetValue(rep.regel12C);

            el.Element("regel_13_a").SetValue(rep.regel13A);
            el.Element("regel_13_b").SetValue(rep.regel13B);
            el.Element("regel_13_c").SetValue(rep.regel13C);

            el.Element("regel_14_a").SetValue(rep.regel14A);
            el.Element("regel_14_b").SetValue(rep.regel14B);
            el.Element("regel_14_c").SetValue(rep.regel14C);

            el.Element("regel_15").SetValue(rep.regel15);
            el.Element("regel_16").SetValue(rep.regel16);
            el.Element("regel_17").SetValue(rep.regel17);
            el.Element("regel_18").SetValue(rep.regel18);
            el.Element("regel_19").SetValue(rep.regel19);

            el.Element("regel_20").SetValue(rep.regel20);
            el.Element("regel_21").SetValue(rep.regel21);
            el.Element("regel_22").SetValue(rep.regel22);
            el.Element("regel_23").SetValue(rep.regel23);
            el.Element("regel_24").SetValue(rep.regel24);
            el.Element("regel_25").SetValue(rep.regel25);
            el.Element("regel_26").SetValue(rep.regel26);
            el.Element("regel_27").SetValue(rep.regel27);
            el.Element("regel_28").SetValue(rep.regel28);
            el.Element("regel_29").SetValue(rep.regel29);
            el.Element("regel_30").SetValue(rep.regel30);
            el.Element("regel_31").SetValue(rep.regel31);
            el.Element("regel_32").SetValue(rep.regel32);
            el.Element("regel_33").SetValue(rep.regel33);







            return el;
        }
        private InstitutionalDTO checkForNullValues(InstitutionalDTO test)
        {
            test.regel1A = test.regel1A == null ? "" : test.regel1A;
            test.regel1B = test.regel1B == null ? "" : test.regel1B;
            test.regel1C = test.regel1C == null ? "" : test.regel1C;

            test.regel2A = test.regel2A == null ? "" : test.regel2A;
            test.regel2B = test.regel2B == null ? "" : test.regel2B;
            test.regel2C = test.regel2C == null ? "" : test.regel2C;

            test.regel3A = test.regel3A == null ? "" : test.regel3A;
            test.regel3B = test.regel3B == null ? "" : test.regel3B;
            test.regel3C = test.regel3C == null ? "" : test.regel3C;

            test.regel4A = test.regel4A == null ? "" : test.regel4A;
            test.regel4B = test.regel4B == null ? "" : test.regel4B;
            test.regel4C = test.regel4C == null ? "" : test.regel4C;

            test.regel5A = test.regel5A == null ? "" : test.regel5A;
            test.regel5B = test.regel5B == null ? "" : test.regel5B;
            test.regel5C = test.regel5C == null ? "" : test.regel5C;

            test.regel6A = test.regel6A == null ? "" : test.regel6A;
            test.regel6B = test.regel6B == null ? "" : test.regel6B;
            test.regel6C = test.regel6C == null ? "" : test.regel6C;

            test.regel7A = test.regel7A == null ? "" : test.regel7A;
            test.regel7B = test.regel7B == null ? "" : test.regel7B;
            test.regel7C = test.regel7C == null ? "" : test.regel7C;

            test.regel8A = test.regel8A == null ? "" : test.regel8A;
            test.regel8B = test.regel8B == null ? "" : test.regel8B;
            test.regel8C = test.regel8C == null ? "" : test.regel8C;

            test.regel9A = test.regel9A == null ? "" : test.regel9A;
            test.regel9B = test.regel9B == null ? "" : test.regel9B;
            test.regel9C = test.regel9C == null ? "" : test.regel9C;

            test.regel10A = test.regel10A == null ? "" : test.regel10A;
            test.regel10B = test.regel10B == null ? "" : test.regel10B;
            test.regel10C = test.regel10C == null ? "" : test.regel10C;

            test.regel11A = test.regel11A == null ? "" : test.regel11A;
            test.regel11B = test.regel11B == null ? "" : test.regel11B;
            test.regel11C = test.regel11C == null ? "" : test.regel11C;

            test.regel12A = test.regel12A == null ? "" : test.regel12A;
            test.regel12B = test.regel12B == null ? "" : test.regel12B;
            test.regel12C = test.regel12C == null ? "" : test.regel12C;

            test.regel13A = test.regel13A == null ? "" : test.regel13A;
            test.regel13B = test.regel13B == null ? "" : test.regel13B;
            test.regel13C = test.regel13C == null ? "" : test.regel13C;

            test.regel14A = test.regel14A == null ? "" : test.regel14A;
            test.regel14B = test.regel14B == null ? "" : test.regel14B;
            test.regel14C = test.regel14C == null ? "" : test.regel14C;

            test.regel15 = test.regel15 == null ? "" : test.regel15;
            test.regel16 = test.regel16 == null ? "" : test.regel16;
            test.regel17 = test.regel17 == null ? "" : test.regel17;
            test.regel18 = test.regel18 == null ? "" : test.regel18;
            test.regel19 = test.regel19 == null ? "" : test.regel19;
            test.regel20 = test.regel20 == null ? "" : test.regel20;

            test.regel21 = test.regel21 == null ? "" : test.regel21;
            test.regel22 = test.regel22 == null ? "" : test.regel22;
            test.regel23 = test.regel23 == null ? "" : test.regel23;
            test.regel24 = test.regel24 == null ? "" : test.regel24;
            test.regel25 = test.regel25 == null ? "" : test.regel25;
            test.regel26 = test.regel26 == null ? "" : test.regel26;
            test.regel27 = test.regel27 == null ? "" : test.regel27;
            test.regel28 = test.regel28 == null ? "" : test.regel28;
            test.regel29 = test.regel29 == null ? "" : test.regel29;
            test.regel30 = test.regel30 == null ? "" : test.regel30;
            test.regel31 = test.regel31 == null ? "" : test.regel31;
            test.regel32 = test.regel32 == null ? "" : test.regel32;
            test.regel33 = test.regel33 == null ? "" : test.regel33;


            return test;
        }
        #endregion

    }
}