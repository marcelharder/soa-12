
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Operators;

namespace api.Data
{
    public class PV : IPV
    {
        private DataContext _context;
        private SpecialReportMaps _special;
        private SpecialMaps _sm;
        private IMapper _map;
        private IInstitutionalText _text;

        private IWebHostEnvironment _env;
        public PV(
            DataContext context,
            IWebHostEnvironment env,
            SpecialReportMaps special,
            IMapper map,
            IInstitutionalText text,
            SpecialMaps sm)
        {
            _context = context;
            _special = special;
            _map = map;
            _text = text;
            _sm = sm;
            _env = env;
        }


        public async Task<Class_Preview_Operative_report> getPreViewAsync(int id)
        {
            // check if there is a record for this preview procedure in the database
            if (await _context.Previews.AnyAsync(u => u.procedure_id == id))
            {
                return await _context.Previews.FirstOrDefaultAsync(u => u.procedure_id == id);
            }
            else
            {
                var result = new Class_Preview_Operative_report();
                result.procedure_id = id;
                var currentProcedure = await _context.Procedures.Where(x => x.ProcedureId == id).FirstOrDefaultAsync();
                var userId = _special.getCurrentUserId();
                var report_code = Convert.ToInt32(_special.getReportCode(currentProcedure.fdType));

                // check if this user has a suggestion for this type of procedure
                if (await getSuggestionForThisProcedureAsync(userId.ToString(), currentProcedure.fdType))
                {
                    // return the stored suggestion for this surgeon and soort.
                    var special_procedure_suggestion = await _context.Suggestions
                                        .Where(x => x.user == userId.ToString())
                                        .Where(x => x.soort == currentProcedure.fdType)
                                        .FirstOrDefaultAsync();
                    result = _map.Map<Class_Suggestion, Class_Preview_Operative_report>(special_procedure_suggestion);
                    result.procedure_id = id;
                    _context.Previews.Add(result);
                    if (await SaveAll())
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                // this user has no suggestion for this type of procedure
                {
                    if (report_code == 6)
                    {
                        // doe niks
                        result.regel_1 = "Please enter your custom report here and 'Save as suggestion'";
                        _context.Previews.Add(result);
                        await SaveAll();
                        return result;
                    }
                    else
                    {
                        // get the institutional text from the xml file
                        var text = await _text.getText(await _sm.getCurrentHospitalIdAsync(), currentProcedure.fdType.ToString(), id);

                        result.regel_1 = text[0]; result.regel_2 = text[1]; result.regel_3 = text[2]; result.regel_4 = text[3]; result.regel_5 = text[4];
                        result.regel_6 = text[5]; result.regel_7 = text[6]; result.regel_8 = text[7]; result.regel_9 = text[8]; result.regel_10 = text[9];

                        result.regel_11 = text[10]; result.regel_12 = text[11]; result.regel_13 = text[12]; result.regel_14 = text[13]; result.regel_15 = text[14];
                        result.regel_16 = text[15]; result.regel_17 = text[16]; result.regel_18 = text[17]; result.regel_19 = text[18]; result.regel_20 = text[19];

                        result.regel_21 = text[20]; result.regel_22 = text[21]; result.regel_23 = text[22]; result.regel_24 = text[23]; result.regel_25 = text[24];
                        result.regel_26 = text[25]; result.regel_27 = text[26]; result.regel_28 = text[27]; result.regel_29 = text[28]; result.regel_30 = text[29];

                        result.regel_31 = text[30]; result.regel_32 = text[31]; result.regel_33 = text[32];

                        _context.Previews.Add(result);
                        await SaveAll();
                        return result;

                    }
                }

            }

        }

        private Class_Preview_Operative_report addExtraLines(Class_Preview_Operative_report help, int reportCode)
        {
            // add the extra lines for the CABG Valve and other procedures.
            switch (reportCode)
            {
                case 21: help.regel_1 = "needs to be implemented for extra procedures no 21....."; break;
                case 23: help.regel_1 = "needs to be implemented for extra procedures no 23....."; break;
                case 24: help.regel_1 = "needs to be implemented for extra procedures no 24....."; break;
                case 25: help.regel_1 = "needs to be implemented for extra procedures no 25....."; break;
                case 26: help.regel_1 = "needs to be implemented for extra procedures no 26....."; break;
            }
            return help;
        }

        public async Task<int> updatePVR(Class_Preview_Operative_report cp)
        {
            _context.Update(cp);
            if (await this.SaveAll())
            {
                return 1;
            }
            return 0;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
            if (await SaveAll()) { return 1; } else { return 0; }
        }

        private async Task<bool> getSuggestionForThisProcedureAsync(string surgeon, int soort)
        {
            var help = false;
            var test = await _context.Suggestions
                                    .Where(x => x.user == surgeon)
                                    .Where(x => x.soort == soort)
                                    .FirstOrDefaultAsync();
            if (test != null) { help = true; }
            return help;
        }

        [Authorize]
        public async Task<Class_Preview_Operative_report> resetPreViewAsync(int procedure_id)
        {
            var sl = await _context.Previews.FirstOrDefaultAsync(x => x.procedure_id == procedure_id);
            if (sl != null)
            {
                await DeleteAsync(sl);
            }
            return await getPreViewAsync(procedure_id);


        }

        #region <!--institutional report suggestions -->
        
        public async Task<InstitutionalDTO> getInstitutionalReport(int soort, int hospitalNo)
        {
            await Task.Run(() =>
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
            });
            return null;
        }

        public async Task<List<Class_Item>> getAdditionalReportItems(int hospitalNo, int soort, int which)
        {
            await Task.Run(() =>
            {
                // this used to send hospital-specific details about pm-wires, iabp and circulatory support
                var l = new List<Class_Item>();
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
                            IEnumerable<XElement> help1 = from d in org
                                            .Elements("reports")
                                            .Elements("circulation_support").Elements("items")
                                                          select d;
                            foreach (XElement f in help1)
                            {
                                Class_Item drop = new Class_Item();
                                drop.value = Convert.ToInt32(f.Attribute("id").Value);
                                drop.description = f.Element("regel_21").Value;
                                l.Add(drop);
                            }; break;
                        case 2:
                            IEnumerable<XElement> help2 = from d in org
                                            .Elements("reports")
                                            .Elements("iabp").Elements("items")
                                                          select d;
                            foreach (XElement f in help2)
                            {
                                Class_Item drop = new Class_Item();
                                drop.value = Convert.ToInt32(f.Attribute("id").Value);
                                drop.description = f.Element("regel_22").Value;
                                l.Add(drop);
                            }; break;

                        case 3:
                            IEnumerable<XElement> help3 = from d in org
                                            .Elements("reports")
                                            .Elements("pmwires").Elements("items")
                                                          select d;
                            foreach (XElement f in help3)
                            {
                                Class_Item drop = new Class_Item();
                                drop.value = Convert.ToInt32(f.Attribute("id").Value);
                                drop.description = f.Element("regel_23").Value;
                                l.Add(drop);
                            }; break;
                    }
                }

                return l;
            });
            return null;
        }

        public async Task<string> updateAdditionalReportItem(AdditionalReportDTO up, int hospitalNo, int soort, int which)
        {
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
                             if(f.Attribute("id").Value == "1"){f.Element("regel_21").SetValue(up.line_1);}; 
                             if(f.Attribute("id").Value == "2"){f.Element("regel_21").SetValue(up.line_2);}; 
                             if(f.Attribute("id").Value == "3"){f.Element("regel_21").SetValue(up.line_3);}; 
                             if(f.Attribute("id").Value == "4"){f.Element("regel_21").SetValue(up.line_4);}; 
                             if(f.Attribute("id").Value == "5"){f.Element("regel_21").SetValue(up.line_5);}; 
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
                             if(f.Attribute("id").Value == "1"){f.Element("regel_22").SetValue(up.line_1);}; 
                             if(f.Attribute("id").Value == "2"){f.Element("regel_22").SetValue(up.line_2);}; 
                             if(f.Attribute("id").Value == "3"){f.Element("regel_22").SetValue(up.line_3);}; 
                             if(f.Attribute("id").Value == "4"){f.Element("regel_22").SetValue(up.line_4);}; 
                             if(f.Attribute("id").Value == "5"){f.Element("regel_22").SetValue(up.line_5);};  
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
                             if(f.Attribute("id").Value == "1"){f.Element("regel_23").SetValue(up.line_1);}; 
                             if(f.Attribute("id").Value == "2"){f.Element("regel_23").SetValue(up.line_2);}; 
                             if(f.Attribute("id").Value == "3"){f.Element("regel_23").SetValue(up.line_3);}; 
                             if(f.Attribute("id").Value == "4"){f.Element("regel_23").SetValue(up.line_4);}; 
                             if(f.Attribute("id").Value == "5"){f.Element("regel_23").SetValue(up.line_5);};  
                      
                            }; 
                            doc.Save(filename);
                            break;
                    }
                }
            });

            return "";
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