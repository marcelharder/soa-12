using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.interfaces.reports;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Serialization;

namespace api.Implementations.reports
{
    public class ManageFinalReport : IManageFinalReport
    {
        private readonly IWebHostEnvironment _env;
        private readonly SpecialReportMaps _rm;
        private DataContext _context;



        public ManageFinalReport(IWebHostEnvironment env, SpecialReportMaps rm, DataContext context)
        {
            _env = env;
            _rm = rm;
            _context = context;
         
        }
        public int deletePDF(int id){
           var id_string = id.ToString();
            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var file_name = pathToFile + id_string + ".pdf";

            if (System.IO.File.Exists(file_name))
            {
                System.IO.File.Delete(file_name);
                System.Threading.Thread.Sleep(20);
            }
            return 1;  
        }
        public int addToExpiredReports(ReportTiming rt)
        {
            var l = GetXmlDetails();
            bool changesMade = false;

            if (!l.Any(r => r.id == rt.id))
            {
                l.Add(rt);
                changesMade = true;
            }

            if (changesMade)
            {
                SaveXmlDetails(l);
            }

            return 3;
        }
        public async Task<bool> isReportExpired(int id)
        {
            var result = false;
            // find out if this report is expired
            await Task.Run(() =>
            {
                var l = new List<ReportTiming>();
                l = GetXmlDetails();  // load the xml file im a list of ReportTimings
                foreach (ReportTiming rep in l)
                {
                    if (rep.id == id)
                    {
                        var currentTicks = DateTime.Now.Ticks;
                        var interval = 2592000000000; // interval is set to 3 days for now
                        if ((rep.publishTime.Ticks + interval) < currentTicks)
                        {
                            // report is expired
                            result = true;
                        }
                        else { result = false; }
                    }
                }
            });
            return result;
        }
        public int deleteExpiredReports()
        {
            // this is called by a CRON job and checks if there are expired reports, which are then deleted
            var currentTicks = DateTime.UtcNow.Ticks; // use UTC time instead of local time
            var interval = TimeSpan.FromDays(3).Ticks; // use TimeSpan to define interval

            try
            {
                // load the xml file im a list of ReportTimings
                var reportTimings = GetXmlDetails();

                // filter on expired report timings
                var expiredReportTimings = reportTimings.Where(rt => (rt.publishTime.Ticks + interval) < currentTicks).ToList();
                foreach (ReportTiming rt in expiredReportTimings) { deletePDF(rt.id); } // delete expired pdf's

                // filter out expired report timings
                var newReportTimings = reportTimings.Where(rt => (rt.publishTime.Ticks + interval) >= currentTicks).ToList();

                // write new report timings to file
                SaveXmlDetails(newReportTimings);

                return 1;
            }
            catch (Exception ex)
            {
                // log the exception instead of returning ret value
                Console.WriteLine("Failed to delete expired reports: " + ex.Message);
                return 2;
            }
        }
        private List<ReportTiming> GetXmlDetails()
        {
            // load the xml file into a list of ReportTimings
            var pathToFile = Path.Combine(_env.ContentRootPath, "conf", "timingsRefReport.xml");

            if (!File.Exists(pathToFile))
            {
                return new List<ReportTiming>();
            }

            using (var stream = File.Open(pathToFile, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<ReportTiming>));
                return (List<ReportTiming>)serializer.Deserialize(stream);
            }
        }
        private void SaveXmlDetails(List<ReportTiming> reportTimings)
        {
            // save the list of ReportTimings to an xml file
            var pathToFile = Path.Combine(_env.ContentRootPath, "conf", "timingsRefReport.xml");

            using (var stream = File.Open(pathToFile, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<ReportTiming>));
                serializer.Serialize(stream, reportTimings);
            }
        }
        public async Task<bool> pdfDoesNotExists(string id_string)
        {
            var result = false;
            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var file_name = pathToFile + id_string + ".pdf";
            await Task.Run(()=>{ if (System.IO.File.Exists(file_name)){result = false;} else { result = true;}});
           return result;
        }
    
    }
}
