using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using api.Entities;
using api.Interfaces;
using api.Data;
using api.DTOs;
using Microsoft.AspNetCore.Identity;

namespace api.Helpers
{

    public class SpecialReportMaps
    {
        private IHttpContextAccessor _http;
        private IMapper _map;
        private UserManager<AppUser> _userManager;

        private IUserRepository _user;
        private IHospitalRepository _hos;
        private IEmployeeRepository _emp;
        private DataContext _context;
        private OperatieDrops _drops;

        private IWebHostEnvironment _env;
        public SpecialReportMaps(

            IHttpContextAccessor http,
            IMapper map,
            IEmployeeRepository emp,
            IUserRepository user,
            IHospitalRepository hos,
            OperatieDrops drops,
            UserManager<AppUser> userManager,
            IWebHostEnvironment env,
            DataContext context)
        {
            _http = http;
            _map = map;
            _env = env;
            _context = context;
            _drops = drops;
            _hos = hos;
            _user = user;
            _emp = emp;
            _userManager = userManager;



        }



        public int getCurrentUserId()
        {
            var userId = _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt32(userId);
        }
        internal Class_Preview_Operative_report mapToClassPreviewOperativeReport(PreviewForReturnDTO pvfr, Class_Preview_Operative_report cp)
        {
            return _map.Map<PreviewForReturnDTO, Class_Preview_Operative_report>(pvfr, cp);
        }
        internal Class_privacy_model mapToClassPrivacyModel(PreviewForReturnDTO pvfr)
        {
            return _map.Map<PreviewForReturnDTO, Class_privacy_model>(pvfr);
        }
        internal Class_Item mapSuggestionToClassItem(Class_Suggestion sug)
        {
            var help = new Class_Item();
            help.value = sug.soort;
            help.description = getProcedureDescription(sug.soort);
            return help;
        }
        public string getReportCode(int fdType)
        {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/procedure.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Code")
                                         where d.Element("ID").Value == fdType.ToString()
                                         select d;
            foreach (XElement x in help) { result = x.Element("report_code").Value; }
            return result;
        }
        public string getProcedureDescription(int soort)
        {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/procedure.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Code")
                                         where d.Element("ID").Value == soort.ToString()
                                         select d;
            foreach (XElement x in help) { result = x.Element("Description").Value; }
            return result;
        }
        public async Task<Class_Preview_Operative_report> useCustomSuggestion(Class_Preview_Operative_report pvr, Class_Suggestion sug)
        {
            await Task.Run(() =>
            {
                pvr.regel_1 = sug.regel_1_a + sug.regel_1_b + sug.regel_1_c;
                pvr.regel_2 = sug.regel_2_a + sug.regel_2_b + sug.regel_2_c;
                pvr.regel_3 = sug.regel_3_a + sug.regel_3_b + sug.regel_3_c;
                pvr.regel_4 = sug.regel_4_a + sug.regel_4_b + sug.regel_4_c;
                pvr.regel_5 = sug.regel_5_a + sug.regel_5_b + sug.regel_5_c;
                pvr.regel_6 = sug.regel_6_a + sug.regel_6_b + sug.regel_6_c;
                pvr.regel_7 = sug.regel_7_a + sug.regel_7_b + sug.regel_7_c;
                pvr.regel_8 = sug.regel_8_a + sug.regel_8_b + sug.regel_8_c;
                pvr.regel_9 = sug.regel_9_a + sug.regel_9_b + sug.regel_9_c;
                pvr.regel_10 = sug.regel_10_a + sug.regel_10_b + sug.regel_10_c;
                pvr.regel_11 = sug.regel_11_a + sug.regel_11_b + sug.regel_11_c;
                pvr.regel_12 = sug.regel_12_a + sug.regel_12_b + sug.regel_12_c;
                pvr.regel_13 = sug.regel_13_a + sug.regel_13_b + sug.regel_13_c;
                pvr.regel_14 = sug.regel_14_a + sug.regel_14_b + sug.regel_14_c;
                pvr.regel_15 = sug.regel_15;
                pvr.regel_16 = sug.regel_16;
                pvr.regel_17 = sug.regel_17;
                pvr.regel_18 = sug.regel_18;
                pvr.regel_19 = sug.regel_19;
                pvr.regel_20 = sug.regel_20;
                pvr.regel_21 = sug.regel_21;
                pvr.regel_22 = sug.regel_22;
                pvr.regel_23 = sug.regel_23;
                pvr.regel_24 = sug.regel_24;
                pvr.regel_25 = sug.regel_25;
                pvr.regel_26 = sug.regel_26;
                pvr.regel_27 = sug.regel_27;
                pvr.regel_28 = sug.regel_28;
                pvr.regel_29 = sug.regel_29;
                pvr.regel_30 = sug.regel_30;
                pvr.regel_31 = sug.regel_31;
                pvr.regel_32 = sug.regel_32;
                pvr.regel_33 = sug.regel_33;
            });
            return pvr;
        }
        public async Task<Class_Suggestion> TransformPVRToSuggestion(Class_Preview_Operative_report c, string userid, int reportCode)
        {
            var help = new Class_Suggestion();
            await Task.Run(() =>
            {
                help.user = userid;
                help.soort = reportCode;
                help.regel_1_a = c.regel_1;
                help.regel_2_a = c.regel_2;
                help.regel_3_a = c.regel_3;
                help.regel_4_a = c.regel_4;
                help.regel_5_a = c.regel_5;
                help.regel_6_a = c.regel_6;
                help.regel_7_a = c.regel_7;
                help.regel_8_a = c.regel_8;
                help.regel_9_a = c.regel_9;
                help.regel_10_a = c.regel_10;
                help.regel_11_a = c.regel_11;
                help.regel_12_a = c.regel_12;
                help.regel_13_a = c.regel_13;
                help.regel_14_a = c.regel_14;
                help.regel_15 = c.regel_15;
                help.regel_16 = c.regel_16;
                help.regel_17 = c.regel_17;
                help.regel_18 = c.regel_18;
                help.regel_19 = c.regel_19;
                help.regel_20 = c.regel_20;
                help.regel_21 = c.regel_21;
                help.regel_22 = c.regel_22;
                help.regel_23 = c.regel_23;
                help.regel_24 = c.regel_24;
                help.regel_25 = c.regel_25;
                help.regel_26 = c.regel_26;
                help.regel_27 = c.regel_27;
                help.regel_28 = c.regel_28;
                help.regel_29 = c.regel_29;
                help.regel_30 = c.regel_30;
                help.regel_31 = c.regel_31;
                help.regel_32 = c.regel_32;
                help.regel_33 = c.regel_33;

                help.regel_1_b = ""; help.regel_1_c = "";
                help.regel_2_b = ""; help.regel_2_c = "";
                help.regel_3_b = ""; help.regel_3_c = "";
                help.regel_4_b = ""; help.regel_4_c = "";
                help.regel_5_b = ""; help.regel_5_c = "";
                help.regel_6_b = ""; help.regel_6_c = "";
                help.regel_7_b = ""; help.regel_7_c = "";
                help.regel_8_b = ""; help.regel_8_c = "";
                help.regel_9_b = ""; help.regel_9_c = "";
                help.regel_10_b = ""; help.regel_10_c = "";
                help.regel_11_b = ""; help.regel_11_c = "";
                help.regel_12_b = ""; help.regel_12_c = "";
                help.regel_13_b = ""; help.regel_13_c = "";
                help.regel_14_b = ""; help.regel_14_c = "";


            });
            return help;
        }
        public async Task<Class_Preview_Operative_report> useGenericSuggestion(Class_Preview_Operative_report pvr)
        {
            var selectedProcedure = await _context.Procedures.Where(s => s.ProcedureId == pvr.procedure_id).FirstOrDefaultAsync();
            var reportCode = getReportCode(selectedProcedure.fdType);
            var sug = new Class_Suggestion();
            if (reportCode == "6") { sug = getProcedureSpecificSuggestion(selectedProcedure.fdType); }
            else { sug = getGenericSuggestionAsync(Convert.ToInt32(reportCode)); }

            await Task.Run(async () =>
            {
                pvr.regel_1 = sug.regel_1_a + sug.regel_1_b + await HarvestLocationAsync(pvr.procedure_id) + sug.regel_1_c;
                pvr.regel_2 = sug.regel_2_a + sug.regel_2_b + sug.regel_2_c;
                pvr.regel_3 = sug.regel_3_a + sug.regel_3_b + sug.regel_3_c;
                pvr.regel_4 = sug.regel_4_a + sug.regel_4_b + sug.regel_4_c;
                pvr.regel_5 = sug.regel_5_a + sug.regel_5_b + sug.regel_5_c;
                pvr.regel_6 = sug.regel_6_a + sug.regel_6_b + sug.regel_6_c;
                pvr.regel_7 = sug.regel_7_a + sug.regel_7_b + sug.regel_7_c;
                pvr.regel_8 = sug.regel_8_a + sug.regel_8_b + sug.regel_8_c;
                pvr.regel_9 = sug.regel_9_a + sug.regel_9_b + sug.regel_9_c;
                pvr.regel_10 = sug.regel_10_a + sug.regel_10_b + sug.regel_10_c;
                pvr.regel_11 = sug.regel_11_a + sug.regel_11_b + sug.regel_11_c;
                pvr.regel_12 = sug.regel_12_a + sug.regel_12_b + sug.regel_12_c;
                pvr.regel_13 = sug.regel_13_a + sug.regel_13_b + sug.regel_13_c;
                pvr.regel_14 = sug.regel_14_a + sug.regel_14_b + sug.regel_14_c;
                pvr.regel_15 = sug.regel_15;
                pvr.regel_16 = sug.regel_16;
                pvr.regel_17 = sug.regel_17;
                pvr.regel_18 = sug.regel_18;
                pvr.regel_19 = sug.regel_19;
                pvr.regel_20 = sug.regel_20;
                pvr.regel_21 = sug.regel_21;
                pvr.regel_22 = sug.regel_22;
                pvr.regel_23 = sug.regel_23;
                pvr.regel_24 = sug.regel_24;
                pvr.regel_25 = sug.regel_25;
                pvr.regel_26 = sug.regel_26;
                pvr.regel_27 = sug.regel_27;
                pvr.regel_28 = sug.regel_28;
                pvr.regel_29 = sug.regel_29;
                pvr.regel_30 = sug.regel_30;
                pvr.regel_31 = sug.regel_31;
                pvr.regel_32 = sug.regel_32;
                pvr.regel_33 = sug.regel_33;

            });

            return pvr;
        }
        public Class_Suggestion getGenericSuggestionAsync(int reportCode)
        {
            // this might be implemented with hospital wide suggestions
            var sug = new Class_Suggestion();
            //var result = new Class_Suggestion();
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/language_file.xml");
            XDocument order = XDocument.Load(filename);

            IEnumerable<XElement> help = from d in order.Descendants("reports").Elements("text_by_type_of_surgery").Elements("soort")
                                         where d.Element("value").Value == reportCode.ToString()
                                         select d;
            //transfer from xml 
            foreach (XElement x in help)
            {
                sug.regel_1_a = x.Element("regel_1_a").Value;
                sug.regel_1_b = x.Element("regel_1_b").Value;
                sug.regel_1_c = x.Element("regel_1_c").Value;


                sug.regel_2_a = x.Element("regel_2_a").Value + x.Element("regel_2_b").Value + x.Element("regel_2_c").Value;

                sug.regel_3_a = x.Element("regel_3_a").Value + x.Element("regel_3_b").Value + x.Element("regel_3_c").Value;

                sug.regel_4_a = x.Element("regel_4_a").Value + x.Element("regel_4_b").Value + x.Element("regel_4_c").Value;

                sug.regel_5_a = x.Element("regel_5_a").Value + x.Element("regel_5_b").Value + x.Element("regel_5_c").Value;
                sug.regel_6_a = x.Element("regel_6_a").Value + x.Element("regel_6_b").Value + x.Element("regel_6_c").Value;
                sug.regel_7_a = x.Element("regel_7_a").Value + x.Element("regel_7_b").Value + x.Element("regel_7_c").Value;
                sug.regel_8_a = x.Element("regel_8_a").Value + x.Element("regel_8_b").Value + x.Element("regel_8_c").Value;
                sug.regel_9_a = x.Element("regel_9_a").Value + x.Element("regel_9_b").Value + x.Element("regel_9_c").Value;
                sug.regel_10_a = x.Element("regel_10_a").Value + x.Element("regel_10_b").Value + x.Element("regel_10_c").Value;
                sug.regel_11_a = x.Element("regel_11_a").Value + x.Element("regel_11_b").Value + x.Element("regel_11_c").Value;
                sug.regel_12_a = x.Element("regel_12_a").Value + x.Element("regel_12_b").Value + x.Element("regel_12_c").Value;
                sug.regel_13_a = x.Element("regel_13_a").Value + x.Element("regel_13_b").Value + x.Element("regel_13_c").Value;
                sug.regel_14_a = x.Element("regel_14_a").Value + x.Element("regel_14_b").Value + x.Element("regel_14_c").Value;
                sug.regel_15 = x.Element("regel_15").Value;
                sug.regel_16 = x.Element("regel_16").Value;
                sug.regel_17 = x.Element("regel_17").Value;
                sug.regel_18 = x.Element("regel_18").Value;
                sug.regel_19 = x.Element("regel_19").Value;
                sug.regel_20 = x.Element("regel_20").Value;
                sug.regel_21 = x.Element("regel_21").Value;
                sug.regel_22 = x.Element("regel_22").Value;
                sug.regel_23 = x.Element("regel_23").Value;
                sug.regel_24 = x.Element("regel_24").Value;
                sug.regel_25 = x.Element("regel_25").Value;
                sug.regel_26 = x.Element("regel_26").Value;
                sug.regel_27 = x.Element("regel_27").Value;
                sug.regel_28 = x.Element("regel_28").Value;
                sug.regel_29 = x.Element("regel_29").Value;
                sug.regel_30 = x.Element("regel_30").Value;
                sug.regel_31 = x.Element("regel_31").Value;
                sug.regel_32 = x.Element("regel_32").Value;
                sug.regel_33 = x.Element("regel_33").Value;
            }

            return sug;
        }
        private async Task<string> HarvestLocationAsync(int procedure_id)
        {
            var options = new List<Class_Item>();
            var result = "";
            options = await _drops.getCABGLeg();

            if (await _context.CABGS.AnyAsync(u => u.Id == procedure_id))
            {
                var selectedCABG = await _context.CABGS.Where(s => s.Id == procedure_id).FirstOrDefaultAsync();
                var loc = Convert.ToInt32(selectedCABG.leg_harvest_location);
                var help = options.Where(x => x.value == loc);
                foreach (Class_Item t in help) { result = t.description; };
                return result;
            }
            return result;
        }
        public Class_Suggestion getProcedureSpecificSuggestion(int v)
        {
            // look this up in the language xml file
            var sug = new Class_Suggestion();
            try
            {
                var contentRoot = _env.ContentRootPath;
                var filename = Path.Combine(contentRoot, "conf/language_file.xml");
                XDocument order = XDocument.Load(filename);

                IEnumerable<XElement> help = from d in order.Descendants("reports").Elements("text_by_type_of_surgery").Elements("soort")
                                             where d.Element("value").Value == v.ToString()
                                             select d;
                //transfer from xml 
                foreach (XElement x in help)
                {
                    sug.regel_1_a = x.Element("regel_1_a").Value + x.Element("regel_1_b").Value + x.Element("regel_1_c").Value;
                    sug.regel_2_a = x.Element("regel_2_a").Value + x.Element("regel_2_b").Value + x.Element("regel_2_c").Value;
                    sug.regel_3_a = x.Element("regel_3_a").Value + x.Element("regel_3_b").Value + x.Element("regel_3_c").Value;
                    sug.regel_4_a = x.Element("regel_4_a").Value + x.Element("regel_4_b").Value + x.Element("regel_4_c").Value;
                    sug.regel_5_a = x.Element("regel_5_a").Value + x.Element("regel_5_b").Value + x.Element("regel_5_c").Value;
                    sug.regel_6_a = x.Element("regel_6_a").Value + x.Element("regel_6_b").Value + x.Element("regel_6_c").Value;
                    sug.regel_7_a = x.Element("regel_7_a").Value + x.Element("regel_7_b").Value + x.Element("regel_7_c").Value;
                    sug.regel_8_a = x.Element("regel_8_a").Value + x.Element("regel_8_b").Value + x.Element("regel_8_c").Value;
                    sug.regel_9_a = x.Element("regel_9_a").Value + x.Element("regel_9_b").Value + x.Element("regel_9_c").Value;
                    sug.regel_10_a = x.Element("regel_10_a").Value + x.Element("regel_10_b").Value + x.Element("regel_10_c").Value;
                    sug.regel_11_a = x.Element("regel_11_a").Value + x.Element("regel_11_b").Value + x.Element("regel_11_c").Value;
                    sug.regel_12_a = x.Element("regel_12_a").Value + x.Element("regel_12_b").Value + x.Element("regel_12_c").Value;
                    sug.regel_13_a = x.Element("regel_13_a").Value + x.Element("regel_13_b").Value + x.Element("regel_13_c").Value;
                    sug.regel_14_a = x.Element("regel_14_a").Value + x.Element("regel_14_b").Value + x.Element("regel_14_c").Value;
                    sug.regel_15 = x.Element("regel_15").Value;
                    sug.regel_16 = x.Element("regel_16").Value;
                    sug.regel_17 = x.Element("regel_17").Value;
                    sug.regel_18 = x.Element("regel_18").Value;
                    sug.regel_19 = x.Element("regel_19").Value;
                    sug.regel_20 = x.Element("regel_20").Value;
                    sug.regel_21 = x.Element("regel_21").Value;
                    sug.regel_22 = x.Element("regel_22").Value;
                    sug.regel_23 = x.Element("regel_23").Value;
                    sug.regel_24 = x.Element("regel_24").Value;
                    sug.regel_25 = x.Element("regel_25").Value;
                    sug.regel_26 = x.Element("regel_26").Value;
                    sug.regel_27 = x.Element("regel_27").Value;
                    sug.regel_28 = x.Element("regel_28").Value;
                    sug.regel_29 = x.Element("regel_29").Value;
                    sug.regel_30 = x.Element("regel_30").Value;
                    sug.regel_31 = x.Element("regel_31").Value;
                    sug.regel_32 = x.Element("regel_32").Value;
                    sug.regel_33 = x.Element("regel_33").Value;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException); }
            return sug;
        }
        public async Task<ReportHeaderDTO> mapToReportHeaderAsync(Class_Procedure proc)
        {
            var selectedProcedure = proc;
            var current_hospital_id = selectedProcedure.hospital.ToString();
            var current_hospital = await _hos.GetSpecificHospital(current_hospital_id);
            var current_user = await _user.GetUser(proc.SelectedSurgeon);

            var dto = new ReportHeaderDTO();

            dto.Id = selectedProcedure.ProcedureId;
            dto.hospital_image = current_hospital.imageUrl;

            var l = new List<string>();
            l = await this.getHeaderTextAsync(current_hospital_id);

            dto.hospital_city = l[0];
            dto.hospital_name = l[1];
            dto.hospital_number = l[2];
            dto.hospital_unit = l[3];
            dto.hospital_dept = l[4];

            dto.operation_date = selectedProcedure.DateOfSurgery;
            var help = await _emp.getSpecificEmployee(selectedProcedure.SelectedPerfusionist);
            dto.perfusionist = help.name.UppercaseFirst();
            dto.surgeon = current_user.KnownAs.UppercaseFirst();
            dto.physician = current_user.KnownAs.UppercaseFirst();
            help = await _emp.getSpecificEmployee(selectedProcedure.SelectedAnaesthesist);
            dto.anaesthesiologist = help.name.UppercaseFirst();
            var user = await _user.GetUser(selectedProcedure.SelectedAssistant);
            if (user != null) { dto.assistant = user.KnownAs.UppercaseFirst(); } else { dto.assistant = "n/a"; }
            dto.surgeon_picture = current_user.PhotoUrl;
            dto.diagnosis = "";
            dto.operation = selectedProcedure.Description;
            dto.title = "Operative Report";
            dto.Comment_1 = selectedProcedure.Comment1;
            dto.Comment_2 = selectedProcedure.Comment2;
            dto.Comment_3 = selectedProcedure.Comment3;

            return dto;
        }
        public async Task<Class_Final_operative_report> updateFinalReportAsync(Class_privacy_model pm, int procedure_id)
        {

            var help = new Class_Final_operative_report();
            help.procedure_id = procedure_id;

            Class_Procedure cp = await _context.Procedures.Include(a => a.ValvesUsed).FirstOrDefaultAsync(x => x.ProcedureId == help.procedure_id);

            // this is used to compile the final report from different sources

            ReportHeaderDTO currentHeader = await mapToReportHeaderAsync(cp);
            Class_Preview_Operative_report prev = await _context.Previews.FirstOrDefaultAsync(x => x.procedure_id == help.procedure_id);
            var loggedInUserId = getCurrentUserId();
            var loggedinUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == loggedInUserId);


            var report_code = Convert.ToInt32(this.getReportCode(cp.fdType));
            if (report_code == 1)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;



                Class_CABG cb = await getCabgDetailsAsync(procedure_id);


                help.Regel25 = translateCabgStuff(1, cb.B1_SITE);
                help.Regel26 = translateCabgStuff(2, cb.Q01);
                help.Regel27 = translateCabgStuff(4, cb.ANGLE01);
                help.Regel28 = translateCabgStuff(3, cb.DIAM01);

                help.Regel29 = translateCabgStuff(1, cb.B2_SITE);
                help.Regel30 = translateCabgStuff(2, cb.Q02);
                help.Regel31 = translateCabgStuff(4, cb.ANGLE02);
                help.Regel32 = translateCabgStuff(3, cb.DIAM02);

                help.Regel33 = translateCabgStuff(1, cb.B3_SITE);
                help.Regel34 = translateCabgStuff(2, cb.Q03);
                help.Regel35 = translateCabgStuff(4, cb.ANGLE03);
                help.Regel36 = translateCabgStuff(3, cb.DIAM03);

                help.Regel37 = translateCabgStuff(1, cb.B4_SITE);
                help.Regel38 = translateCabgStuff(2, cb.Q04);
                help.Regel39 = translateCabgStuff(4, cb.ANGLE04);
                help.Regel40 = translateCabgStuff(3, cb.DIAM04);

                help.Regel41 = translateCabgStuff(1, cb.B5_SITE);
                help.Regel42 = translateCabgStuff(2, cb.Q05);
                help.Regel43 = translateCabgStuff(4, cb.ANGLE05);
                help.Regel44 = translateCabgStuff(3, cb.DIAM05);

                help.Regel1 = translateCabgStuff(1, cb.B6_SITE);
                help.Regel2 = translateCabgStuff(2, cb.Q06);
                help.Regel3 = translateCabgStuff(4, cb.ANGLE06);
                help.Regel4 = translateCabgStuff(3, cb.DIAM06);

                help.Regel45 = "The course of the graft(s) is: " + cb.course;

                help = this.getGeneralCABGDetails(help, prev);

            }
            if (report_code == 2)
            {

                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;



                Class_CABG cb = await getCabgDetailsAsync(procedure_id);
                help.Regel25 = translateCabgStuff(1, cb.B1_SITE);
                help.Regel26 = translateCabgStuff(2, cb.Q01);
                help.Regel27 = translateCabgStuff(4, cb.ANGLE01);
                help.Regel28 = translateCabgStuff(3, cb.DIAM01);

                help.Regel29 = translateCabgStuff(1, cb.B2_SITE);
                help.Regel30 = translateCabgStuff(2, cb.Q02);
                help.Regel31 = translateCabgStuff(4, cb.ANGLE02);
                help.Regel32 = translateCabgStuff(3, cb.DIAM02);

                help.Regel33 = translateCabgStuff(1, cb.B3_SITE);
                help.Regel34 = translateCabgStuff(2, cb.Q03);
                help.Regel35 = translateCabgStuff(4, cb.ANGLE03);
                help.Regel36 = translateCabgStuff(3, cb.DIAM03);

                help.Regel37 = translateCabgStuff(1, cb.B4_SITE);
                help.Regel38 = translateCabgStuff(2, cb.Q04);
                help.Regel39 = translateCabgStuff(4, cb.ANGLE04);
                help.Regel40 = translateCabgStuff(3, cb.DIAM04);

                help.Regel41 = translateCabgStuff(1, cb.B5_SITE);
                help.Regel42 = translateCabgStuff(2, cb.Q05);
                help.Regel43 = translateCabgStuff(4, cb.ANGLE05);
                help.Regel44 = translateCabgStuff(3, cb.DIAM05);

                help.Regel1 = translateCabgStuff(1, cb.B6_SITE);
                help.Regel2 = translateCabgStuff(2, cb.Q06);
                help.Regel3 = translateCabgStuff(4, cb.ANGLE06);
                help.Regel4 = translateCabgStuff(3, cb.DIAM06);

                help.Regel45 = "The course of the graft(s) is: " + cb.course;

                help = this.getGeneralCABGDetails(help, prev);


            }
            if (report_code == 3)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                // get the valve where implant position is 'Aortic'
                Class_Valve cv = cp.ValvesUsed.FirstOrDefault(a => a.Implant_Position == "Aortic");


                if (cp.fdType == 3)
                { // aortic valve replacement
                    help.Regel25 = cv.MODEL;
                    help.Regel26 = cv.SIZE;
                    help.Regel27 = cv.SERIAL_IMP;
                }
                if (cp.fdType == 30)
                {// aortic valve replacement, minimally invasive approach
                    help.Regel25 = cv.MODEL;
                    help.Regel26 = cv.SIZE;
                    help.Regel27 = cv.SERIAL_IMP;
                }
                help = this.getGeneralDetails(help, prev);


            }
            if (report_code == 4)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                Class_Valve cv = cp.ValvesUsed.FirstOrDefault(a => a.Implant_Position == "Mitral");


                if (cp.fdType == 4)
                {// mitral valve replacement
                    help.Regel28 = cv.MODEL;
                    help.Regel29 = cv.SIZE;
                    help.Regel30 = cv.SERIAL_IMP;
                }
                if (cp.fdType == 41)
                {// mitral valve repair
                    help.Regel28 = cv.MODEL;
                    help.Regel29 = cv.SIZE;
                    help.Regel30 = cv.SERIAL_IMP;
                }
                help = this.getGeneralDetails(help, prev);

            }
            if (report_code == 5)
            {// double valve replacement

                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                // this will go in avr/mvr blok 2
                help.Regel31 = prev.regel_27;
                help.Regel32 = prev.regel_28;
                help.Regel33 = prev.regel_29;
                help.Regel34 = prev.regel_30;
                help.Regel35 = prev.regel_31;
                help.Regel36 = prev.regel_32;
                help.Regel37 = prev.regel_33;

                Class_Valve cv = cp.ValvesUsed.FirstOrDefault(a => a.Implant_Position == "Aortic");

                help.Regel25 = cv.MODEL;
                help.Regel26 = cv.SIZE;
                help.Regel27 = cv.SERIAL_IMP;


                Class_Valve cvm = cp.ValvesUsed.FirstOrDefault(a => a.Implant_Position == "Mitral");

                help.Regel28 = cvm.MODEL;
                help.Regel29 = cvm.SIZE;
                help.Regel30 = cvm.SERIAL_IMP;

                // this will go in avr/mvr blok 3
                help.Regel38 = prev.regel_15;
                help.Regel39 = prev.regel_16;
                help.Regel40 = prev.regel_17;
                help.Regel41 = prev.regel_18;
                help.Regel42 = prev.regel_19;
                help.Regel43 = prev.regel_20;
                help.Regel44 = prev.regel_21;
                help.Regel45 = prev.regel_22;
                help.Regel46 = prev.regel_23;




            }
            if (report_code == 6)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                help.Regel25 = prev.regel_9;
                help.Regel26 = prev.regel_10;
                help.Regel27 = prev.regel_11;
                help.Regel28 = prev.regel_12;

                help.Regel29 = prev.regel_13;
                help.Regel30 = prev.regel_14;
                help.Regel31 = prev.regel_15;
                help.Regel32 = prev.regel_16;

                help.Regel33 = prev.regel_17;

                help.Regel34 = prev.regel_18;
                help.Regel35 = prev.regel_19;
                help.Regel36 = prev.regel_20;

                help.Regel37 = prev.regel_21;
                help.Regel38 = prev.regel_22;
                help.Regel39 = prev.regel_23;

                /*  help.Regel40 = prev.regel_1;

                 help.Regel41 = prev.regel_1;
                 help.Regel42 = prev.regel_1;
                 help.Regel43 = prev.regel_1;
                 help.Regel44 = prev.regel_1;  */

                help = this.getGeneralDetails(help, prev);


            }


            help.HospitalUrl = currentHeader.surgeon_picture;
            help.HeaderText1 = currentHeader.hospital_name;
            help.HeaderText2 = currentHeader.hospital_unit;
            help.HeaderText3 = currentHeader.hospital_dept;
            help.HeaderText4 = currentHeader.hospital_city;
            help.HeaderText5 = "Hospital number: " + pm.MedicalRecordNumber;
            help.HeaderText6 = "National ID: ";
            help.HeaderText7 = "Patient name: " + pm.patientName;
            help.HeaderText8 = "Physician: " + currentHeader.surgeon.UppercaseFirst();
            help.HeaderText9 = "";

            help.Regel10 = pm.Diagnosis.UppercaseFirst();
            help.Regel11 = currentHeader.operation;
            help.Regel12 = currentHeader.operation_date.ToString();

            help.Regel13 = currentHeader.surgeon;
            help.Regel14 = currentHeader.assistant;
            help.Regel15 = currentHeader.anaesthesiologist;
            help.Regel16 = currentHeader.perfusionist;

            help.Comment1 = cp.Comment1;
            help.Comment2 = cp.Comment2;
            help.Comment3 = cp.Comment3;

            help.UserName = loggedinUser.UserName;

            /*   help.AorticLineA = "";
              help.AorticLineB = "";
              help.AorticLineC = "";

              help.MitralLineA = "";
              help.MitralLineB = "";
              help.MitralLineC = ""; */


            // now make it straight into pdf skipping the finalreport step

            transformToPDF(report_code,help);  

            // save PDF and mark the creation date, delete after 3 days

          //  _context.finalReports.Add(help);

          //  if (await _context.SaveChangesAsync() > 0) { return help; }

            return null;
        }

        private void transformToPDF(int Code,Class_Final_operative_report help)
        {
            throw new NotImplementedException();
        }

        private async Task<List<string>> getHeaderTextAsync(string current_hospital_id)
        {
            var help = new List<string>();
            // get this from the hospital details, so it will be changeable
            var sh = await _context.Hospitals.FirstOrDefaultAsync(x => x.HospitalNo == current_hospital_id.makeSureTwoChar());
            if (sh != null) // check if this id is in the list of hospitals
            {
                help.Add(sh.OpReportDetails1);
                help.Add(sh.OpReportDetails2);
                help.Add("Hospital No:");
                help.Add(sh.OpReportDetails4);
                help.Add(sh.OpReportDetails5);
            }





            /*  // this is the opreportdetails from the xml file
             var contentRoot = _env.ContentRootPath;
             var filename = Path.Combine(contentRoot, "data/config/reportHeader.xml");
             XDocument order = XDocument.Load(filename);
             IEnumerable<XElement> h = from d in order.Descendants("hospital")
                                       where d.Attribute("id").Value == current_hospital_id.makeSureTwoChar()
                                       select d;
             foreach (XElement x in h)
             {
                 help.Add(x.Element("items").Element("reportHeader01").Value);
                 help.Add(x.Element("items").Element("reportHeader02").Value);
                 help.Add(x.Element("items").Element("reportHeader03").Value);
                 help.Add(x.Element("items").Element("reportHeader04").Value);
                 help.Add(x.Element("items").Element("reportHeader05").Value);
             } */
            return help;
        }



        private async Task<Class_CABG> getCabgDetailsAsync(int procedure_id)
        {
            var help = await _context.CABGS.FirstOrDefaultAsync(x => x.PROCEDURE_ID == procedure_id);
            return help;
        }
        private async Task<Class_Valve> getValvesDetailsAsync(string serial)
        {
            var help = await _context.Valves.FirstOrDefaultAsync(x => x.SERIAL_IMP == serial);
            return help;
        }
        private string translateCabgStuff(int soort, string test)
        {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/language_file.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("cabg") select d;
            foreach (XElement x in help)
            {

                switch (soort)
                {
                    // locatie
                    case 1:
                        IEnumerable<XElement> rm = from d in order.Descendants("locatie").Elements("items") select d;
                        foreach (XElement el in rm)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;
                    // quality
                    case 2:
                        IEnumerable<XElement> rm1 = from d in order.Descendants("quality").Elements("items") select d;
                        foreach (XElement el in rm1)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;
                    // diameter
                    case 3:
                        IEnumerable<XElement> rm2 = from d in order.Descendants("diameter").Elements("items") select d;
                        foreach (XElement el in rm2)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;
                    // angle
                    case 4:
                        IEnumerable<XElement> rm3 = from d in order.Descendants("angle").Elements("items") select d;
                        foreach (XElement el in rm3)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;


                }





            }
            return result;
        }
        private Class_Final_operative_report getGeneralDetails(Class_Final_operative_report help, Class_Preview_Operative_report prev)
        {

            help.Regel46 = prev.regel_15;
            help.Regel47 = prev.regel_16;
            help.Regel48 = prev.regel_17;
            help.Regel49 = prev.regel_18;
            help.Regel50 = prev.regel_19;

            help.Regel51 = prev.regel_20;
            help.Regel52 = prev.regel_21;
            help.Regel53 = prev.regel_22;
            help.Regel54 = prev.regel_23;

            help.Regel58 = prev.regel_9;
            help.Regel59 = prev.regel_10;
            help.Regel60 = prev.regel_11;

            help.Regel61 = prev.regel_12;
            help.Regel62 = prev.regel_13;
            help.Regel63 = prev.regel_14;

            help.Regel31 = prev.regel_27;
            help.Regel32 = prev.regel_28;
            help.Regel33 = prev.regel_29;
            help.Regel34 = prev.regel_30;
            help.Regel35 = prev.regel_31;
            help.Regel36 = prev.regel_32;




            return help;

        }
        private Class_Final_operative_report getGeneralCABGDetails(Class_Final_operative_report help, Class_Preview_Operative_report prev)
        {
            help.Regel46 = prev.regel_15;
            help.Regel47 = prev.regel_16;
            help.Regel48 = prev.regel_17;
            help.Regel49 = prev.regel_18;
            help.Regel50 = prev.regel_19;

            help.Regel51 = prev.regel_20;
            help.Regel52 = prev.regel_21;
            help.Regel53 = prev.regel_22;
            help.Regel54 = prev.regel_23;

            help.Regel58 = prev.regel_9;
            help.Regel59 = prev.regel_10;
            help.Regel60 = prev.regel_11;

            help.Regel61 = prev.regel_12;
            help.Regel62 = prev.regel_13;
            help.Regel63 = prev.regel_14;
            return help;

        }

    }


}