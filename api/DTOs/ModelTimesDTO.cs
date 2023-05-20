using System;

namespace api.DTOs
{
    public class ModelTimesDTO
    {
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        public int beginHour { get; set; }
        public int endHour { get; set; }
    }
}