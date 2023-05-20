using System;

namespace api.DTOs
{
    public class DischargeForReturnDTO
    {
        public int patient_id { get; set; }
        public int procedure_id { get; set; }
        public int dead { get; set; }
        public short dead_location { get; set; }
        public short dead_cause { get; set; }
        public DateTime dead_date { get; set; }
        public string discharged_to { get; set; }
        public string full_description { get; set; }
        public short discharge_diagnosis { get; set; }
        public string discharge_activities { get; set; }
        public DateTime discharge_date { get; set; }

    }
}
