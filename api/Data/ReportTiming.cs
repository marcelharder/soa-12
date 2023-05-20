using System;

namespace api.Data
{
    public  class ReportTiming
    {
        public int id { get; set; }
        public DateTime publishTime { get; set; }
        public string fileLocation { get; set; }
    }
}