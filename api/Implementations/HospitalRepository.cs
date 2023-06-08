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

        public async Task<InstitutionalDTO> getInstitutionalReport(int hospitalNo, int soort)
        {
            InstitutionalDTO it = new InstitutionalDTO();
            await Task.Run(() =>
            {

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

            });
            return it;
        }

        public async Task<AdditionalReportDTO> getAdditionalReportItems(int hospitalNo, int which)
        {
            var l = new List<string>();
            var ar = new AdditionalReportDTO();

            await Task.Run(() =>
            {
                var contentRoot = _env.ContentRootPath;
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
                    }

                    if (l.Count > 0)
                    {
                        ar.line_1 = l[0];
                        if (l.Count > 1)
                        {
                            ar.line_2 = l[1];
                            if (l.Count > 2)
                            {
                                ar.line_3 = l[2];
                                if (l.Count > 3)
                                {
                                    ar.line_4 = l[3];
                                    if (l.Count > 4)
                                    {
                                        ar.line_5 = l[4];
                                       
                                    }
                                }
                            }
                        }
                    }
                }
            });

            return ar;
        }

        public async Task<string> updateAdditionalReportItem(AdditionalReportDTO up, int hospitalNo, int which)
        {
            up = checkforNullInAdditionalReport(up);
            await Task.Run(() =>
            {
                // this used to send hospital-specific details about pm-wires, iabp and circulatory support

                var contentRoot = _env.ContentRootPath;
                var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
                XDocument doc = XDocument.Load(filename);
                // see if there is already an XElelemt with this id
                IEnumerable<XElement> test = from d in doc.Descendants("hospital")
                                             where d.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar()
                                             select d;
                foreach (XElement org in test)
                {
                    switch (which)
                    {
                        case 1:
                            // get the circulatory-support
                            IEnumerable<XElement> help1 = from d in org
                                                .Elements("reports")
                                                .Elements("circulation_support").Elements("items")
                                                          select d;
                            foreach (XElement f in help1)
                            {
                                if (f.Attribute("id").Value == "1") { f.Element("regel_21").SetValue(up.line_1); };
                                if (f.Attribute("id").Value == "2") { f.Element("regel_21").SetValue(up.line_2); };
                                if (f.Attribute("id").Value == "3") { f.Element("regel_21").SetValue(up.line_3); };
                                if (f.Attribute("id").Value == "4") { f.Element("regel_21").SetValue(up.line_4); };
                                if (f.Attribute("id").Value == "5") { f.Element("regel_21").SetValue(up.line_5); };
                            };
                            doc.Save(filename);
                            break;

                        case 2:

                            IEnumerable<XElement> help2 = from d in org
                                                .Elements("reports")
                                                .Elements("iabp").Elements("items")
                                                          select d;
                            foreach (XElement f in help2)
                            {

                                if (f.Attribute("id").Value == "1") { f.Element("regel_22").SetValue(up.line_1); };
                                if (f.Attribute("id").Value == "2") { f.Element("regel_22").SetValue(up.line_2); };
                                if (f.Attribute("id").Value == "3") { f.Element("regel_22").SetValue(up.line_3); };
                                if (f.Attribute("id").Value == "4") { f.Element("regel_22").SetValue(up.line_4); };
                                if (f.Attribute("id").Value == "5") { f.Element("regel_22").SetValue(up.line_5); };
                           
                            };
                            doc.Save(filename);
                            break;
                        case 3:

                            IEnumerable<XElement> help3 = from d in org
                                               .Elements("reports")
                                               .Elements("pmwires").Elements("items")
                                                          select d;
                            foreach (XElement f in help3)
                            {
                                if (f.Attribute("id").Value == "1") { f.Element("regel_23").SetValue(up.line_1); };
                                if (f.Attribute("id").Value == "2") { f.Element("regel_23").SetValue(up.line_2); };
                                if (f.Attribute("id").Value == "3") { f.Element("regel_23").SetValue(up.line_3); };
                                if (f.Attribute("id").Value == "4") { f.Element("regel_23").SetValue(up.line_4); };
                                if (f.Attribute("id").Value == "5") { f.Element("regel_23").SetValue(up.line_5); };
                               

                            };
                            doc.Save(filename);
                            break;
                    }
                }
            });

            return "";
        }

        private AdditionalReportDTO checkforNullInAdditionalReport(AdditionalReportDTO up)
        {

            up.line_1 = up.line_1 == null ? "" : up.line_1;
            up.line_2 = up.line_2 == null ? "" : up.line_2;
            up.line_3 = up.line_3 == null ? "" : up.line_3;
            up.line_4 = up.line_4 == null ? "" : up.line_4;
            up.line_5 = up.line_5 == null ? "" : up.line_5;
            return up;
        }

        public async Task<string> createInstitutionalReport(int hospitalNo)
        {
            var result = "";
            await Task.Run(() =>
            {

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

            });
            return result;
        }

        public async Task<string> updateInstitutionalReport(InstitutionalDTO rep, int soort, int hospitalNo)
        {
            await Task.Run(() =>
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
            });
            return "";
        }

        private InstitutionalDTO getIDTO(InstitutionalDTO it, XElement el)
        {
            it.Regel1A = el.Element("regel_1_a").Value;
            it.Regel1B = el.Element("regel_1_b").Value;
            it.Regel1C = el.Element("regel_1_c").Value;

            it.Regel2A = el.Element("regel_2_a").Value;
            it.Regel2B = el.Element("regel_2_b").Value;
            it.Regel2C = el.Element("regel_2_c").Value;

            it.Regel3A = el.Element("regel_3_a").Value;
            it.Regel3B = el.Element("regel_3_b").Value;
            it.Regel3C = el.Element("regel_3_c").Value;

            it.Regel4A = el.Element("regel_4_a").Value;
            it.Regel4B = el.Element("regel_4_b").Value;
            it.Regel4C = el.Element("regel_4_c").Value;

            it.Regel5A = el.Element("regel_5_a").Value;
            it.Regel5B = el.Element("regel_5_b").Value;
            it.Regel5C = el.Element("regel_5_c").Value;

            it.Regel6A = el.Element("regel_6_a").Value;
            it.Regel6B = el.Element("regel_6_b").Value;
            it.Regel6C = el.Element("regel_6_c").Value;

            it.Regel7A = el.Element("regel_7_a").Value;
            it.Regel7B = el.Element("regel_7_b").Value;
            it.Regel7C = el.Element("regel_7_c").Value;

            it.Regel8A = el.Element("regel_8_a").Value;
            it.Regel8B = el.Element("regel_8_b").Value;
            it.Regel8C = el.Element("regel_8_c").Value;

            it.Regel9A = el.Element("regel_9_a").Value;
            it.Regel9B = el.Element("regel_9_b").Value;
            it.Regel9C = el.Element("regel_9_c").Value;

            it.Regel10A = el.Element("regel_10_a").Value;
            it.Regel10B = el.Element("regel_10_b").Value;
            it.Regel10C = el.Element("regel_10_c").Value;

            it.Regel11A = el.Element("regel_11_a").Value;
            it.Regel11B = el.Element("regel_11_b").Value;
            it.Regel11C = el.Element("regel_11_c").Value;

            it.Regel12A = el.Element("regel_12_a").Value;
            it.Regel12B = el.Element("regel_12_b").Value;
            it.Regel12C = el.Element("regel_12_c").Value;

            it.Regel13A = el.Element("regel_13_a").Value;
            it.Regel13B = el.Element("regel_13_b").Value;
            it.Regel13C = el.Element("regel_13_c").Value;

            it.Regel14A = el.Element("regel_14_a").Value;
            it.Regel14B = el.Element("regel_14_b").Value;
            it.Regel14C = el.Element("regel_14_c").Value;

            it.Regel15 = el.Element("regel_15").Value;
            it.Regel16 = el.Element("regel_16").Value;
            it.Regel17 = el.Element("regel_17").Value;
            it.Regel18 = el.Element("regel_18").Value;
            it.Regel19 = el.Element("regel_19").Value;
            it.Regel20 = el.Element("regel_20").Value;
            it.Regel21 = el.Element("regel_21").Value;
            it.Regel22 = el.Element("regel_22").Value;
            it.Regel23 = el.Element("regel_23").Value;
            it.Regel24 = el.Element("regel_24").Value;
            it.Regel25 = el.Element("regel_25").Value;
            it.Regel26 = el.Element("regel_26").Value;
            it.Regel27 = el.Element("regel_27").Value;
            it.Regel28 = el.Element("regel_28").Value;
            it.Regel29 = el.Element("regel_29").Value;
            it.Regel30 = el.Element("regel_30").Value;
            it.Regel31 = el.Element("regel_31").Value;
            it.Regel32 = el.Element("regel_32").Value;
            it.Regel33 = el.Element("regel_33").Value;



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

            el.Element("regel_1_a").SetValue(rep.Regel1A);
            el.Element("regel_1_b").SetValue(rep.Regel1B);
            el.Element("regel_1_c").SetValue(rep.Regel1C);

            el.Element("regel_2_a").SetValue(rep.Regel2A);
            el.Element("regel_2_b").SetValue(rep.Regel2B);
            el.Element("regel_2_c").SetValue(rep.Regel2C);

            el.Element("regel_3_a").SetValue(rep.Regel3A);
            el.Element("regel_3_b").SetValue(rep.Regel3B);
            el.Element("regel_3_c").SetValue(rep.Regel3C);

            el.Element("regel_4_a").SetValue(rep.Regel4A);
            el.Element("regel_4_b").SetValue(rep.Regel4B);
            el.Element("regel_4_c").SetValue(rep.Regel4C);

            el.Element("regel_5_a").SetValue(rep.Regel5A);
            el.Element("regel_5_b").SetValue(rep.Regel5B);
            el.Element("regel_5_c").SetValue(rep.Regel5C);

            el.Element("regel_6_a").SetValue(rep.Regel6A);
            el.Element("regel_6_b").SetValue(rep.Regel6B);
            el.Element("regel_6_c").SetValue(rep.Regel6C);

            el.Element("regel_7_a").SetValue(rep.Regel7A);
            el.Element("regel_7_b").SetValue(rep.Regel7B);
            el.Element("regel_7_c").SetValue(rep.Regel7C);

            el.Element("regel_8_a").SetValue(rep.Regel8A);
            el.Element("regel_8_b").SetValue(rep.Regel8B);
            el.Element("regel_8_c").SetValue(rep.Regel8C);

            el.Element("regel_9_a").SetValue(rep.Regel9A);
            el.Element("regel_9_b").SetValue(rep.Regel9B);
            el.Element("regel_9_c").SetValue(rep.Regel9C);

            el.Element("regel_10_a").SetValue(rep.Regel10A);
            el.Element("regel_10_b").SetValue(rep.Regel10B);
            el.Element("regel_10_c").SetValue(rep.Regel10C);

            el.Element("regel_11_a").SetValue(rep.Regel11A);
            el.Element("regel_11_b").SetValue(rep.Regel11B);
            el.Element("regel_11_c").SetValue(rep.Regel11C);

            el.Element("regel_12_a").SetValue(rep.Regel12A);
            el.Element("regel_12_b").SetValue(rep.Regel12B);
            el.Element("regel_12_c").SetValue(rep.Regel12C);

            el.Element("regel_13_a").SetValue(rep.Regel13A);
            el.Element("regel_13_b").SetValue(rep.Regel13B);
            el.Element("regel_13_c").SetValue(rep.Regel13C);

            el.Element("regel_14_a").SetValue(rep.Regel14A);
            el.Element("regel_14_b").SetValue(rep.Regel14B);
            el.Element("regel_14_c").SetValue(rep.Regel14C);

            el.Element("regel_15").SetValue(rep.Regel15);
            el.Element("regel_16").SetValue(rep.Regel16);
            el.Element("regel_17").SetValue(rep.Regel17);
            el.Element("regel_18").SetValue(rep.Regel18);
            el.Element("regel_19").SetValue(rep.Regel19);

            el.Element("regel_20").SetValue(rep.Regel20);
            el.Element("regel_21").SetValue(rep.Regel21);
            el.Element("regel_22").SetValue(rep.Regel22);
            el.Element("regel_23").SetValue(rep.Regel23);
            el.Element("regel_24").SetValue(rep.Regel24);
            el.Element("regel_25").SetValue(rep.Regel25);
            el.Element("regel_26").SetValue(rep.Regel26);
            el.Element("regel_27").SetValue(rep.Regel27);
            el.Element("regel_28").SetValue(rep.Regel28);
            el.Element("regel_29").SetValue(rep.Regel29);
            el.Element("regel_30").SetValue(rep.Regel30);
            el.Element("regel_31").SetValue(rep.Regel31);
            el.Element("regel_32").SetValue(rep.Regel32);
            el.Element("regel_33").SetValue(rep.Regel33);







            return el;
        }

        private InstitutionalDTO checkForNullValues(InstitutionalDTO test)
        {
            test.Regel1A = test.Regel1A == null ? "" : test.Regel1A;
            test.Regel1B = test.Regel1B == null ? "" : test.Regel1B;
            test.Regel1C = test.Regel1C == null ? "" : test.Regel1C;

            test.Regel2A = test.Regel2A == null ? "" : test.Regel2A;
            test.Regel2B = test.Regel2B == null ? "" : test.Regel2B;
            test.Regel2C = test.Regel2C == null ? "" : test.Regel2C;

            test.Regel3A = test.Regel3A == null ? "" : test.Regel3A;
            test.Regel3B = test.Regel3B == null ? "" : test.Regel3B;
            test.Regel3C = test.Regel3C == null ? "" : test.Regel3C;

            test.Regel4A = test.Regel4A == null ? "" : test.Regel4A;
            test.Regel4B = test.Regel4B == null ? "" : test.Regel4B;
            test.Regel4C = test.Regel4C == null ? "" : test.Regel4C;

            test.Regel5A = test.Regel5A == null ? "" : test.Regel5A;
            test.Regel5B = test.Regel5B == null ? "" : test.Regel5B;
            test.Regel5C = test.Regel5C == null ? "" : test.Regel5C;

            test.Regel6A = test.Regel6A == null ? "" : test.Regel6A;
            test.Regel6B = test.Regel6B == null ? "" : test.Regel6B;
            test.Regel6C = test.Regel6C == null ? "" : test.Regel6C;

            test.Regel7A = test.Regel7A == null ? "" : test.Regel7A;
            test.Regel7B = test.Regel7B == null ? "" : test.Regel7B;
            test.Regel7C = test.Regel7C == null ? "" : test.Regel7C;

            test.Regel8A = test.Regel8A == null ? "" : test.Regel8A;
            test.Regel8B = test.Regel8B == null ? "" : test.Regel8B;
            test.Regel8C = test.Regel8C == null ? "" : test.Regel8C;

            test.Regel9A = test.Regel9A == null ? "" : test.Regel9A;
            test.Regel9B = test.Regel9B == null ? "" : test.Regel9B;
            test.Regel9C = test.Regel9C == null ? "" : test.Regel9C;

            test.Regel10A = test.Regel10A == null ? "" : test.Regel10A;
            test.Regel10B = test.Regel10B == null ? "" : test.Regel10B;
            test.Regel10C = test.Regel10C == null ? "" : test.Regel10C;

            test.Regel11A = test.Regel11A == null ? "" : test.Regel11A;
            test.Regel11B = test.Regel11B == null ? "" : test.Regel11B;
            test.Regel11C = test.Regel11C == null ? "" : test.Regel11C;

            test.Regel12A = test.Regel12A == null ? "" : test.Regel12A;
            test.Regel12B = test.Regel12B == null ? "" : test.Regel12B;
            test.Regel12C = test.Regel12C == null ? "" : test.Regel12C;

            test.Regel13A = test.Regel13A == null ? "" : test.Regel13A;
            test.Regel13B = test.Regel13B == null ? "" : test.Regel13B;
            test.Regel13C = test.Regel13C == null ? "" : test.Regel13C;

            test.Regel14A = test.Regel14A == null ? "" : test.Regel14A;
            test.Regel14B = test.Regel14B == null ? "" : test.Regel14B;
            test.Regel14C = test.Regel14C == null ? "" : test.Regel14C;

            test.Regel15 = test.Regel15 == null ? "" : test.Regel15;
            test.Regel16 = test.Regel16 == null ? "" : test.Regel16;
            test.Regel17 = test.Regel17 == null ? "" : test.Regel17;
            test.Regel18 = test.Regel18 == null ? "" : test.Regel18;
            test.Regel19 = test.Regel19 == null ? "" : test.Regel19;
            test.Regel20 = test.Regel20 == null ? "" : test.Regel20;

            test.Regel21 = test.Regel21 == null ? "" : test.Regel21;
            test.Regel22 = test.Regel22 == null ? "" : test.Regel22;
            test.Regel23 = test.Regel23 == null ? "" : test.Regel23;
            test.Regel24 = test.Regel24 == null ? "" : test.Regel24;
            test.Regel25 = test.Regel25 == null ? "" : test.Regel25;
            test.Regel26 = test.Regel26 == null ? "" : test.Regel26;
            test.Regel27 = test.Regel27 == null ? "" : test.Regel27;
            test.Regel28 = test.Regel28 == null ? "" : test.Regel28;
            test.Regel29 = test.Regel29 == null ? "" : test.Regel29;
            test.Regel30 = test.Regel30 == null ? "" : test.Regel30;
            test.Regel31 = test.Regel31 == null ? "" : test.Regel31;
            test.Regel32 = test.Regel32 == null ? "" : test.Regel32;
            test.Regel33 = test.Regel33 == null ? "" : test.Regel33;


            return test;
        }

        #endregion

    }
}