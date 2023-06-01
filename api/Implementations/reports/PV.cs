
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
                /* if (await DeleteAsync(sl) == 1)
                {
                    return await getPreViewAsync(procedure_id);
                }
                else { return null; } */
            }
            return await getPreViewAsync(procedure_id);


        }

        public async Task<InstitutionalDTO> getInstitutionalReport(int hospitalNo, int soort)
        {

            int type = 0;
            switch (soort){
                case 1: type = 0;break;
                case 2: type = 1;break;
                case 3: type = 2;break;
                case 30: type = 3;break;
                case 4: type = 4;break;
                case 41: type = 5;break;
                case 5: type = 6;break;
                case 51: type = 7;break;
            }
            Hospital rep = new Hospital();
            //  get the XML file in memory
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
            XDocument order = XDocument.Load(filename);
            // select hospital where id attribute = hospitalNo
            IEnumerable<XElement> help = from d in order.Descendants("hospital")
                                         where d.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar()
                                         select d;
            // serialize to test
            foreach (XElement x in help)
            {
                var serializer = new XmlSerializer(typeof(Hospital));
                rep = (Hospital)serializer.Deserialize(x.CreateReader());
            }
            // construct dto
            InstitutionalDTO it = new InstitutionalDTO();

            it.Regel1A = rep.Reports.TextByTypeOfSurgery.Soort[type].Regel1A;
            it.Regel1B = rep.Reports.TextByTypeOfSurgery.Soort[type].Regel1B;
            it.Regel1C = rep.Reports.TextByTypeOfSurgery.Soort[type].Regel1C;
            it.Regel2A = rep.Reports.TextByTypeOfSurgery.Soort[type].Regel2A;

            return it;
        }

        public async Task<string> createInstitutionalReport(int hospitalNo)
        {
            throw new NotImplementedException();
        }

        public async Task<string> updateInstitutionalReport(InstitutionalReport rep)
        {
            throw new NotImplementedException();
        }
    }
}